using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Data;

namespace WWOMConverter
{
    class Method
    {
        enum RecycleFlags : uint
        {
            // No confirmation dialog when emptying the recycle bin
            SHERB_NOCONFIRMATION = 0x00000001,
            // No progress tracking window during the emptying of the recycle bin
            SHERB_NOPROGRESSUI = 0x00000001,
            // No sound whent the emptying of the recycle bin is complete
            SHERB_NOSOUND = 0x00000004
        }

        // Shell32.dll is where SHEmptyRecycleBin is located
        // <summary>
        // Import Win32 DLL for emptying the recycle bin
        // </summary>
        // <param name="hwnd"></param>
        // <param name="pszRootPath"></param>
        // <param name="dwFlags"></param>
        // <returns></returns>
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        // The signature of SHEmptyRecycleBin (located in Shell32.dll)
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

        public static bool EmptyRecycleBin()
        {
            try
            {
                uint result = SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHERB_NOSOUND | RecycleFlags.SHERB_NOCONFIRMATION);

                if (result > 0)
                    return true;
                else
                {
                    throw new Exception("\nAlready Emptyed!\n");
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("\nRecycle Bin: {0}", ex.Message);
                Console.WriteLine(message);
                return false;
            }
        }
        public static string ReadFile(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            return text;
        }

        public static void WriteLog(string filePath, string textToAdd)
        {
            string logFile = string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log"; // DateTime.Now.ToShortDateString().Replace(@"/", @"-").Replace(@"\", @"-") + ".log";
            filePath = Path.Combine(filePath, logFile);
            StreamWriter swFromFile = new StreamWriter(filePath, true, Encoding.UTF8);
            try
            {
                string message = string.Format("{0:yyyy/MM/dd hh:mm:ss.fff tt}", DateTime.Now.ToLocalTime()) + "\t" + textToAdd;
                swFromFile.WriteLine(message);
                Console.WriteLine(message);
            }
            catch
            {
                //
            }
            swFromFile.Flush();
            swFromFile.Close();
        }

        public static byte[] ReadFile(string filePath, bool bit)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        public static bool IsFileInUse(string path)
        {
            string __message = "";
            try
            {
                //Just opening the file as open/create
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    //If required we can check for read/write by using fs.CanRead or fs.CanWrite
                }
                return false;
            }
            catch (IOException ex)
            {
                //check if message is for a File IO
                __message = ex.Message.ToString();
                if (__message.Contains("The process cannot access the file") ||
                    __message.Contains("because it is being used by another process"))
                    return true;
                else
                    throw;
            }
        }

        public static void TryCreateFolder(string targetPath)
        {
            if (!Directory.Exists(targetPath))
            {
                try
                {
                    Directory.CreateDirectory(targetPath);
                }
                catch
                {
                    //
                }
            }
        }

        public static void MoveFiles(string sourcePath, string targetPath)
        {
            TryCreateFolder(targetPath);

            if (!Directory.Exists(sourcePath) || !Directory.Exists(targetPath))
                return;

            DirectoryInfo di = new DirectoryInfo(sourcePath);

            foreach (FileInfo item in di.GetFiles())
            {
                string targetFullName = Path.Combine(targetPath, item.Name);

                if (File.Exists(targetFullName))
                {
                    File.Delete(targetFullName);
                }

                if (item.Exists)
                {
                    System.IO.File.Move(item.FullName, targetFullName);
                    //item.MoveTo(targetFullName);
                }
            }

            foreach (DirectoryInfo item in di.GetDirectories())
            {
                string _subPath = Path.Combine(targetPath, item.Name);
                if (Directory.Exists(_subPath))
                    Directory.Delete(_subPath);
                if (item.Exists)
                {
                    item.MoveTo(targetPath);
                    //item.Delete();
                }
            }
            Method.EmptyRecycleBin();
        }

        public static void DeleteFiles(string PathStr)
        {
            if (!Directory.Exists(PathStr))
                return;

            DirectoryInfo di = new DirectoryInfo(PathStr);

            foreach (FileInfo item in di.GetFiles())
            {
                if (item.Exists)
                    item.Delete();
            }


            foreach (DirectoryInfo item in di.GetDirectories())
            {
                if (item.Exists)
                {
                    item.Delete(true);
                    //item.Delete();
                }
            }

            Method.EmptyRecycleBin();
        }

