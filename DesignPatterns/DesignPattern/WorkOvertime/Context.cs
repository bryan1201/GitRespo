using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOvertime
{
    public class Context
    {
        private State state { get; set; }
        public Context(State state)
        {
            this.state = state;
        }
        public State State
        {
            get { return state; }
            set
            {
                string s = string.Format("Current State: {0}", state.GetType().Name);
                state = value;
                Console.WriteLine();
            }
        }
    }
}
