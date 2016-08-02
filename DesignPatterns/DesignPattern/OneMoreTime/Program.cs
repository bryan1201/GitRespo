using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMoreTime
{
    class Program
    {
        static void Main(string[] args)
        {
            GameFight();
        }

        static public void GameFight()
        {
            // 大戰Boss前
            GameRole fighter = new GameRole();
            fighter.GetInitState();
            fighter.StateDisplay();

            // 保存進度
            RoleStateCaretaker stateAdmin = new RoleStateCaretaker();
            stateAdmin.Memento = fighter.SaveState();

            // 大戰Boss時，損耗嚴重
            fighter.Fight();
            fighter.StateDisplay();

            // 恢復先前的狀態
            fighter.RecoveryState(stateAdmin.Memento);
            fighter.StateDisplay();

            Console.Read();
        }

        static public void Idea()
        {
            Originator org = new Originator();
            org.State = "On";
            org.Show();

            Caretaker ct = new Caretaker();
            ct.Memento = org.CreateMemento();

            org.State = "Off";
            org.Show();

            org.SetMemento(ct.Memento);
            org.Show();

            Console.Read();
        }
    }
}
