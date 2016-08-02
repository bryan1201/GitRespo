using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBATranslator
{
    public abstract class Player
    {
        protected string name { get; set; }
        public Player(string name)
        {
            this.name = name;
        }
        public abstract void Attack();
        public abstract void Defense();
    }

    class Forwards: Player
    {
        public Forwards(string name):base(name)
        {

        }

        public override void Attack()
        {
            Console.WriteLine("Forwards {0} Attack", name);
        }

        public override void Defense()
        {
            Console.WriteLine("Forwards {0} Defense", name);
        }
    }

    class Center : Player
    {
        public Center(string name) : base(name)
        {

        }

        public override void Attack()
        {
            Console.WriteLine("Center {0} Attack", name);
        }

        public override void Defense()
        {
            Console.WriteLine("Center {0} Defense", name);
        }
    }

    class Guards : Player
    {
        public Guards(string name) : base(name)
        {

        }

        public override void Attack()
        {
            Console.WriteLine("Guards {0} Attack", name);
        }

        public override void Defense()
        {
            Console.WriteLine("Guards {0} Defense", name);
        }
    }

    class ForeignCenter
    {
        private string name;
        public string Name { get; set; }
        public void ChineseAttack()
        {
            Console.WriteLine("外籍中鋒 {0} 進攻！",name);
        }

        public void ChineseDefense()
        {
            Console.WriteLine("外籍中鋒 {0} 防守！", name);
        }
    }

    class Translator: Player
    {
        private ForeignCenter wjzf = new ForeignCenter();
        public Translator(string name) : base(name)
        {
            wjzf.Name = name;
        }

        public override void Attack()
        {
            wjzf.ChineseAttack();
        }

        public override void Defense()
        {
            wjzf.ChineseDefense();
        }
    }
}
