using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Storage;

namespace SmartShopping.PhoneApp
{
    public class AppConfiguration
    {
        private const byte DEFAULT_DebugLevel = 0;
        private const short DEFAULT_BTFilterInRange = -60;
        private const short DEFAULT_BTFilterOutRange = -100;
        private const int DEFAULT_BTSamplingIntervalms = 1000;
        private const int DEFAULT_BTMinOutRangeTimeoutms = 5000;
        private const string DEFAULT_ServiceUrl = "https://<defaultservicename>.azurewebsites.net/";
        private const string DEFAULT_ContentId = "Content";
        private const ushort DEFAULT_BeaconCompanyId = 0xFFFF;
        private const string DEFAULT_BeaconPrefix = "SSBeacon";

        public byte DebugLevel = DEFAULT_DebugLevel;
        public string UserDataFile = null;
        public short BTFilterInRange = DEFAULT_BTFilterInRange;
        public short BTFilterOutRange = DEFAULT_BTFilterOutRange;
        public int BTSamplingIntervalms = DEFAULT_BTSamplingIntervalms;
        public int BTMinOutRangeTimeoutms = DEFAULT_BTMinOutRangeTimeoutms;
        public string ServiceUrl = DEFAULT_ServiceUrl;
        public string ContentId = null;
        public ushort BeaconCompanyId = DEFAULT_BeaconCompanyId;
        public string BeaconPrefix = DEFAULT_BeaconPrefix;

        public Dictionary<string, Scenario> Scenarios = null;

