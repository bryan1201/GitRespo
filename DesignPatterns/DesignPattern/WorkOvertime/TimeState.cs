using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOvertime
{
    public class ForenoonState: State
    {
        public override void WriteProgram(Work w)
        {
            string s = string.Format("Day: {0}, Current Time: {1}, Work at forenoon, I feel that my spirit is very good!", w.Day, w.Hour);
            if (w.Hour < 12)
            {
                Console.WriteLine(s);
            }
            else
            {
                w.SetState(new NoonState());
                w.WriteProgram();
            }
        }
    }

    public class NoonState: State
    {
        public override void WriteProgram(Work w)
        {
            string s = string.Format("Day: {0}, Current Time: {1}, Work at Noon, I am hungry now!", w.Day, w.Hour);
            if (w.Hour < 13)
            {
                Console.WriteLine(s);
            }
            else
            {
                w.SetState(new AfternoonState()); w.WriteProgram();
            }
        }
    }

    public class AfternoonState : State
    {
        public override void WriteProgram(Work w)
        {
            string s = string.Format("Day: {0}, Current Time: {1}, Work at afternoon, I am good to work hard still!",w.Day, w.Hour);
            if (w.Hour < 17)
            {
                Console.WriteLine(s);
            }
            else
            {
                w.SetState(new EveningState()); w.WriteProgram();
            }
        }
    }

    public class EveningState : State
    {
        public override void WriteProgram(Work w)
        {
            string s = string.Format("Day: {0}, Current Time: {1}, Work at evening, I feel tired!",w.Day, w.Hour);
            if (w.TaskFinished)
            {
                w.SetState(new RestState());
                w.WriteProgram();
            }
            else
            {
                if (w.Hour < 21)
                {
                    Console.WriteLine(s);
                }
                else
                {
                    w.SetState(new SleepingState()); w.WriteProgram();
                }
            }
        }
    }

    public class SleepingState : State
    {
        public override void WriteProgram(Work w)
        {
            string s = string.Format("Day: {0}, Current Time: {1}, Work at night, I cannot stand anymore! I want to go to bed!",w.Day, w.Hour);
            Console.WriteLine(s);
        }
    }

    public class RestState : State
    {
        public override void WriteProgram(Work w)
        {
            string s = string.Format("Day: {0}, Current Time: {1}, I have got off work!",w.Day, w.Hour);
            Console.WriteLine(s);
        }
    }
}
