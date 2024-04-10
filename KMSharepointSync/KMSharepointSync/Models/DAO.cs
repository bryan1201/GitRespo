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
        private string _TaskId { get; set; }

        public DAO(string _option, string taskId)
        {
            _TaskId = string.IsNullOrEmpty(taskId) ? string.Empty : taskId;
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

            string sqlCmd = string.Format("DELETE TmpSharePointFolder WHERE TaskId = '{0}'", _TaskId);
            EXECSqlCmd(sqlCmd);

            foreach (SharepointFolder spf in splist)
            {
                sqlCmd = "INSERT INTO TmpSharePointFolder(TaskId, Id, Name, Path, Parent, Url, ServerRelativeUrl) VALUES({0})";
                string sqlvalues = string.Format("N'{0}',N'{1}',N'{2}',N'{3}',N'{4}', N'{5}', N'{6}'", _TaskId, spf.Id, spf.Name, spf.Path, spf.Parent, spf.Url, spf.ServerRelativeUrl);
                sqlCmd = string.Format(sqlCmd, sqlvalues);
                EXECSqlCmd(sqlCmd);
            }
        }

        public void UpdateSharepointFolderPath(string rootspfolderId = @"8DC40534-03C9-491A-90DD-D16416382E93")
        {
            //EXEC[sp_UpdateSharePointFolder] '8DC40534-03C9-491A-90DD-D16416382E93'

            string sqlCmd = "EXEC KMDBAPIPRD.dbo.sp_UpdateSharePointFolder @SharePointRootId = '{0}', @TaskId = '{1}'";
            sqlCmd = string.Format(sqlCmd, rootspfolderId, _TaskId);
            EXECSqlCmd(sqlCmd);
        }

        public void RenewSharePointFile(List<SharepointFile> spfilelist)
        {
            if (spfilelist is null)
                return;
            if (spfilelist.Count == 0)
                return;

            string sqlCmd = string.Format("DELETE SharePointFile WHERE TaskId='{0}'", _TaskId);
            EXECSqlCmd(sqlCmd);

            foreach (SharepointFile item in spfilelist)
            {
                sqlCmd = "INSERT INTO SharePointFile(TaskId, UniqueId,ParentUniqueId,ServerRedirectedEmbedUri,FileLeafRef,FileDirRef,FileRef,Author,Editor,Created,Modified) VALUES({0})";
                string sqlvalues = string.Format("N'{0}',N'{1}',N'{2}',N'{3}',N'{4}', N'{5}', N'{6}', N'{7}', N'{8}', N'{9}',N'{10}'", _TaskId, item.UniqueId, item.ParentUniqueId, item.ServerRedirectedEmbedUri, item.FileLeafRef, item.FileDirRef, item.FileRef, item.Author, item.Editor, item.Created, item.Modified);
                sqlCmd = string.Format(sqlCmd, sqlvalues);
                EXECSqlCmd(sqlCmd);
            }
        }

        public void RenewKMFolderPath(int rootkmfolderId = 39575)
        {
            //EXEC KMDBAPIPRD.dbo.sp_UpdateKMFolder @FolderId = 39575
            string sqlCmd = "EXEC KMDBAPIPRD.dbo.sp_UpdateKMFolder @FolderId = {0}, @TaskId='{1}'";
            sqlCmd = string.Format(sqlCmd, rootkmfolderId, _TaskId);
            EXECSqlCmd(sqlCmd);
        }

        public IEnumerable<SharepointKM_FolderPathMapping> GetSharepointKM_FolderPathMapping()
        {
            string sqlCmd = string.Format("SELECT MA.* FROM KMDBAPIPRD.dbo.v_SharepointKM_FolderPathMapping MA WHERE MA.TaskId = '{0}'", _TaskId);
            //sqlCmd = string.Format(sqlCmd, rootkmfolderId);
            DataTable dt = EXECSqlCmdDataTable(sqlCmd);
            SharepointKM_FolderPathMapping foldermapping = new SharepointKM_FolderPathMapping();
            return (IEnumerable<SharepointKM_FolderPathMapping>)foldermapping.ConvertToTankReading(dt);
            //return ConvertToTankReadingSharepointKM_FolderPathMapping(dt);
        }

        public IEnumerable<SharepointKM_FilePathMapping> GetSharepointKM_FilePathMapping()
        {
            string sqlCmd = string.Format("SELECT MA.* FROM KMDBAPIPRD.dbo.v_SharepointKM_FilePathMapping MA WHERE TaskId='{0}'", _TaskId);
            DataTable dt = EXECSqlCmdDataTable(sqlCmd);
            SharepointKM_FilePathMapping filemapping = new SharepointKM_FilePathMapping();
            return (IEnumerable<SharepointKM_FilePathMapping>)filemapping.ConvertToTankReading(dt);
            //return ConvertToTankReadingSharepointKM_FilePathMapping(dt);
        }

        public IEnumerable<SyncTaskInfo> GetSyncTaskInfoList()
        {
            string sqlCmd = "SELECT * FROM KMDBAPIPRD.dbo.SyncTaskInfo";
            DataTable dt = EXECSqlCmdDataTable(sqlCmd);
            SyncTaskInfoList stinfo = new SyncTaskInfoList();
            return (IEnumerable<SyncTaskInfo>)stinfo.ConvertToTankReading(dt);
        }

        public IEnumerable<SyncTaskInfoLog> GetSyncTaskInfoLogList()
        {
            string sqlCmd = "SELECT * FROM KMDBAPIPRD.dbo.SyncTaskInfoLog";
            DataTable dt = EXECSqlCmdDataTable(sqlCmd);
            SyncTaskInfoLogList stinfolog = new SyncTaskInfoLogList();
            return (IEnumerable<SyncTaskInfoLog>)stinfolog.ConvertToTankReading(dt);
        }

        public bool AddSyncTaskInfoLog(SyncTaskInfoLog item) 
        { 
            bool result = true;
            try
            {
                string sqlCmd = "INSERT INTO SyncTaskInfoLog(TaskId,UserId,LogMessage,StatusCode,StatusDescription,StartDateTime,EndDateTime) VALUES({0})";
                string sqlvalues = string.Format("N'{0}',N'{1}',N'{2}',N'{3}',N'{4}', N'{5}', N'{6}'", item.TaskId, item.UserId, item.LogMessage, item.StatusCode, item.StatusDescription, item.SetDateTimeToString(item.StartDateTime), item.SetDateTimeToString(item.EndDateTime));
                sqlCmd = string.Format(sqlCmd, sqlvalues);
                EXECSqlCmd(sqlCmd);
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                result = false;
            }

            return result;
        }
    }
}