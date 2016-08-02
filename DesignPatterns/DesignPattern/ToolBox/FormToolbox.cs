using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolBox
{
    public partial class FormToolbox : Form
    {
        private Singleton s1 = null;
        private Singleton s2 = null;
        private static FormToolbox ftb = null;
        private FormToolbox()
        {
            InitializeComponent();
        }

        public static FormToolbox GetInstance()
        {
            if(ftb==null||ftb.IsDisposed)
            {
                ftb = new FormToolbox();
                ftb.StartPosition = FormStartPosition.CenterScreen;
                ftb.MdiParent = Form1.ActiveForm;
            }
            return ftb;
        }

        private void btnS1_Click(object sender, EventArgs e)
        {
            s1 = Singleton.GetInstance();
            bthIsEqual.Text = "s1 = Singleton.GetInstance()";
        }

        private void btnS2_Click(object sender, EventArgs e)
        {
            s2 = Singleton.GetInstance();
            bthIsEqual.Text = "s2 = Singleton.GetInstance()";
        }

        private void bthIsEqual_Click(object sender, EventArgs e)
        {
            if (s1 == null || s2 == null)
                return;
            if (s1 == s2)
            {
                bthIsEqual.Text = "s1, s1 are the same instance!";
            }
        }
    }
}
