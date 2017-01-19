using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons
{
    class Success : IAction
    {
        public void GetManConclusion(Man concreteElementA)
        {
            try
            {
                string rslt = string.Format("{0} {1}時，背後多半有一個偉大的女人。", concreteElementA.GetType().Name, this.GetType().Name);
                Console.WriteLine(rslt);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public void GetWomanConclusion(Woman concreteElementB)
        {
            try
            {
                string rslt = string.Format("{0} {1}時，背後大多有一個不成功的男人。", concreteElementB.GetType().Name, this.GetType().Name);
                Console.WriteLine(rslt);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
