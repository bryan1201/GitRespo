//------------------------------------------------------------------------------
// <auto-generated>
//    這個程式碼是由範本產生。
//
//    對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//    如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Routes.Models
{
    using System;
    
    public partial class FnGetTask_Result
    {
        public string TaskID { get; set; }
        public string FID { get; set; }
        public string EquID { get; set; }
        public Nullable<int> FormCode { get; set; }
        public Nullable<int> FlowCode { get; set; }
        public Nullable<int> State { get; set; }
        public string Applicant { get; set; }
        public Nullable<System.DateTime> Cdt { get; set; }
        public Nullable<System.DateTime> Udt { get; set; }
    }
}