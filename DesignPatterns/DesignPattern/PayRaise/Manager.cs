using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRaise
{
    public class Request
    {
        public string RequestType { get; set; }
        public string RequestContent { get; set; }
        public int Number { get; set; }

        public Request()
        {

        }

        public Request(string reqType, string reqContent, int number)
        {
            this.RequestType = reqType;
            this.RequestContent = reqContent;
            this.Number = number;

            Console.WriteLine("\nStart {0} {1} {2} ", this.RequestType, this.RequestContent, this.Number);
        }
    }

    abstract public class Manager
    {
        protected string name { get; set; }
        protected Manager superior { get; set; }
        public Manager(string name)
        {
            this.name = name;
        }

        public void SetSuperior(Manager superior)
        {
            this.superior = superior;
        }

        abstract public void RequestApplications(Request request);
    }

    public class CommonManager:Manager
    {
        public CommonManager(string name):base(name)
        {

        }

        public override void RequestApplications(Request request)
        {
            if(request.RequestType=="請假" && request.Number <=2)
            {
                Console.WriteLine("{0}:{1} 數量{2} 被批准！", name, request.RequestContent, request.Number);
            }
            else if(superior != null)
            {
                Console.WriteLine("{0}:{1} 數量{2} 被批准！", name, request.RequestContent, request.Number);
                superior.RequestApplications(request);
            }
        }
    }

    public class Majordomo : Manager
    {
        public Majordomo(string name) : base(name)
        {

        }

        public override void RequestApplications(Request request)
        {
            if (request.RequestType == "請假" && request.Number <= 5)
            {
                Console.WriteLine("{0}:{1} 數量{2} 被批准！", name, request.RequestContent, request.Number);
            }
            else if (superior != null)
            {
                Console.WriteLine("{0}:{1} 數量{2} 被批准！", name, request.RequestContent, request.Number);
                superior.RequestApplications(request);
            }
        }
    }

    public class GeneralManager : Manager
    {
        public GeneralManager(string name) : base(name)
        {

        }

        public override void RequestApplications(Request request)
        {
            if (request.RequestType == "請假")
            {
                Console.WriteLine("{0}:{1} {2} 數量{3} 被批准！", name, request.RequestType, request.RequestContent, request.Number);
            }
            else if(request.RequestType == "加薪" && request.Number<=500)
            {
                Console.WriteLine("{0}:{1} {2} 數量{3} 被批准！", name, request.RequestType, request.RequestContent, request.Number);
            }
            else if (request.RequestType == "加薪" && request.Number > 500)
            {
                Console.WriteLine("{0}:{1} {2} 數量{3} 再說吧！", name, request.RequestType, request.RequestContent, request.Number);
            }
            else if (request.RequestType == "加薪&積優報表" && request.Number > 500)
            {
                Console.WriteLine("{0}:{1} {2} 數量{3} 被批准！", name, request.RequestType, request.RequestContent, request.Number);
            }
        }
    }
}
