using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildSmalMan
{
    class PersonDirector
    {
        private PersonBuilder _pb;
        public PersonDirector(PersonBuilder pb)
        {
            this._pb = pb;
        }

        public void CreatePerson()
        {
            _pb.BuildHead();
            _pb.BuildBody();
            _pb.BuildArmLeft();
            _pb.BuildArmRight();
            _pb.BuildLegLeft();
            _pb.BuildLegRight();
        }
    }
}
