using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox
{
    public sealed class Singleton
    {
        private static readonly Singleton instance = new Singleton();
        private static readonly object syncRoot = new object();
        private Singleton()
        {

        }
        public static Singleton GetInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        //instance = new Singleton();
                        return instance;
                    }
                }
            }
            return instance;
        }
    }
}
