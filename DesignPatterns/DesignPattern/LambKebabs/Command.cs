﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambKebabs
{
    public class Barbecuer
    {
        public string BakeMutton()
        {
            //Console.WriteLine("BakeMutton, 烤羊肉串！");
            return "BakeMutton, 烤羊肉串！";
        }

        public string BakeChickedWing()
        {
            //Console.WriteLine("BakeChickedWing, 烤雞翅！");
            return "BakeChickedWing, 烤雞翅！";
        }
    }

    public abstract class Command
    {
        protected Barbecuer receiver { get; set; }

        public Command(Barbecuer rec)
        {
            this.receiver = rec;
        }

        abstract public void ExecuteCommand();
    }

    class BakeMuttonCommand : Command
    {
        public BakeMuttonCommand(Barbecuer rec) : base(rec)
        {
            
        }

        public override void ExecuteCommand()
        {
            Console.WriteLine(string.Format("{0}, {1}",receiver.ToString(), receiver.BakeMutton()));            
        }
    }

    class BakeChickenWingCommand:Command
    {
        public BakeChickenWingCommand(Barbecuer rec):base(rec)
        {

        }

        public override void ExecuteCommand()
        {
            //receiver.BakeChickedWing();
            Console.WriteLine(string.Format("{0},{1}", receiver.ToString(), receiver.BakeChickedWing()));
        }
    }

    public class Waiter
    {
        private IList<Command> orders = new List<Command>();
        public void SetOrder(Command cmd)
        {
            if (cmd.ToString() == "LambKebabs.BakeChickenWingCommand")
            {
                Console.WriteLine("服務員：雞翅沒了，請點別的燒烤！");
            }
            else
            {
                orders.Add(cmd);
                Console.WriteLine("日誌：增加訂單 {0}，時間 {1}", cmd.ToString(), DateTime.Now.ToString());
            }
        }

        public void CancelOrder(Command cmd)
        {
            orders.Remove(cmd);
            Console.WriteLine("日誌：取消訂單 {0}，時間 {1}", cmd.ToString(), DateTime.Now.ToString());
        }

        public void Notify()
        {
            foreach (Command cmd in orders)
                cmd.ExecuteCommand();
        }
    }
}
