using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace BuildSmalMan
{
    abstract class PersonBuilder
    {
        protected Graphics _graphic;
        protected Pen _pen;

        public PersonBuilder(Graphics graphic, Pen pen)
        {
            this._graphic = graphic;
            this._pen = pen;
        }
        public abstract void BuildHead();
        public abstract void BuildBody();
        public abstract void BuildArmLeft();
        public abstract void BuildArmRight();
        public abstract void BuildLegLeft();
        public abstract void BuildLegRight();
    }
}
