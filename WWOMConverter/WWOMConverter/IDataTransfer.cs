using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericParsing;
using System.Data;
using System.ComponentModel;

namespace WWOMConverter
{
    interface IDataTransfer
    {
        void ImportAction();
    }

    interface IDept
    {

    }

    interface IUser
    {

    }

    class ProcessData : IDataTransfer
    {
        public void ImportAction()
        {
            
            Method.CopyFiles(
                sourcePath: Constant.S_HRFileServerDept, 
                targetPath: Constant.S_SourceFileDept, MappingExt:Constant.S_MappingExt,
                copydate: System.DateTime.Now);

            Method.WriteLog(Constant.S_ProgramLog, "Method.CopyFiles " + Constant.S_HRFileServerDept + " completed");

            Method.CopyFiles(
                sourcePath: Constant.S_HRFileServerPa, 
                targetPath: Constant.S_SourceFilePa, 
                MappingExt:Constant.S_MappingExt, 
                copydate: System.DateTime.Now);

            Method.WriteLog(Constant.S_ProgramLog, "Method.CopyFiles " + Constant.S_HRFileServerPa + " completed");
            
            string sqlCmd = "EXEC sp_ClearWWOMData";
            DAO.sqlCmd(Constant.S_SqlConnStr, sql: sqlCmd);

            Method.WriteLog(Constant.S_ProgramLog, @"ImportAction() " + sqlCmd + " completed");

            ImportData(Constant.S_SourceFileDept, Constant.S_DestTableDepts);

            Method.WriteLog(Constant.S_ProgramLog, @"ImportData() "+ Constant.S_SourceFileDept + " completed");

            ImportData(Constant.S_SourceFilePa, Constant.S_DestTableUsers);

            Method.WriteLog(Constant.S_ProgramLog, @"ImportData() " + Constant.S_SourceFilePa + " completed");

            sqlCmd = "EXEC sp_SyncHR2WWOM";
            DAO.sqlCmd(Constant.S_SqlConnStr, sql: sqlCmd);

            Method.WriteLog(Constant.S_ProgramLog, @"ImportAction() " + sqlCmd + " completed");
        }
        private void ImportData(string sourcefilename, string desttablename)
        {
            // Or... programmatically setting up the parser for TSV. 
            int rows = System.IO.File.ReadAllLines(sourcefilename).Length;
            WWOMDBContext dbcontext = new WWOMDBContext();
            DataTable dt = new DataTable();
            using (GenericParserAdapter parser = new GenericParserAdapter())
            {
                parser.SetDataSource(sourcefilename);

                parser.ColumnDelimiter = Convert.ToChar(9);
                parser.FirstRowHasHeader = false;
                parser.SkipStartingDataRows = 0;
                parser.MaxBufferSize = 10240;
                parser.MaxRows = rows;
                parser.TextQualifier = '\"';

                dt = parser.GetDataTable();
            }

            DAO.DatatableToSQL(Constant.S_SqlConnStr, dt, desttablename);
        }
    }
}
