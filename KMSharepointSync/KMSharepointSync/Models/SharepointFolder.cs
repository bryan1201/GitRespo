using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.SharePoint.Client;
using System.Security;

using System.Net;
using System.Collections;
using System.Collections.Specialized;
using Jayrock.Json;
using Jayrock.Json.Conversion;

namespace KMSharepointSync.Models
{
    public class SharepointFolder
    {
        public string Url { get; set; }
        public string ServerRelativeUrl { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Parent { get; set; }
    }

    public class SharepointFolderList
    {
        private const string KMPRDEnvironment = "PRD";
        private Web _web;
        public List<SharepointFolder> SPFolderList { get; set; }
        public SharepointFolderList()
        {
            SPFolderList = new List<SharepointFolder>();
        }

        public SharepointFolderList(Web web, List<ListItem> src_spfolderlist)
        {
            _web = web;
            SPFolderList = new List<SharepointFolder>();
            UpdateSPList(src_spfolderlist);
            DAOSPList(SPFolderList);
        }

        private void UpdateSPList(List<ListItem> splist)
        {
            foreach (ListItem listfolder in splist)
            {
                SharepointFolder fd = new SharepointFolder();
                fd.Url = _web.Url;
                fd.ServerRelativeUrl = _web.ServerRelativeUrl;
                fd.Id = listfolder.FieldValues["UniqueId"].ToString();
                fd.Name = listfolder.FieldValues["FileLeafRef"].ToString();
                fd.Path = listfolder.FieldValues["FileRef"].ToString();
                fd.Parent = listfolder.FieldValues["ParentUniqueId"].ToString();

                SPFolderList.Add(fd);
            }
        }

        private void DAOSPList(List<SharepointFolder> splist)
        {
            string sqlCmd = "DELETE TmpSharePointFolder";
            DAO.sqlCmd(sqlCmd, KMPRDEnvironment);

            foreach (SharepointFolder spf in splist) {
                sqlCmd = "INSERT INTO TmpSharePointFolder(Id, Name, Path, Parent, Url, ServerRelativeUrl) VALUES({0})";
                string sqlvalues = string.Format("'{0}','{1}','{2}','{3}','{4}', '{5}'", spf.Id, spf.Name, spf.Path, spf.Parent, spf.Url, spf.ServerRelativeUrl);
                sqlCmd = string.Format(sqlCmd, sqlvalues);
                DAO.sqlCmd(sqlCmd, "PRD");
            }
        }

        private void DAOUpdateKMFolder(int rootkmfolderId=39575)
        {
            //EXEC KMDBAPIPRD.dbo.sp_UpdateKMFolder @FolderId = 39575
            string sqlCmd = "EXEC KMDBAPIPRD.dbo.sp_UpdateKMFolder @FolderId = {0}";
            sqlCmd = string.Format(sqlCmd, rootkmfolderId);
            DAO.sqlCmd(sqlCmd, KMPRDEnvironment);
        }

        public Web GetSharepointFolder()
        {
            //Web web;
            string _url = @"https://inventeccorp.sharepoint.com/sites/msteams_a0f382_290647/";
            string kmadmpassword = "T@aiPei+km0988";
            string kmadmaccount = "IECKMPRDADM@inventec.com";
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

                List list = clientContext.Web.Lists.GetByTitle("文件");
                CamlQuery camlQuery = new CamlQuery();
                List<ListItem> listFiles = new List<ListItem>();
                List<ListItem> listFolders = new List<ListItem>();
                int iPage = 0;
                listFiles = GetAllItems(iPage, clientContext, list, camlQuery, listFiles, listFolders);

                //Dictionary<string, IEnumerable<Folder>> listsFolders;
                //Dictionary<string, IEnumerable<File>> listsFiles;
                //LoadContent(clientContext.Web, out listsFolders, out listsFiles);

                clientContext.Load(_web);
                clientContext.Load(_web, x => x.HasUniqueRoleAssignments);
                clientContext.ExecuteQuery();

                //SharepointFolderList splist = new SharepointFolderList(web, listFolders);
                SPFolderList = new List<SharepointFolder>();
                UpdateSPList(listFolders);
                DAOSPList(SPFolderList);
                DAOUpdateKMFolder(rootkmfolderId: 39575);
                //List list = clientContext.Web.Lists.GetByTitle("文件");
                //CamlQuery camlQuery = new CamlQuery();
                //camlQuery.ViewXml = @"<View Scope='RecursiveAll'><Query></Query></View>";

                //Folder ff = list.RootFolder;
                //FolderCollection fcol = list.RootFolder.Folders; // here you will save the folder info inside a Folder Collection list
                //List<string> lstFile = new List<string>();
                //FileCollection ficol = list.RootFolder.Files;   // here you will save the File names inside a file Collection list 
                // ------informational -------
                //clientContext.Load(ff);
                //clientContext.Load(list);
                //clientContext.Load(list.RootFolder);
                //clientContext.Load(list.RootFolder.Folders);
                //clientContext.Load(list.RootFolder.Files);
                //clientContext.ExecuteQuery();

            }
            return _web;
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

    }
}