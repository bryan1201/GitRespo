using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace KMSharepointSync.Models
{
    public class KMService
    {
        public static String DataFormat = ConfigurationManager.AppSettings["DataFormat"];
        public static string KMUserId = ConfigurationManager.AppSettings["KMUserId"];
        public static string API_Key = ConfigurationManager.AppSettings["API_Key"];
        public static string TENANT = ConfigurationManager.AppSettings["TENANT"]; //psg
        public static string KMPUrl = ConfigurationManager.AppSettings["KMPUrl"];   // (必須)KM Server Site的API虛擬目錄URL路徑         
        private static string GlobalCurrentDocumentId = "";        // 暫時無作用,不需要去動它
        private static string GlobalCurrentCategoryId = "1";       // 暫時無作用,不需要去動它
        private static string GlobalSearchKeyword = "KM";

        public static string GetServiceUrl(ServiceType serviceType, string docId, string userId, string tenant)
        {
            StringBuilder sb = new StringBuilder();
            string ServiceUrl = "";

            switch (serviceType)
            {
                case ServiceType.GetRootFolder:
                    ServiceUrl = GetKmUrl() + "folder/root/public?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.UploadFile:
                    ServiceUrl = GetKmUrl() + "upload?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.AcquireDocumentDraft:
                    ServiceUrl = GetKmUrl() + "document/acquirenewdocumentdraft?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.SubmitNewDocument:
                    ServiceUrl = GetKmUrl() + "document/new?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.AllDocumentClass:
                    ServiceUrl = GetKmUrl() + "documentclass/all/enabled?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetCategoryInfo:
                    ServiceUrl = GetKmUrl() + "category/" + GlobalCurrentCategoryId + "?load_path=False&shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetSearchExBySimple:
                    ServiceUrl = GetKmUrl() + "search/ext/simple/" + GlobalSearchKeyword + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetAdvancedResult:
                    ServiceUrl = GetKmUrl() + "search/advancedresult" + "?shell=true&tid=0&api_key=" + API_Key + "&who=" + userId + "&format=" + DataFormat + "&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetDocument:
                    ServiceUrl = GetKmUrl() + "document/" + GlobalCurrentDocumentId.ToString() + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.AddUser:
                    ServiceUrl = GetKmUrl() + "user/add" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetUser:
                    ServiceUrl = GetKmUrl() + "user/{0}" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetUserBySubjectId:
                    ServiceUrl = GetKmUrl() + "user/exact/{0}" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.UpdateUser:
                    ServiceUrl = GetKmUrl() + "user/update" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.DeleteUser:
                    ServiceUrl = GetKmUrl() + "user/delete/{0}" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetAllUser:
                    ServiceUrl = GetKmUrl() + "user/all" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.DownloadFile2:
                    ServiceUrl = GetKmUrl() + "download2/{0}" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.AcquireFolderDraft:
                    ServiceUrl = GetKmUrl() + "folder/draft/{0}" + "?shell=true&tid=0&pi=0&ps=10&api_key=" + KMService.API_Key + "&who=" + userId + "&format=" + DataFormat + "&tenant=" + tenant;
                    break;
                case ServiceType.AddFolder:
                    ServiceUrl = GetKmUrl() + "folder/new/{0}" + "?shell=true&tid=0&api_key=" + API_Key + "&who=" + userId + "&format=" + DataFormat + "&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetDocumentFileById:
                    /*
                     http://km.iec.inventec/ESP/api/document/file/10090?version_number=0&format=json&tid=0&who=IEC891652&tenant=psg&pi=0&ps=10&api_key=154e10710ea44cdaaaec9cb4f7910ddc
                     */
                    sb.Append(GetKmUrl());
                    sb.Append("document/file/{0}?version_number=0");
                    sb.Append(string.Format("&format={0}", DataFormat));
                    sb.Append(string.Format("&tid=0&who={0}", userId));
                    sb.Append(string.Format("&tenant={0}", tenant));
                    sb.Append(string.Format("&api_key={0}", API_Key));
                    ServiceUrl = string.Format(sb.ToString(),docId);
                    break;
                default:
                    break;
            }
            return ServiceUrl;
        }

        private static string GetKmUrl()
        {
            return KMPUrl;
        }
    }
}