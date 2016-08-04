using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Barcelona.Models
{
    public class ExcelModels
    {
    }

    public class attachFileTable
    {
        //ID, FileTable, TempTable, DisplayName
        [Key]
        public Int64 ID { get; set; }
        public string FileTable { get; set; }
        public string TempTable { get; set; }
        public string DisplayName { get; set; }
        public int ColumnsCount { get; set; }
    }

    public class EventVision
    {//Id, AdultQty, Area, X, Y, cdt, IPCamID
        [Key]
        public Int64 Id { get; set; }
        [Display(Name = "AdultQ")]
        public int AdultQty { get; set; }

        public string Area { get; set; }
        public Nullable<double> X { get; set; }
        public Nullable<double> Y { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }

        [Display(Name = "IPCam")]
        public string IPCamID { get; set; }
    }

    public class EventHealthin
    {//Id, Bloodpressure1, Bloodpressure2, Bloodglucose, Heartbeat, UserId, BarceloneId, HealthinId, cdt
        [Key]
        public Int64 Id { get; set; }

        public Nullable<double> Bloodpressure1 { get; set; }
        public Nullable<double> Bloodpressure2 { get; set; }
        public Nullable<double> Bloodglucose { get; set; }
        public string UserId { get; set; }
        public string BarcelonaId { get; set; }
        public string HealthinId { get; set; }

        [Display(Name = "Create")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime cdt { get; set; }
    }

   
}