        public static void DeleteFiles(string PathStr, int size)
        {
            int IsMapping = -1;
            bool IsFileInUse = true;
            if (!Directory.Exists(PathStr))
                return;

            DirectoryInfo di = new DirectoryInfo(PathStr);
            IEnumerable<System.IO.FileInfo> fileList = di.GetFiles("*.log");

            //Create the query
            IEnumerable<System.IO.FileInfo> fileQuery =
                from file in fileList
                orderby file.LastWriteTime
                select file;

            var oldestFile =
                (from file in fileQuery
                 orderby file.LastWriteTime descending
                 select new { file.FullName, file.LastWriteTime })
                .Last();

            DateTime oldestfiletime = oldestFile.LastWriteTime;

            foreach (FileInfo item in di.GetFiles())
            {
                int IsToday = IsSameDate(DateTime.Today, item.LastWriteTime);
                IsMapping = IsSameDate(oldestfiletime, item.LastWriteTime);
                IsFileInUse = Method.IsFileInUse(item.FullName);
                // Use static Path methods to extract only the file name from the path.
                if (item.Exists && IsMapping == 0 && IsFileInUse == false && IsToday != 0)
                    item.Delete();
            }

            foreach (DirectoryInfo item in di.GetDirectories())
            {
                int IsToday = IsSameDate(DateTime.Today, item.LastWriteTime);
                IsMapping = IsSameDate(oldestfiletime, item.LastWriteTime);
                IsFileInUse = Method.IsFileInUse(item.FullName);
                if (item.Exists && IsMapping == 0 && IsFileInUse == false && IsToday != 0)
                {
                    item.Delete(true);
                    //item.Delete();
                }
            }

            Method.EmptyRecycleBin();
        }

        public static int CopyFiles(string sourcePath, string targetPath, string MappingExt, DateTime copydate)
        {
            // To copy all the files in one directory to another directory.
            // Get the files in the source folder. (To recursively iterate through
            // all subfolders under the current directory, see
            // "How to: Iterate Through a Directory Tree.")
            // Note: Check for target path was performed previously
            //       in this code example.
            string fileName = "*.log";
            string destFile = "";
            int retRslt = 0;

            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath, MappingExt);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    DateTime lastWriteTime = File.GetCreationTime(s);
                    int IsMapping = IsSameDate(copydate, lastWriteTime);
                    bool IsFileInUse = Method.IsFileInUse(s);
                    // Use static Path methods to extract only the file name from the path.
                    if (IsMapping == 0 && IsFileInUse == false)
                    {
                        fileName = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine(targetPath, fileName);
                        System.IO.File.Copy(s, destFile, true);
                        System.IO.File.SetLastAccessTime(destFile, lastWriteTime);
                        retRslt++;
                    }
                }
            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }

            return retRslt;
        }

        public static int IsSameDate(DateTime date1, DateTime date2)
        {
            int retRslt = 0;
            date2 = DateTime.Parse(date2.ToShortDateString());
            retRslt = DateTime.Compare(date1, date2);
            return retRslt;
        }

        public static bool IsMappedExt(string stdExt, string filename)
        {
            bool retRslt = false;
            filename = filename.ToUpper();
            StringBuilder AddExt = new StringBuilder();
            AddExt.Append(".");
            AddExt.Append(stdExt.ToUpper());

            string MappedExt = filename.Substring(filename.Length - 4, 4);
            if (MappedExt == AddExt.ToString())
                retRslt = true;

            return retRslt;
        }

        public static void Pause()
        {
            Console.Write("\nPress any key to continue . . . \n");
            Console.ReadKey(true);
        }

        public static string EncodingConvert(string sourceString)
        {
            string EncodingName = Encoding.Default.EncodingName;
            StringBuilder strRslt = new StringBuilder();
            ASCIIEncoding AE = new ASCIIEncoding();
            System.Text.UnicodeEncoding UE = new UnicodeEncoding();

            byte[] ByteArray = UE.GetBytes(sourceString);

            char[] CharArray = UE.GetChars(ByteArray);
            for (int x = 0; x <= CharArray.Length - 1; x++)
            {
                strRslt.Append(CharArray[x]);
            }

            return strRslt.ToString();
        }

        public static long GetTotalFreeSpace(string driveName)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name.Replace(":\\", "") == driveName)
                {
                    return drive.TotalFreeSpace;
                }
            }
            return -1;
        }
        // End of Method function
    }
}
