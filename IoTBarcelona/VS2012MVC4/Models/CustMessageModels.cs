using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Barcelona.Models
{
    public class CustMessageModels
    {
    }

    // Android Message
    // {"data":{"message":"Notification Hub test notification"}}
    public class AndroidMessageBody
    {
        public string message { get; set; }
    }

    public class CustNotification
    {
        public string Receivers { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class CustMailAttach
    {//attachId, MsgId, ShareFileGroupId
        [Key]
        public Guid attachId { get; set; }
        public Guid MsgId { get; set; }
        public Guid ShareFileGroupId { get; set; }
    }

    public class CustMailMessage
    {
        [Key]
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public DateTime cdt { get; set; }
        public bool IsSuccess { get; set; }
    }

}