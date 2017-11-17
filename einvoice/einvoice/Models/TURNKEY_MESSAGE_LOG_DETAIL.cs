using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace einvoice.Models
{
    public class TURNKEY_MESSAGE_LOG_DETAIL
    {
        public string SEQNO { get; set; }
        public string SUBSEQNO { get; set; }

        [Key]
        public string PROCESS_DTS { get; set; }
        public string TASK { get; set; }
        public string STATUS { get; set; }
        public string FILENAME { get; set; }
        public string UUID { get; set; }
    }
}