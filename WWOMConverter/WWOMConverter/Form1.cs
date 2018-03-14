using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenericParsing;

namespace WWOMConverter
{
    public partial class FormWWOM : Form
    {
        public FormWWOM()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

            string sqlCmd = "EXEC sp_ClearWWOMData";
            DAO.sqlCmd(Constant.S_SqlConnStr, sql: sqlCmd);

            ImportData(Constant.S_SourceFileDept, Constant.S_DestTableDepts);
            ImportData(Constant.S_SourceFilePa, Constant.S_DestTableUsers);

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
