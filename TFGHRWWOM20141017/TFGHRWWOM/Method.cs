using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Data;
using ConsoleApplication1;

namespace General
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

        public static string BuildXML(string obj_value, string obj_name)
        {
            StringBuilder myValue = new StringBuilder();
            myValue.Append("<");
            myValue.Append(obj_name);
            myValue.Append(">");
            myValue.Append(obj_value);
            myValue.Append("</");
            myValue.Append(obj_name);
            myValue.Append(">");
            return myValue.ToString();
        }

        public static string GrabXml(string str, string skey)
        {
            if (str == null)
            {
                return "";
            }
            int startlen = str.IndexOf("<" + skey + ">") + ("<" + skey + ">").Length;
            int len = str.IndexOf("</" + skey + ">") - startlen;

            string s_value = "";
            if (len > 0) { s_value = str.Substring(startlen, len); }

            return s_value;

        }

        public static string ChgXml(string str, string skey, string s_newvalue)
        {
            StringBuilder getStr = new StringBuilder();
            StringBuilder oldValue = new StringBuilder();
            StringBuilder newValue = new StringBuilder();
            getStr.Append(str);
            int exists = getStr.ToString().IndexOf(skey);
            if (exists < 0)
            {
                //str = str + BuildXML(s_value, skey);
                str = getStr.Append(BuildXML(s_newvalue, skey)).ToString();
            }
            else
            {
                //str = str.Replace("<" + skey + ">" + GrabXml(str, skey) + "</" + skey + ">", "<" + skey + ">" + s_value + "</" + skey + ">");
                oldValue.Append(BuildXML(GrabXml(str, skey), skey));
                newValue.Append(BuildXML(s_newvalue, skey));

                str = getStr.Replace(oldValue.ToString(), newValue.ToString()).ToString();
            }
            return str;
        }

        public static string[] GetDestSites(string DestSites)
        {
            string[] ArrayDestSites;
            if (DestSites == null || DestSites.Length ==0)
            {
                return null;
            }
            else
            {
                ArrayDestSites = DestSites.Split(';');
            }
            return ArrayDestSites;
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
                swFromFile.WriteLine(string.Format("{0:yyyy/MM/dd hh:mm:ss.fff tt}",DateTime.Now.ToLocalTime()) + "\t" + textToAdd);
            }
            catch {
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
                if (item.Exists && IsMapping == 0 && IsFileInUse == false && IsToday!=0)
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

        public static string FormatZipFileName(string SrcSite, string WorkingSystem, string ZippedFileExt)
        {
            //Example: IPC_NPSLog.zip
            StringBuilder retFileName = new StringBuilder();
            retFileName.Append(SrcSite);
            retFileName.Append(WorkingSystem);
            retFileName.Append(".");
            retFileName.Append(ZippedFileExt);
            return retFileName.ToString();
        }

        public static string FormatZipFileName(string SrcSite, string LocalHostName, string WorkingSystem, string ZippedFileExt)
        {
            //Example: IPC_NPSLog.zip
            StringBuilder retFileName = new StringBuilder();
            retFileName.Append(SrcSite);
            retFileName.Append("_");
            retFileName.Append(LocalHostName);
            retFileName.Append(WorkingSystem);
            retFileName.Append(".");
            retFileName.Append(ZippedFileExt);
            return retFileName.ToString();
        }


        public static string CorrectFilePath(string fp)
        {
            fp = fp.Trim();
            if (fp.Length == 0)
                return "";
            string endStr = @"\";
            string orgStr = fp.Substring(fp.Length - 1);
            if (orgStr != endStr)
                fp = fp + endStr;
            return fp;
        }


        public static string GetZipFileName(string PathStr, string MappingExt, string ZippedFileExt)
        {
            StringBuilder strZipFileName = new StringBuilder();
            string sfname = "";
            string[] fileEntries;

            if (Directory.Exists(PathStr))
            {
                fileEntries = Directory.GetFiles(PathStr, MappingExt);
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(PathStr);
                }
                catch
                {
                    //
                }
                return strZipFileName.ToString();
            }

            if (fileEntries.Length > 0)
            {
                string fileName = fileEntries[0].ToString();
                FileInfo fi = new FileInfo(fileName);
                sfname = fi.Name;
                strZipFileName.Append(sfname.Substring(0, sfname.Length - 2));
                strZipFileName.Append(ZippedFileExt);
            }
            return strZipFileName.ToString();
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
                if (drive.IsReady && drive.Name.Replace(":\\","") == driveName)
                {
                    return drive.TotalFreeSpace;
                }
            }
            return -1;
        }
        // End of Method function
    }

    /// <summary> 
    /// 做為字碼轉換工具 
    /// </summary> 
    public class CharSetConverter
    {
        internal const int LOCALE_SYSTEM_DEFAULT = 0x0800;
        internal const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        internal const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;

        /// <summary> 
        /// 使用OS的kernel.dll做為簡繁轉換工具，只要有裝OS就可以使用，不用額外引用dll，但只能做逐字轉換，無法進行詞意的轉換 
        /// <para>所以無法將電腦轉成計算機</para> 
        /// </summary> 
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);

        /// <summary> 
        /// 繁體轉簡體 
        /// </summary> 
        /// <param name="pSource">要轉換的繁體字：體</param> 
        /// <returns>轉換後的簡體字：体</returns> 
        public static string ToSimplified(string pSource)
        {
            String tTarget = new String(' ', pSource.Length);
            int tReturn = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_SIMPLIFIED_CHINESE, pSource, pSource.Length, tTarget, pSource.Length);
            return tTarget;
        }

        /// <summary> 
        /// 簡體轉繁體 
        /// </summary> 
        /// <param name="pSource">要轉換的繁體字：体</param> 
        /// <returns>轉換後的簡體字：體</returns> 
        public static string ToTraditional(string pSource)
        {
            String tTarget = new String(' ', pSource.Length);
            int tReturn = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_TRADITIONAL_CHINESE, pSource, pSource.Length, tTarget, pSource.Length);
            return tTarget;
        }
    }
}
