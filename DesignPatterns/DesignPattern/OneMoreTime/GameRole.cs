using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMoreTime
{
    public class RoleStateCaretaker
    {
        public RoleStateMemento Memento { get; set; }
    }

    public class RoleStateMemento
    {
        //生命力
        public int Vitality { get; set; }
        //攻擊力
        public int Attack { get; set; }
        //防御力
        public int Defense { get; set; }

        public RoleStateMemento(int vit, int atk, int def)
        {
            this.Vitality = vit;
            this.Attack = atk;
            this.Defense = def;
        }
    }

    public class GameRole
    {
        //生命力
        public int Vitality { get; set; }
        //攻擊力
        public int Attack { get; set; }
        //防御力
        public int Defense { get; set; }
        //顯示狀態
        public void StateDisplay()
        {
            Console.WriteLine("Game Role Current State: ");
            Console.WriteLine("Vitality: {0}", this.Vitality);
            Console.WriteLine("Attack: {0}", this.Attack);
            Console.WriteLine("Defense: {0}\n", this.Defense);
        }

        // 取得初始狀態
        public void GetInitState()
        {
            // 通常從磁碟機檔案中取得
            this.Vitality = 60;
            this.Attack = 22;
            this.Defense = 18;
        }

        // 戰鬥
        public void Fight()
        {
            // 與Boss大戰後損耗為零
            this.Vitality = 0;
            this.Attack = 0;
            this.Defense = 0;
        }

        // 保存角色狀態
        public RoleStateMemento SaveState()
        {
            return (new RoleStateMemento(Vitality, Attack, Defense));
        }

        // 恢復角色狀態
        public void RecoveryState(RoleStateMemento memento)
        {
            this.Vitality = memento.Vitality;
            this.Attack = memento.Attack;
            this.Defense = memento.Defense;
        }
    }

    class Memento
    {
        public string State { get; set; }
        public Memento(string state)
        {
            this.State = state;
        }
    }

    class Caretaker
    {
        public Memento Memento { get; set; }
    }

    class Originator
    {
        public string State { get; set; }
        public Memento CreateMemento()
        {
            return (new Memento(State));
        }

        public void SetMemento(Memento memento)
        {
            State = memento.State;
        }
        public void Show()
        {
            Console.WriteLine("State = {0}", State);
        }
    }
}
