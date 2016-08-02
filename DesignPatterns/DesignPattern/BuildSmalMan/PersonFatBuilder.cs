using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace BuildSmalMan
{
    class PersonFatBuilder : PersonBuilder
    {
        public PersonFatBuilder(Graphics graphic, Pen pen):base(graphic, pen)
        {
            pen.Width = 5;
            pen.Color = Color.Yellow;
            graphic.Clear(Color.Black);
        }

        public override void BuildHead()
        {
            this._graphic.DrawEllipse(this._pen, 50, 20, 30, 30);
        }

        public override void BuildBody()
        {
            this._graphic.DrawEllipse(_pen, 45, 50, 40, 50);
        }

        public override void BuildArmLeft()
        {
            this._graphic.DrawLine(_pen, 50, 50, 30, 100);
        }

        public override void BuildArmRight()
        {
            this._graphic.DrawLine(_pen, 80, 50, 100, 100);
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
