using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace BuildSmalMan
{
    class PersonThinBuilder : PersonBuilder
    {
        public PersonThinBuilder(Graphics graphic, Pen pen) : base(graphic, pen)
        {
            pen.Width = 1;
            pen.Color = Color.DarkBlue;
            graphic.Clear(Color.White);
        }

        public override void BuildHead()
        {
            this._graphic.DrawEllipse(this._pen, 50, 20, 30, 30);
        }

        public override void BuildBody()
        {
            this._graphic.DrawRectangle(_pen, 60, 50, 10, 50);
        }

        public override void BuildArmLeft()
        {
            this._graphic.DrawLine(_pen, 60, 50, 40, 100);
        }

        public override void BuildArmRight()
        {
            this._graphic.DrawLine(_pen, 70, 50, 90, 100);
        }
        public override void BuildLegLeft()
        {
            this._graphic.DrawLine(_pen, 60, 100, 45, 150);
        }
        public override void BuildLegRight()
        {
            this._graphic.DrawLine(_pen, 70, 100, 85, 150);
        }
    }
}
