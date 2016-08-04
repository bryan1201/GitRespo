using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Storage;
using SmartShopping.PhoneApp;
using Windows.ApplicationModel.Background;

namespace SmartShopping.PhoneApp
{
    public class UserRecord
    {
        public string ID;
        public string DisplayName;

        public string ScenarioID; 

        public UserRecord(string id, string displayname,
            string scenario)
        {
            ID = id;
            DisplayName = displayname;
            ScenarioID = scenario;
        }
    };

    public enum USERLOGIN_STATUS
    {
        LOGGED_OUT,
        LOGGED_IN
    }

    public class UserStatus
    {
        public USERLOGIN_STATUS Status;
        public UserRecord Profile;
    }

    public class UserManager
    {

        public Dictionary<string, UserRecord> UserProfiles; // username -> UserRecord
        public UserStatus CurrentUser;

        public async Task<bool> LoadDataFromXml(string filename)
        {
            const string XMLDATA_RECORD_USER = "user";
            string dataFileName = filename;
            bool isSuccess = false;
            Stream xmlStream = null;

            if (UserProfiles == null)
                UserProfiles = new Dictionary<string, UserRecord>();
            else
                UserProfiles.Clear();

            CurrentUser = new UserStatus() { Status = USERLOGIN_STATUS.LOGGED_OUT, Profile = null };

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
                        var file = await Package.Current.InstalledLocation.GetFileAsync(dataFileName);
                        xmlStream = await file.OpenStreamForReadAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }


                if (xmlStream == null)
                {
                    throw new FileNotFoundException("No userdata found!!", dataFileName);
                }

                XDocument dataxml = XDocument.Load(xmlStream);

                foreach (XElement element in dataxml.Descendants(XMLDATA_RECORD_USER))
                {
                    XAttribute attr = null;
                    string id = null;
                    string displayname = null;
                    string scenario = "0";
                    try
                    {
                        attr = element.Attribute("ID");
                        id = (attr == null) ? null : attr.Value;
                        if (id == null) continue;
                        id = id.Trim();
                        if (id.Length == 0) continue;

                        attr = element.Attribute("DisplayName");
                        displayname = (attr == null) ? null : attr.Value;
                        displayname = displayname.Trim();
                        if (displayname == null || displayname.Length == 0)
                            displayname = "(" + id + ")";

                        attr = element.Attribute("Scenario");
                        scenario = (attr == null) ? null : attr.Value;
                        scenario = scenario.Trim();

                        UserRecord record = new UserRecord(id, displayname,
                            scenario);
                        UserProfiles.Add(id, record);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
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
}
