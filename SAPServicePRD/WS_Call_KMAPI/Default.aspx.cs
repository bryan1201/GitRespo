using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.Text;
using Jayrock.Json;
using Jayrock.Json.Conversion;

using System.IO;
using System.Collections;
using System.Collections.Specialized;

public partial class _Default : System.Web.UI.Page
{

    private string DataFormat = "JSON";                             // (必須)API資料傳輸格式，建議預設使用JSON
    private string userid = "IEC940480";                                // (必須)KM系統中有權限讀寫的帳號，建議使用系統管理者帳號
    private string API_Key = "096481b7b31c419ab91423ea619cd486";    // (必須)KM系統中已註冊並啟用的API Key
    private string KMUrl = "http://km.iec.inventec/ESP/api/";
    // "http://vitalskm.gss.com.tw/pdca/api/";              // (必須)KM Server Site的API虛擬目錄URL路徑

    private string tenant = "psg"; //要改PSG使用20230117

    private string GlobalFolderId = "35330";//要改FolderID使用20230117   // 文件夾ID (新增文件時，必須使用傳入的參數之一)
    private string GlobalDocClassId = "1";                          // 知識類型ID (新增文件時，必須使用傳入的參數之一)
    private string UploadCategoryId = "5791257";                     // 分類主題項目ID (新增文件時，使用傳入的參數之一，如有多個請用,隔開)

    private string UploadFileName1 = @"\\10.1.251.134\pu\TEST\BU1_HP_ITO_SA_20221220-172226.xlsx";  //"\\testUpload2.txt";   // 附件檔案名稱1 (新增文件時，使用傳入的參數之一)
    private string UploadFileName2 = "\\testUpload.txt";   // 附件檔案名稱2 (新增文件時，使用傳入的參數之一)

    private string GlobalSearchKeyword = "KM";                      // 「搜尋取回一筆」&「搜尋取回全部結果」測試功能中，若文字方塊空白時，預設進行查詢所使用的關鍵字
    private string GlobalTag = "15433732";                           // 取某文件標籤、取得熱門標籤 兩個測試功能會用到的標籤(Tag)ID

    private string GlobalCurrentDocumentId = "";             // 暫時無作用,不需要去動它
    private string GlobalCurrentCategoryId = "1";                   // 暫時無作用,不需要去動它

    private string ErrMessage = "";

    public enum ServiceType
    {
        GetRootFolder,
        UploadFile,
        AcquireDocumentDraft,
        SubmitNewDocument,
        AllDocumentClass,
        GetCategoryInfo,
        GetSearchExBySimple,
        DeleteDocument,
        GetDocument,
        AddUser,
        GetUser,
        GetUserBySubjectId,
        UpdateUser,
        DeleteUser,
        GetAllUser,
        DownloadFile2,
        GetAdvancedResult,
        AcquireFolderDraft,
        AddFolder,
        UpdateDocumentFiles
    }

    ///
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnCreateNewDoc_Click(object sender, EventArgs e)
    {
        

        CreateNewDoc NewDoc = new CreateNewDoc();
        //string aa = NewDoc.CreateNew("32", "rwe", "rew");
        lblResult.Text = NewDoc.CreateNew(txtFolderID.Text.Trim(), txtSourceFile.Text.Trim(), txtKMDocName.Text.Trim());
        

    }

}