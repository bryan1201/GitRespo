using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOvertime
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int j = 1; j <= 30; j++)
            {
                Console.WriteLine("# Day {0}", j);
                Work ep = new Work(); // Emergency Projects
                ep.Day = j;
                double div = double.Parse(j.ToString()) / 4f;
                double div2 = Math.Ceiling(double.Parse(j.ToString()) / 5f);
                for (int i = 8; i < 24; i = i + int.Parse(div2.ToString()))
                {
                    if (i >= 18 && (div - Math.Round(div))==0)
                        ep.TaskFinished = true; 

                    ep.Hour = i;
                    ep.WriteProgram();
                }
            }
            //ep.SetState(new RestState());
            //ep.WriteProgram();
            Console.Read();
        }
    }
}
