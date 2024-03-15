using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using KMSharepointSync.Models;

namespace KMSharepointSync.Models
{
    public class DAO
    {
        private string _connString = Constant.PRDDBContext;
        private string _qasConnectionString = Constant.QASDBContext;
        private string _prdConnectionString = Constant.PRDDBContext;
        public string KMDBEnvironment { get; set; }

        public DAO(string _option)
        {
            KMDBEnvironment = string.IsNullOrEmpty(_option) ? "PRD" : _option;
        }

        public void sqlCmd(string sql)
        {
            SqlConnection cn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandTimeout = 1800;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public DataTable sqlCmdDataTable(string sql)
        {
            SqlConnection cn = new SqlConnection(_connString);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandTimeout = 1800;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            da.Fill(ds, "datasetresult");
            DataTable dt = ds.Tables[0];

            return dt;
        }

        public void EXECSqlCmd(string sql)
        {
            string connstring = GetConnectionEnvironment(KMDBEnvironment);

            SqlConnection cn = new SqlConnection(connstring);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandTimeout = 1800;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public DataTable EXECSqlCmdDataTable(string sql)
        {
            string connstring = GetConnectionEnvironment(KMDBEnvironment);

            SqlConnection cn = new SqlConnection(connstring);
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandTimeout = 1800;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            da.Fill(ds, "datasetresult");
            DataTable dt = ds.Tables[0];

            return dt;
        }

        private string GetConnectionEnvironment(string option)
        {
            string connstring = string.Empty;
            switch (option)
            {
                case "QAS":
                    connstring = _qasConnectionString;
                    break;
                case "PRD":
                    connstring = _prdConnectionString;
                    break;
            }
            return connstring;
        }
        public void RenewTmpSharePointFolder(List<SharepointFolder> splist)
        {
            if (splist is null)
                return;
            if (splist.Count == 0)
                return;

            string sqlCmd = "DELETE TmpSharePointFolder";
            EXECSqlCmd(sqlCmd);

            foreach (SharepointFolder spf in splist)
            {
                sqlCmd = "INSERT INTO TmpSharePointFolder(Id, Name, Path, Parent, Url, ServerRelativeUrl) VALUES({0})";
                string sqlvalues = string.Format("'{0}','{1}','{2}','{3}','{4}', '{5}'", spf.Id, spf.Name, spf.Path, spf.Parent, spf.Url, spf.ServerRelativeUrl);
                sqlCmd = string.Format(sqlCmd, sqlvalues);
                EXECSqlCmd(sqlCmd);
            }
        }

        public void UpdateSharepointFolderPath(string rootspfolderId = @"8DC40534-03C9-491A-90DD-D16416382E93")
        {
            //EXEC[sp_UpdateSharePointFolder] '8DC40534-03C9-491A-90DD-D16416382E93'
            StringBuilder sbpara = new StringBuilder();
            sbpara.Append("'");
            sbpara.Append(rootspfolderId);
            sbpara.Append("'");

            string sqlCmd = "EXEC KMDBAPIPRD.dbo.sp_UpdateSharePointFolder @SharePointRootId = {0}";
            sqlCmd = string.Format(sqlCmd, sbpara.ToString());
            EXECSqlCmd(sqlCmd);
        }

        public void RenewSharePointFile(List<SharepointFile> spfilelist)
        {
            if (spfilelist is null)
                return;
            if (spfilelist.Count == 0)
                return;

            string sqlCmd = "DELETE SharePointFile";
            EXECSqlCmd(sqlCmd);

            foreach (SharepointFile item in spfilelist)
            {
                sqlCmd = "INSERT INTO SharePointFile(UniqueId,ParentUniqueId,ServerRedirectedEmbedUri,FileLeafRef,FileDirRef,FileRef,Author,Editor,Created,Modified) VALUES({0})";
                string sqlvalues = string.Format("'{0}','{1}','{2}','{3}','{4}', '{5}', '{6}', '{7}', '{8}', '{9}'", item.UniqueId, item.ParentUniqueId, item.ServerRedirectedEmbedUri, item.FileLeafRef, item.FileDirRef, item.FileRef, item.Author, item.Editor, item.Created, item.Modified);
                sqlCmd = string.Format(sqlCmd, sqlvalues);
                EXECSqlCmd(sqlCmd);
            }
        }

        public void RenewKMFolderPath(int rootkmfolderId = 39575)
        {
            //EXEC KMDBAPIPRD.dbo.sp_UpdateKMFolder @FolderId = 39575
            string sqlCmd = "EXEC KMDBAPIPRD.dbo.sp_UpdateKMFolder @FolderId = {0}";
            sqlCmd = string.Format(sqlCmd, rootkmfolderId);
            EXECSqlCmd(sqlCmd);
        }

        public IEnumerable<SharepointKM_FolderPathMapping> GetSharepointKM_FolderPathMapping()
        {
            string sqlCmd = "SELECT MA.* FROM KMDBAPIPRD.dbo.v_SharepointKM_FolderPathMapping MA";
            //sqlCmd = string.Format(sqlCmd, rootkmfolderId);
            DataTable dt = EXECSqlCmdDataTable(sqlCmd);
            return ConvertToTankReadingSharepointKM_FolderPathMapping(dt);
        }

        private IEnumerable<SharepointKM_FolderPathMapping> ConvertToTankReadingSharepointKM_FolderPathMapping(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new SharepointKM_FolderPathMapping
                {
                    KM_ParentId = Convert.ToString(row["KM_ParentId"]),
                    KM_ParentPath = Convert.ToString(row["KM_ParentPath"]),
                    KM_Id = Convert.ToString(row["KM_Id"]),
                    KM_Path = Convert.ToString(row["KM_Path"]),
                    SP_Id = Convert.ToString(row["SP_Id"]),
                    SP_Name = Convert.ToString(row["SP_Name"]),
                    SP_Path = Convert.ToString(row["SP_Path"]),
                    SP_IndentName = Convert.ToString(row["SP_IndentName"]),
                    SP_sortOrder = Convert.ToString(row["SP_sortOrder"]),
                    SP_Url = Convert.ToString(row["SP_Url"]),
                    SP_ServerRelativeUrl = Convert.ToString(row["SP_ServerRelativeUrl"])
                };
            }
        }

        public IEnumerable<SharepointKM_FilePathMapping> GetSharepointKM_FilePathMapping()
        {
            string sqlCmd = "SELECT MA.* FROM KMDBAPIPRD.dbo.v_SharepointKM_FilePathMapping MA";
            DataTable dt = EXECSqlCmdDataTable(sqlCmd);
            return ConvertToTankReadingSharepointKM_FilePathMapping(dt);
        }
        private IEnumerable<SharepointKM_FilePathMapping> ConvertToTankReadingSharepointKM_FilePathMapping(DataTable dataTable)
        {
            //SP_FileRef, KM_Path, SP_Author, SP_Editor, SP_Created, SP_Modified, SP_sortOrder
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new SharepointKM_FilePathMapping
                {
                    KM_DOCUMENT_ID = Convert.ToString(row["KM_DOCUMENT_ID"]),
                    KM_FolderId = Convert.ToString(row["KM_FolderId"]),
                    SP_FileLeafRef = Convert.ToString(row["SP_FileLeafRef"]),
                    SP_FileRef = Convert.ToString(row["SP_FileRef"]),
                    KM_Path = Convert.ToString(row["KM_Path"]),
                    SP_Author = Convert.ToString(row["SP_Author"]),
                    SP_Editor = Convert.ToString(row["SP_Editor"]),
                    SP_Created = Convert.ToString(row["SP_Created"]),
                    SP_Modified = Convert.ToString(row["SP_Modified"]),
                    SP_sortOrder = Convert.ToString(row["SP_sortOrder"])
                };
            }
        }
    }
}