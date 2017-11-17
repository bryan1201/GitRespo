using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace einvoice.Models
{
    public class TURNKEY_SYSEVENT_LOG
    {
        [Key]
        public string EVENTDTS { get; set; }
        public string PARTY_ID { get; set; }
        public string SEQNO { get; set; }
        public string SUBSEQNO { get; set; }
        public string ERRORCODE { get; set; }
        public string UUID { get; set; }
        public string INFORMATION1 { get; set; }
        public string INFORMATION2 { get; set; }
        public string INFORMATION3 { get; set; }
        public string MESSAGE1 { get; set; }
        public string MESSAGE2 { get; set; }
        public string MESSAGE3 { get; set; }
        public string MESSAGE4 { get; set; }
        public string MESSAGE5 { get; set; }
        public string MESSAGE6 { get; set; }
    }
}