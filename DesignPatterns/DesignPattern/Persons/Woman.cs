using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons
{
    class Woman:Person
    {
        public override void Accept(Action visitor)
        {
            try
            {
                visitor.GetWomanConclusion(this);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
