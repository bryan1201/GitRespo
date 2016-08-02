using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOvertime
{
    public class Work
    {
        private State current { get; set; }
        public Work()
        {
            this.current = new ForenoonState();
        }

        public double Hour { get; set; }
        public double Day { get; set; }
        private bool finish = false;
        public bool TaskFinished
        {
            get
            {
                return finish;
            }
            set
            {
                finish = value;
            }
        }

        public void SetState(State s)
        {
            current = s;
        }

        public void WriteProgram()
        {
            current.WriteProgram(this);
        }
    }
}
