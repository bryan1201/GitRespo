using Jayrock.Json;
using Jayrock.Json.Conversion;
using Microsoft.SharePoint.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Web;

namespace KMSharepointSync.Models
{
    public class SharepointFolderList
    {
        private const string KMPRDEnvironment = "PRD";
        private Web _web;
        //private string GlobalCurrentCategoryId = "1";
        private string JsonAcquireDraftFolderResult = string.Empty;
        private string JsonResultAddFolder = string.Empty;
        private string JsonAcquireDraftFolderResultIndent = string.Empty;
        private string _TaskId { get; set; }

        private List<SharepointFolder> SPFolderList { get; set; }
        private List<SharepointFile> SPFileList { get; set; }

        public SharepointFolderList(string taskId)
        {
            _TaskId = taskId;
            SPFolderList = new List<SharepointFolder>();
            SPFileList = new List<SharepointFile>();
        }

        public SharepointFolderList(Web web, List<ListItem> src_spfolderlist)
        {
            _web = web;
            SPFolderList = new List<SharepointFolder>();
            SPFileList = new List<SharepointFile>();
            UpdateSPFolderList(src_spfolderlist);
        }

        private void UpdateSPFolderList(List<ListItem> splist)
        {
            foreach (ListItem listfolder in splist)
            {
                SharepointFolder fd = new SharepointFolder();
                fd.TaskId = _TaskId;
                fd.Url = _web.Url;
                fd.ServerRelativeUrl = _web.ServerRelativeUrl;
                fd.Id = listfolder.FieldValues["UniqueId"].ToString();
                fd.Name = listfolder.FieldValues["FileLeafRef"].ToString();
                fd.Path = listfolder.FieldValues["FileRef"].ToString();
                fd.Parent = listfolder.FieldValues["ParentUniqueId"].ToString();

                SPFolderList.Add(fd);
            }
        }

        private void UpdateSPFileList(List<ListItem> spfilelist)
        {
            foreach (ListItem item in spfilelist)
            {
                SharepointFile sf = new SharepointFile();
                sf.TaskId = _TaskId;
                sf.UniqueId = item.FieldValues["UniqueId"].ToString();
                sf.ParentUniqueId = item.FieldValues["ParentUniqueId"].ToString();
                sf.ServerRedirectedEmbedUri = item.ServerRedirectedEmbedUri;
                sf.FileLeafRef = item.FieldValues["FileLeafRef"].ToString();
                sf.FileDirRef = item.FieldValues["FileDirRef"].ToString();
                sf.FileRef = item.FieldValues["FileRef"].ToString();
                sf.Author = ((FieldUserValue)item.FieldValues["Author"]).LookupValue;
                sf.Editor = ((FieldUserValue)item.FieldValues["Editor"]).LookupValue;
                sf.Created = ((DateTime)item.FieldValues["Created"]).ToString("yyyyMMddHHmmss");
                sf.Modified = ((DateTime)item.FieldValues["Modified"]).ToString("yyyyMMddHHmmss");
                SPFileList.Add(sf);

                /*
                    ((FieldUserValue)item.FieldValues["Author"]).LookupValue
                    "Jiang.Joe 蔣鎧駿 IEC1"

                    ((DateTime)item.FieldValues["Created"]).ToString()
                    "2024/2/6 上午 08:29:01"

                    ((DateTime)item.FieldValues["Modified"]).ToString()
                    "2024/2/6 上午 08:29:01"
                */
            }
        }


        public string GetJsonAcquireDraftFolderResult()
        {
            return JsonAcquireDraftFolderResult;
        }
        public string GetJsonResultAddFolder()
        {
            return JsonResultAddFolder;
        }

        public string GetJsonAcquireDraftFolderResultIndent()
        {
            return JsonAcquireDraftFolderResultIndent;
        }


