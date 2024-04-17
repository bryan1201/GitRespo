using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nightingale
{
    class Program
    {
        // Factory Pattern
        static void Main(string[] args)
        {
            Console.WriteLine("簡單工廠模式 Simple Factory Pattern");
            Nightingale stA = SimpleFactory.Create("Student");
            stA.BuyRice();
            Nightingale stB = SimpleFactory.Create("Student");
            stB.Sweep();
            Nightingale stC = SimpleFactory.Create("Student");
            stC.Wash();
            Console.WriteLine("按任意鍵...執行工廠模式");
            Console.Read();

            Console.WriteLine("工廠模式 Factory Pattern");
            IFactory factory = new StudentGraduatedFactory();
            Nightingale student = factory.CreateNightingale();

            student.MonitorNurse();
            student.EnsureThePurityOfMedicine();
            student.OperateHospitalEquipment();
            Console.Read();
            Console.Read();

            IFactory factory2 = new VolunteerFactory();
            Nightingale volunteer = factory2.CreateNightingale();
            volunteer.BuyRice();
            volunteer.Sweep();
            volunteer.Wash();

            Console.Read();
            Console.Read();
        }
    }
}
