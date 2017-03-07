using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;

using ImportReport.Configuration;

namespace ImportReport
{
    public partial class FormMain : Form
    {
        protected ImportSection importConfiguration;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            const string filter = "Microsoft Office Excel 活頁簿 (*.xls) | *.xls";

            try
            {
                openFileDialog1.Filter = filter;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxFile.Text))
                openFileDialog1.FileName = textBoxFile.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBoxFile.Text = openFileDialog1.FileName;
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            HSSFWorkbook sourceWorkbook = null;
            IReportImporter importer;
            StringBuilder sql = new StringBuilder();

            try
            {
                textBoxSQL.Text = string.Empty;
                Cursor = Cursors.WaitCursor;

                if (string.IsNullOrEmpty(textBoxFile.Text.Trim()))
                    throw new Exception("請選擇檔案名稱與路徑！");

                importConfiguration = ConfigurationManager.GetSection(ImportSection.SECTION_NAME) as ImportSection;

                using (FileStream fileStream = new FileStream(textBoxFile.Text.Trim(), FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    sourceWorkbook = new HSSFWorkbook(fileStream);
                    fileStream.Close();
                }

                importer = new BaseImporter();

                for (int sheetIndex = 0; sheetIndex < sourceWorkbook.NumberOfSheets; sheetIndex++)
                {
                    HSSFSheet sourceSheet = sourceWorkbook.GetSheetAt(sheetIndex) as HSSFSheet;
                    foreach (HallElement hall in importConfiguration.Halls)
                    {
                        importer.Import(importConfiguration, dateTimePickerDate.Value.AddDays(sheetIndex * 7), hall.Name, sourceSheet, ref sql);
                        Application.DoEvents();
                    }
                }

                textBoxSQL.Text = sql.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}