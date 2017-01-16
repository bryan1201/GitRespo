using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons
{
    class Amativeness : Action
    {
        public override void GetManConclusion(Man concreteElementA)
        {
            try
            {
                string rslt = string.Format("{0} {1}時，凡事不懂也要裝懂。", concreteElementA.GetType().Name, this.GetType().Name);
                Console.WriteLine(rslt);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public override void GetWomanConclusion(Woman concreteElementB)
        {
            /*
              string succ = string.Format("{0}{1}時，背後大多有一個不成功的男人。", this.GetType().Name, Action);
                string fail = string.Format("{0}{1}時，眼淚汪汪，誰也勤不動。", this.GetType().Name, Action);
                string love = string.Format("{0}{1}時，遇事懂也裝作不懂。", this.GetType().Name, Action);
             */

            try
            {
                string rslt = string.Format("{0} {1}時，遇事懂也裝作不懂。", concreteElementB.GetType().Name, this.GetType().Name);
                Console.WriteLine(rslt);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
