﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Barcelona.Models;
using System.IO;
using Controllers.General;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
namespace VS2012MVC4.Controllers
{
    public static class FileTable
    {
        public static string tempSimEventVision = "tempSimEventVision";
        public static string tempSimEventHealthin = "tempSimEventHealthin";
        public static string EventHealthin = "EventHealthin";
        public static string EventVision = "EventVision";
    }

    public class MediaFileController : Controller
    {
        //
        // GET: /MediaFile/
        private DBContext db = new DBContext();
        private const string StorageProjectFiles = "StorageProjectFiles";
        private const string barcelona = "barcelona";

        // 顯示於Barcelona Fedora的Web browser
        // guid = '744265FC-9D33-4CEA-9E6D-FCA0376708AD'
        public ActionResult Simple(string sguid)
        {
            // guid是建立Media分享的群組id，有時間及流量限制，起過其中之一，就不可再觀看Media Video
            // 寄給分享對象的超連結為
            if (sguid == null || sguid.Trim() == "")
                sguid = "744265FC-9D33-4CEA-9E6D-FCA0376708AD";

            Guid guid = System.Guid.Empty;
            System.Guid.TryParse(sguid, out guid);

            var summaryAdvice = db.sharefilegroup.Where(x => x.GroupId.Equals(guid));
            ViewData["SummaryAdvice"] = summaryAdvice;

            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            var vsharefile = db.vsharefile.Where(x => x.GroupId.Equals(guid)).OrderByDescending(x => x.cdt);
            return View(vsharefile);
        }

        public ActionResult Index()
        {
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            var attach = db.attachFiles.Where(x => x.editor.Equals(Editor)).OrderByDescending(x => x.fileId);
            return View(attach);
        }

        public ActionResult ShareFileUser()
        {
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            var share = db.vsharefileUser.Where(x => x.UserName == Editor);
            var ss = db.vsharefileUser.ToList();
            //var attach = db.attachFiles.Where(x => x.editor.Equals(Editor)).OrderByDescending(x => x.fileId);
            return View(share);
        }

        public ActionResult Manage()
        {
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            var attach = db.attachFiles.Where(x => x.editor.Equals(Editor)).OrderByDescending(x => x.fileId);
            return View(attach);
        }

        public ActionResult DeleteFile(int fileId, bool record)
        {
            if (fileId == 0)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            string result = (from f in db.attachFiles.Where(x => x.fileId == fileId)
                             select f.fileName).FirstOrDefault();

            var attachFile = (from f in db.attachFiles.Where(x => x.fileId == fileId)
                              select f).FirstOrDefault();

            //string fullFilePath =  Server.MapPath(result);
            if (!string.IsNullOrEmpty(result))
            {
                DeleteBolb(result);
                db.attachFiles.Remove(attachFile);
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
                return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult GetFileVerPath(int fileId, bool record)
        {
            if (fileId == 0)
            {
                return View();
            }
            string result = (from f in db.attachFiles.Where(x => x.fileId == fileId)
                             select f.filePath + f.fileName).FirstOrDefault();

            var attachFile = (from f in db.attachFiles.Where(x => x.fileId == fileId)
                              select f).FirstOrDefault();

            //string fullFilePath =  Server.MapPath(result);
            if (!string.IsNullOrEmpty(result) && System.IO.File.Exists(result))
            {
                return File(System.IO.File.ReadAllBytes(result), "application/unknown", HttpUtility.UrlEncode(Path.GetFileName(result)));
            }
            else
                return View("File not exists: " + result);
        }

        //
        // GET: /LabInformation/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /LabInformation/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection labInfo, attachFile attach)
        {
            string FolderId = Constant.StorageContainer;
            string Editor = Method.GetLogonUserId(Session, this, User.Identity.Name.ToUpper());
            string FormId = System.Guid.NewGuid().ToString().ToUpper();
            attach.fID = FormId;
            attach.editor = Editor;
            attach.cdt = DateTime.Now;
            attach.udt = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    var r = new List<attachFile>();
                    int i = 0;

                    foreach (string file in Request.Files)
                    {
                        HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;
                        if (hpf.ContentLength == 0)
                            continue;

                        string uniqueBlobName = string.Format("{0}/{1}{2}", hpf.ContentType, Guid.NewGuid().ToString(), Path.GetExtension(hpf.FileName));
                        string filePath = FileToUpload(hpf, FolderId, uniqueBlobName);

                        string formId = FormId;

                        FileInfo newinfo = new FileInfo(hpf.FileName);
                        
                        r.Add(new attachFile()
                        {
                            fID = formId,
                            displayname = attach.displayname,
                            fileName = uniqueBlobName,
                            Length = hpf.ContentLength,
                            Type = hpf.ContentType,
                            folderId = FolderId,
                            editor = Editor,
                            filePath = filePath,
                            Version = attach.Version,

                            cdt = DateTime.Now.ToLocalTime(),
                            udt = DateTime.Now.ToLocalTime()
                        });

                        i++;
                    }
                    foreach (attachFile a in r)
                    {
                        db.attachFiles.Add(a);
                    }

                    //db.attachFiles.Add(attach);
                    db.SaveChanges();
                    return RedirectToAction("Manage");
                }
            }
            catch
            { 
            //
            }
            return View(attach);
        }

        private void DeleteBolb(string FileName)
        {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(Microsoft.Azure.CloudConfigurationManager.GetSetting(StorageProjectFiles));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(barcelona);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(FileName);
            blockBlob.DeleteIfExists();
        }

        private string GetFileUri(HttpPostedFileBase file)
        {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(Microsoft.Azure.CloudConfigurationManager.GetSetting(StorageProjectFiles));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(barcelona);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);
            return blockBlob.Uri.AbsoluteUri;
        }

        private string FileToUpload(HttpPostedFileBase file, string FolderId, string uniqueBlobName)
        {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(Microsoft.Azure.CloudConfigurationManager.GetSetting(StorageProjectFiles));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(FolderId);
            try
            {
                container.CreateIfNotExists();
            }
            catch (StorageException)
            {
                throw;
            }
            /*
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(newinfo.Name);
            await blockBlob.UploadFromFileAsync(newinfo.FullName, FileMode.Open);
             */
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(uniqueBlobName);
            blockBlob.Properties.ContentType = file.ContentType;
            blockBlob.UploadFromStream(file.InputStream);
            return blockBlob.Uri.AbsoluteUri;
        }
      

        /// <summary>
        /// Validates the connection string information in app.config and throws an exception if it looks like 
        /// the user hasn't updated this to valid values. 
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string</param>
        /// <returns>CloudStorageAccount object</returns>
        private static Microsoft.WindowsAzure.Storage.CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount;
            try
            {
                storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }
    }
}
