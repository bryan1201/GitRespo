using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;

using TransformReport.Configuration;

namespace TransformReport
{
    public partial class FormMain : Form
    {
        protected readonly TransformSection transformConfiguration;
        protected IList<string> hallList;

        public FormMain()
        {
            InitializeComponent();
            transformConfiguration = ConfigurationManager.GetSection(TransformSection.SECTION_NAME) as TransformSection;
            IReportTransformer transformer = new BaseTransformer();
            hallList = transformer.GetHallsList(transformConfiguration);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            const string filter = "Microsoft Office Excel 活頁簿 (*.xls) | *.xls";

            try
            {
                openFileDialog1.Filter = filter;
                saveFileDialog1.Filter = filter;

                textBoxTemplate.Text = string.Format("{0}\\{1}", Application.StartupPath, transformConfiguration.TemplateExcelFile);
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

        private void buttonTransform_Click(object sender, EventArgs e)
        {
            HSSFWorkbook targetWorkbook = null;
            string filePath = string.Empty;
            float fSleep = 0.0f; int iSleep = 0;
            IReportTransformer transformer;

            try
            {
                float.TryParse(txtThreadSleep.Text, out fSleep);
                fSleep = fSleep * 1000f;
                int.TryParse(fSleep.ToString(),out iSleep);
            }
            catch { 
                //do nothing)
            }

            try
            {
                textBoxProgress.Text = string.Empty;
                textBoxProgress.Visible = true;
                Cursor = Cursors.WaitCursor;

                if (string.IsNullOrEmpty(textBoxOutput.Text.Trim()))
                    throw new Exception("請選擇輸出檔案名稱與路徑！");

                using (FileStream templateStream = new FileStream(textBoxTemplate.Text.Trim(), FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    targetWorkbook = new HSSFWorkbook(templateStream);
                    templateStream.Close();
                }

                
                transformer = new BaseTransformer();
                HSSFSheet targetSheet = targetWorkbook.GetSheetAt(0) as HSSFSheet;

                IList<string> halllist = transformer.GetHallsList(transformConfiguration);
                foreach (string hall in halllist)
                {
                    string rptAdult = string.Format("{0}A", hall);
                    string rptChild = string.Format("{0}C", hall);

                    filePath = transformer.GetListBoxItembyFindString(rptAdult, this.lstboxHallFiles);
                    textBoxProgress.Text = string.Format("處理{0}會所社區中…", hall);
                    textBoxProgress.Refresh();
                    transformer.Transform(transformConfiguration, hall, "社區", filePath, targetSheet);
                    Thread.Sleep(iSleep);
                    filePath = transformer.GetListBoxItembyFindString(rptChild, this.lstboxHallFiles);
                    textBoxProgress.Text = string.Format("處理{0}會所兒童中…", hall);
                    textBoxProgress.Refresh();
                    transformer.Transform(transformConfiguration, hall, "兒童", filePath, targetSheet);
                    Thread.Sleep(iSleep);
                }

                // 12會所社區
                /*
                textBoxProgress.Text = "處理12會所社區中…";
                Application.DoEvents();
                if (!string.IsNullOrEmpty(filePath = textBox12Adult.Text.Trim()))
                    transformer.Transform(transformConfiguration, "12", "社區", filePath, targetSheet);

                // 12會所兒童
                textBoxProgress.Text = "處理12會所兒童中…";
                Application.DoEvents();
                if (!string.IsNullOrEmpty(filePath = textBox12Child.Text.Trim()))
                    transformer.Transform(transformConfiguration, "12", "兒童", filePath, targetSheet);

                // 36會所社區
                textBoxProgress.Text = "處理36會所社區中…";
                Application.DoEvents();
                if (!string.IsNullOrEmpty(filePath = textBox36Adult.Text.Trim()))
                    transformer.Transform(transformConfiguration, "36", "社區", filePath, targetSheet);

                // 36會所兒童
                textBoxProgress.Text = "處理36會所兒童中…";
                Application.DoEvents();
                if (!string.IsNullOrEmpty(filePath = textBox36Child.Text.Trim()))
                    transformer.Transform(transformConfiguration, "36", "兒童", filePath, targetSheet);

                // 37會所社區
                textBoxProgress.Text = "處理37會所社區中…";
                Application.DoEvents();
                if (!string.IsNullOrEmpty(filePath = textBox37Adult.Text.Trim()))
                    transformer.Transform(transformConfiguration, "37", "社區", filePath, targetSheet);

                // 37會所兒童
                textBoxProgress.Text = "處理37會所兒童中…";
                Application.DoEvents();
                if (!string.IsNullOrEmpty(filePath = textBox37Child.Text.Trim()))
                    transformer.Transform(transformConfiguration, "37", "兒童", filePath, targetSheet);

                // 60會所社區
                textBoxProgress.Text = "處理60會所社區中…";
                Application.DoEvents();
                if (!string.IsNullOrEmpty(filePath = textBox60Adult.Text.Trim()))
                    transformer.Transform(transformConfiguration, "60", "社區", filePath, targetSheet);

                // 60會所兒童
                textBoxProgress.Text = "處理60會所兒童中…";
                Application.DoEvents();
                if (!string.IsNullOrEmpty(filePath = textBox60Child.Text.Trim()))
                    transformer.Transform(transformConfiguration, "60", "兒童", filePath, targetSheet);

                // 61會所社區
                textBoxProgress.Text = "處理61會所社區中…";
                Application.DoEvents();
                if (!string.IsNullOrEmpty(filePath = textBox61Adult.Text.Trim()))
                    transformer.Transform(transformConfiguration, "61", "社區", filePath, targetSheet);

                // 61會所兒童
                textBoxProgress.Text = "處理61會所兒童中…";
                Application.DoEvents();
                if (!string.IsNullOrEmpty(filePath = textBox61Child.Text.Trim()))
                    transformer.Transform(transformConfiguration, "61", "兒童", filePath, targetSheet);
                */

                using (FileStream targetStream = new FileStream(textBoxOutput.Text.Trim(), FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    targetWorkbook.Write(targetStream);
                    targetStream.Close();
                }
            }
#if RELEASE
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
            finally
            {
                Cursor = Cursors.Default;
                textBoxProgress.Visible = false;
            }
        }

        private void button12Adult_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox12Adult.Text.Trim()))
                    openFileDialog1.FileName = textBox12Adult.Text.Trim();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    textBox12Adult.Text = openFileDialog1.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button12Child_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox12Child.Text.Trim()))
                    openFileDialog1.FileName = textBox12Child.Text.Trim();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    textBox12Child.Text = openFileDialog1.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button36Adult_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox36Adult.Text.Trim()))
                    openFileDialog1.FileName = textBox36Adult.Text.Trim();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    textBox36Adult.Text = openFileDialog1.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button36Child_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox36Child.Text.Trim()))
                    openFileDialog1.FileName = textBox36Child.Text.Trim();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    textBox36Child.Text = openFileDialog1.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button37Adult_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox37Adult.Text.Trim()))
                    openFileDialog1.FileName = textBox37Adult.Text.Trim();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    textBox37Adult.Text = openFileDialog1.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button37Child_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox37Child.Text.Trim()))
                    openFileDialog1.FileName = textBox37Child.Text.Trim();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    textBox37Child.Text = openFileDialog1.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button60Adult_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox60Adult.Text.Trim()))
                    openFileDialog1.FileName = textBox60Adult.Text.Trim();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    textBox60Adult.Text = openFileDialog1.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button60Child_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox60Child.Text.Trim()))
                    openFileDialog1.FileName = textBox60Child.Text.Trim();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    textBox60Child.Text = openFileDialog1.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button61Adult_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox60Adult.Text.Trim()))
                    openFileDialog1.FileName = textBox61Adult.Text.Trim();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    textBox61Adult.Text = openFileDialog1.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button61Child_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox60Child.Text.Trim()))
                    openFileDialog1.FileName = textBox61Child.Text.Trim();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    textBox61Child.Text = openFileDialog1.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBoxTemplate.Text.Trim()))
                    openFileDialog1.FileName = textBoxTemplate.Text.Trim();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    textBoxTemplate.Text = openFileDialog1.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonOutput_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBoxOutput.Text.Trim()))
                    saveFileDialog1.FileName = textBoxOutput.Text.Trim();
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    textBoxOutput.Text = saveFileDialog1.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonInput_Click(object sender, EventArgs e)
        {
            TextBox txt = textBoxInput;
            try
            {
                this.lstboxHallFiles.Items.Clear();
                if (!string.IsNullOrEmpty(txt.Text.Trim()))
                    openFolderDialog1.SelectedPath = txt.Text.Trim();
                if (openFolderDialog1.ShowDialog() == DialogResult.OK)
                    txt.Text = openFolderDialog1.SelectedPath;

                IReportTransformer transformer = new BaseTransformer();
                transformer.SetExcelFiles(this, transformConfiguration, txt.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}