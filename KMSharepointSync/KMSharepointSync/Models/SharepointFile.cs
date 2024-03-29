using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMSharepointSync.Models
{
    public class SharepointFile
    {
        public string TaskId { get; set; }
        public string UniqueId { get; set; } // {3aa81dba-3979-433e-8cbe-49ca5c8bb545}
        public string ParentUniqueId { get; set; } // "{E4149CF4-6356-4E05-826F-4DA0928F3175}"
        /*
            SELECT KM_Id, SP_Id,KM_Path FROM [dbo].[v_SharepointKM_FolderPathMapping] WHERE SP_Id= 'E4149CF4-6356-4E05-826F-4DA0928F3175';
            KM_Id	SP_Id	KM_Path
            39672	e4149cf4-6356-4e05-826f-4da0928f3175	/智慧醫材電子系統/000_公告區(相關說明請見此資料夾)/
         */
        public string ServerRedirectedEmbedUri { get; set; }
        public string FileLeafRef { get; set; } //檔名
        public string FileDirRef { get; set; } // 除了網站Url的路徑(含檔名) "/sites/msteams_a0f382_290647/Shared Documents/智慧醫材電子系統/000_公告區(相關說明請見此資料夾)/QMS 2.0 一到四階文件全面電子化發行公告 (2023_11_01 起生效).pdf"
        public string FileRef { get; set; } //"/sites/msteams_a0f382_290647/Shared Documents/智慧醫材電子系統/000_公告區(相關說明請見此資料夾)"
        public string Author { get; set; }
        public string Editor { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
    }
}