        public async Task<bool> LoadDataFromXml(string filename)
        {
            const string XMLDATA_RECORD_CONFIG = "config";
            string dataFileName = (filename == null)? @"SmartShoppingSample.Config.xml" : filename;
            bool isSuccess = false;
            Stream xmlStream = null;

            string serviceDefaultContentId = null;

            try
            {
                if (xmlStream == null)
                {
                    // Try to get menu from Picture folder next
                    try
                    {
                        var folder = KnownFolders.PicturesLibrary;
                        var file = await folder.GetFileAsync(dataFileName);
                        xmlStream = await file.OpenStreamForReadAsync();
                        UserDataFile = dataFileName;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

                if (xmlStream == null)
                {
                    // Try to get menu from App's current folder
                    try
                    {
                        var file = await Package.Current.InstalledLocation.GetFileAsync(@"Data\" + dataFileName);
                        xmlStream = await file.OpenStreamForReadAsync();
                        UserDataFile = @"Data\" + dataFileName;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }


                if (xmlStream == null)
                {
                    throw new FileNotFoundException("No configfile found!!", dataFileName);
                }

                XDocument dataxml = XDocument.Load(xmlStream);

                foreach (XElement element in dataxml.Descendants(XMLDATA_RECORD_CONFIG))
                {
                    XAttribute attr = null;
                    string key = null;
                    string value = null;
                    try
                    {
                        attr = element.Attribute("Key");
                        key = (attr == null) ? null : attr.Value;
                        if (key == null) continue;
                        key = key.Trim();
                        if (key.Length == 0) continue;

                        attr = element.Attribute("Value");
                        value = (attr == null) ? null : attr.Value;
                        // config.Add(key, value);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        continue;
                    }

                    if (key == null)
                        continue;

                    switch (key)
                    {
                        case "DebugLevel":
                            byte.TryParse(value, out DebugLevel);
                            break;
                        case "UserDataFile":
                            UserDataFile = value;
                            break;
                        case "BTFilterInRange":
                            short.TryParse(value, out BTFilterInRange);
                            break;
                        case "BTFilterOutRange":
                            short.TryParse(value, out BTFilterOutRange);
                            break;
                        case "BTSamplingIntervalms":
                            int.TryParse(value, out BTSamplingIntervalms);
                            break;
                        case "BTMinOutRangeTimeoutms":
                            int.TryParse(value, out BTMinOutRangeTimeoutms);
                            break;
                        case "ServiceBaseUrl":
                        {
                            ServiceUrl = value.ToLower();
                            break;
                        }
                        case "BeaconCompanyId":
                            UInt16.TryParse(value, NumberStyles.HexNumber | NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out BeaconCompanyId);
                            break;
                        case "BeaconPrefix":
                            BeaconPrefix = value;
                            break;
                        case "ContentId":
                            ContentId = value;
                            break;
                        default:
                            break;
                    }
                }
                if (ContentId == null)
                {
                    if (serviceDefaultContentId == null)
                        serviceDefaultContentId = DEFAULT_ContentId;
                    ContentId = serviceDefaultContentId;
                }

                Scenarios = Scenario.LoadDataXmlStream(dataxml);

                foreach (var scenario in Scenarios.Values)
                {
                    if (scenario.BTFilterInRange == 0 || scenario.BTFilterOutRange == 0)
                    {
                        scenario.BTFilterInRange = BTFilterInRange;
                        scenario.BTFilterOutRange = BTFilterOutRange;
                    }
                    if (scenario.BTSamplingIntervalms == 0)
                        scenario.BTSamplingIntervalms = BTSamplingIntervalms;
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return isSuccess;
        }
    }

    public class Scenario
    {
        public const byte DEFAULT_EMUMODE = 0; // default emulation off

        public string ID = null;
        public string DisplayName = null;
        public byte EmuAzure = DEFAULT_EMUMODE;
        public short BTFilterInRange = 0;
        public short BTFilterOutRange = 0;
        public int BTSamplingIntervalms = 0;

        public Scenario(string id)
        {
            ID = id;
        }

        public static Dictionary<string, Scenario> LoadDataXmlStream(XDocument dataxml)
        {
            const string XMLDATA_RECORD_SCENARIO = "scenario";
            Dictionary<string, Scenario> scenarios = new Dictionary<string, Scenario>();

            try
            {
                foreach (XElement element in dataxml.Descendants(XMLDATA_RECORD_SCENARIO))
                {
                    XAttribute attr = null;
                    string id = null;
                    string displayName = null;
                    byte emuAzure = DEFAULT_EMUMODE;
                    short btFilterInRange = 0;
                    short btFilterOutRange = 0;
                    int btSamplingIntervalms = 0;

                    try
                    {
                        attr = element.Attribute("ID");
                        id = (attr == null) ? null : attr.Value;
                        if (id == null) continue;
                        id = id.Trim();
                        if (id.Length == 0) continue;

                        attr = element.Attribute("DisplayName");
                        displayName = (attr == null) ? null : attr.Value;
                        displayName = displayName.Trim();
                        if (displayName.Length == 0) displayName = id;

                        attr = element.Attribute("EmuAzure");
                        byte.TryParse((attr == null) ? "" : attr.Value, out emuAzure);

                        attr = element.Attribute("BTFilterInRange");
                        short.TryParse((attr == null) ? "" : attr.Value, out btFilterInRange);

                        attr = element.Attribute("BTFilterOutRange");
                        short.TryParse((attr == null) ? "" : attr.Value, out btFilterOutRange);

                        attr = element.Attribute("BTSamplingIntervalms");
                        int.TryParse((attr == null) ? "" : attr.Value, out btSamplingIntervalms);


                        Scenario scenario = new Scenario(id);
                        scenario.DisplayName = displayName;
                        scenario.EmuAzure = emuAzure;
                        scenario.BTFilterInRange = btFilterInRange;
                        scenario.BTFilterOutRange = btFilterOutRange;
                        scenario.BTSamplingIntervalms = btSamplingIntervalms;

                        scenarios.Add(scenario.ID, scenario);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            if (scenarios.Count == 0)
            {
                Scenario defaultScenario = new Scenario("0");
                scenarios.Add(defaultScenario.ID, defaultScenario);
            }
            return scenarios;
        }
    }
}
