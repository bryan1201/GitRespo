using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossIsComing
{
    class Secretary : ISubject
    {
        public event EventHandler Update;
        private string _action;
        public void Notify()
        {
            Update();
        }

        public string SubjectState
        {
            get { return this._action; }
            set { this._action = value; }
        }
    }
}
