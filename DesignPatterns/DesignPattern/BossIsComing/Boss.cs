using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossIsComing
{
    class Boss : ISubject
    {
        public event EventHandler SecretaryUpdate;
        private string _action;
        public void Notify()
        {
            SecretaryUpdate();
        }

        public string SubjectState
        {
            get { return this._action; }
            set { this._action = value; }
        }
    }
}
