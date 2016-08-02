using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BuildSmalMan
{
    public partial class FormBuild : Form
    {
        public FormBuild()
        {
            InitializeComponent();
            label1.Text = "1";
        }

        public void BuildSmallMan(string bn)
        {
            SolidBrush sb = new SolidBrush(color: Color.Yellow);

            Pen p = new Pen(sb,5.5f);
            Graphics gThin = pictureBox1.CreateGraphics();
            gThin.Clear(Color.Blue);
            gThin.DrawEllipse(p, 50, 20, 30, 30);
            if(bn=="1")
            gThin.DrawRectangle(p, 60, 50, 10, 50);
            else
                gThin.DrawRectangle(p, 50, 50, 30, 50);
            gThin.DrawLine(p, 60, 50, 40, 100);
            gThin.DrawLine(p, 70, 50, 90, 100);
            gThin.DrawLine(p, 60, 100, 45, 150);
            gThin.DrawLine(p, 70, 100, 85, 150);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label1.Text == "1")
                label1.Text = "2";
            else
                label1.Text = "1";
            //BuildSmallMan(bn: label1.Text);
            BuildPerson(bn: label1.Text);
        }

        // Builder 建造者模式
        public void BuildPerson(string bn)
        {
            Pen p = new Pen(Color.Yellow);

            if (bn == "1")
            {
                PersonThinBuilder ptb = new PersonThinBuilder(pictureBox1.CreateGraphics(), p);
                PersonDirector pdThin = new PersonDirector(ptb);
                pdThin.CreatePerson();
            }
            else
            {
                PersonFatBuilder pfb = new PersonFatBuilder(pictureBox1.CreateGraphics(), p);
                PersonDirector pdFat = new PersonDirector(pfb);
                pdFat.CreatePerson();
            }
        } 
    }
}