        public void GetSharepointFolder(string url, string kmadm, string kmpwd, string spSharedDocLib = "文件", int kmRootFolderId = 39575)
        {
            //Web web;
            string _url = url;
            string kmadmpassword = kmpwd;//"T@aiPei+km0988";
            string kmadmaccount = kmadm;
            using (ClientContext clientContext = new ClientContext(_url))
            {
                SecureString passWord = new SecureString();
                foreach (char c in kmadmpassword.ToCharArray()) passWord.AppendChar(c);
                clientContext.Credentials = //System.Net.CredentialCache.DefaultCredentials;
                             new SharePointOnlineCredentials(kmadmaccount, passWord);
                _web = clientContext.Web;
                var lists = clientContext.LoadQuery(_web.Lists.Where(l => l.BaseType == BaseType.DocumentLibrary));
                clientContext.ExecuteQuery();
                clientContext.RequestTimeout = -1;

                List list = clientContext.Web.Lists.GetByTitle(spSharedDocLib); //spSharedDocLib = "文件";

                clientContext.Load(list.Fields);

                // We must call ExecuteQuery before enumerate list.Fields.
                clientContext.ExecuteQuery();

                CamlQuery camlQuery = new CamlQuery();
                List<ListItem> listFiles = new List<ListItem>();
                List<ListItem> listFolders = new List<ListItem>();
                int iPage = 0;

                listFiles = GetAllItems(iPage, clientContext, list, camlQuery, listFiles, listFolders);

                clientContext.Load(_web);
                clientContext.Load(_web, x => x.HasUniqueRoleAssignments);
                clientContext.ExecuteQuery();

                UpdateSPFolderList(listFolders);
                UpdateSPFileList(listFiles);

                DAO dbaccess = new DAO(KMPRDEnvironment, taskId:_TaskId);

                dbaccess.RenewTmpSharePointFolder(splist: SPFolderList);

                string ParentUniqueId = listFolders[0].FieldValues["ParentUniqueId"].ToString().Replace("{", "").Replace("}", "");
                dbaccess.UpdateSharepointFolderPath(rootspfolderId: ParentUniqueId);
                dbaccess.RenewSharePointFile(spfilelist: SPFileList);
                dbaccess.RenewKMFolderPath(rootkmfolderId: kmRootFolderId); //kmRootFolderId= 39575
            }
            //return _web;
        }

        public List<ListItem> GetAllItems(int page, ClientContext Context, List list, CamlQuery camlQuery, List<ListItem> ListFiles, List<ListItem> ListFolders)
        {
            camlQuery.ViewXml =
            @"< View Scope = 'RecursiveAll'>
                < Query >
                    <Where>
                  </Where>
                    <OrderBy>
                        <FieldRef Name='ID' />
                    </OrderBy>
                </ Query >
            </ View >";

            ListItemCollection AllItems = list.GetItems(camlQuery);
            try
            {
                Context.Load(AllItems);
                Context.ExecuteQuery();
                foreach (ListItem item in AllItems)
                {
                    if (item.FileSystemObjectType == FileSystemObjectType.File)
                    {
                        ListFiles.Add(item);
                        /*
                         * item.ServerRedirectedEmbedUri
                            "https://inventeccorp.sharepoint.com/sites/msteams_a0f382_290647/_layouts/15/Embed.aspx?UniqueId=0b03ef65-8a88-4436-a0f0-c86a6ef93f25"
                         * https://inventeccorp.sharepoint.com/sites/msteams_a0f382_290647/Shared Documents/DCAIC智慧醫材電子系統/081_E_管理審查會議紀錄(機密)/1_ASAICMP004-F002-E112001  管理審查會議紀錄.pdf
                         * item.FieldValues["FileLeafRef"]
                            "1_ASAICMP004-F002-E112001  管理審查會議紀錄.pdf"

                            item.FieldValues["FileDirRef"]
                            "/sites/msteams_a0f382_290647/Shared Documents/DCAIC智慧醫材電子系統/081_E_管理審查會議紀錄(機密)"
                            
                            item.FieldValues["FileRef"]
                            "/sites/msteams_a0f382_290647/Shared Documents/DCAIC智慧醫材電子系統/081_E_管理審查會議紀錄(機密)/1_ASAICMP004-F002-E112001  管理審查會議紀錄.pdf"
                         
                            ((FieldUserValue)item.FieldValues["Author"]).LookupValue
                            "Jiang.Joe 蔣鎧駿 IEC1"

                            ((DateTime)item.FieldValues["Created"]).ToString()
                            "2024/2/6 上午 08:29:01"

                            ((DateTime)item.FieldValues["Modified"]).ToString()
                            "2024/2/6 上午 08:29:01"
                         */
                    }

                    if (item.FileSystemObjectType == FileSystemObjectType.Folder)
                    {
                        if (!ListFolders.Contains(item))
                            ListFolders.Add(item);
                    }

                    if (item.FileSystemObjectType == FileSystemObjectType.Folder)
                    {
                        camlQuery.FolderServerRelativeUrl = item.FieldValues["FileRef"].ToString();

                        if (page < 2000)
                            GetAllItems(page + 1, Context, list, camlQuery, ListFiles, ListFolders);
                    }
                }
            }
            catch
            { //do nothing!
            }

            return ListFiles;
        }
        public List<SharepointFolder> GetSPFolderList()
        {
            return this.SPFolderList;
        }
        public List<SharepointFile> GetSPFileList()
        {
            return this.SPFileList;
        }
        public string GetWebUrl()
        {
            return _web.Url;
        }

        public string GetServerRelativeUrl()
        {
            return _web.ServerRelativeUrl;
        }
        public string GetWebTitle()
        {
            return _web.Title;
        }
        public string GetWebDescription()
        {
            return _web.Description;
        }

    }
}