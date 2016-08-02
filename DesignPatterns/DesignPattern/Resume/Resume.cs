using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume
{
    class Resume : ICloneable
    {
        private string _name;
        private string _sex;
        private string _age;
        //private string _timeArea;
        //private string _company;

        private WorkExperience work;

        public Resume(string name)
        {
            this._name = name;
            work = new WorkExperience();
        }

        private Resume(WorkExperience work)
        {
            this.work = (WorkExperience)work.Clone();
        }

        // 設定個人資訊
        public void SetPersonalInfo(string sex, string age)
        {
            this._sex = sex;
            this._age = age;
        }

        public void SetWorkExperience(string timeArea, string company)
        {
            work.WorkDate = timeArea;
            work.Company = company;
            //this._timeArea = timeArea;
            //this._company = company;
        }

        // Display
        public void Display()
        {
            Console.WriteLine("{0} {1} {2}", _name, _sex, _age);
            //Console.WriteLine("工作經歷：{0} {1}", _timeArea, _company);
            Console.WriteLine("工作經歷：{0} {1}", work.WorkDate, work.Company);
        }

        public Object Clone()
        {
            Resume obj = new Resume(this.work);
            obj._name = this._name;
            obj._sex = this._sex;
            obj._age = this._age;
            return obj;
            //return (Object)this.MemberwiseClone();
        }
    }
}
