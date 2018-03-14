using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace WWOMConverter
{
    static class Program
    {
        private static BackgroundWorker worker = new BackgroundWorker();
        private static event EventHandler BackgroundWorkFinished;
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormWWOM());
            */
            /*
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            Console.WriteLine("Starting Application...");

            worker.RunWorkerAsync();
            Console.ReadKey();
            */
            try
            {
                IDataTransfer idt = new ProcessData();
                idt.ImportAction();
                Method.WriteLog(Constant.S_ProgramLog, "idt.ImportAction() completed");
            }
            catch (Exception ex)
            {
                Method.WriteLog(Constant.S_ProgramLog, ex.Message);
            }
        }

        static void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine(e.ProgressPercentage.ToString());
        }

        static void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Starting to do some work now...");



            e.Result = 100;
        }

        static void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Value Of i = " + e.Result.ToString());
            Console.WriteLine("Done now...");
        }
    }
}
