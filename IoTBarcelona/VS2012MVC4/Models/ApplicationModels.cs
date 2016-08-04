using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Barcelona.Models
{
    public class Applications
    {
        //ApplicationId, ApplicationName, Description
        [Key]
        public Guid ApplicationId { get; set; }
        [Required]
        public string ApplicationName { get; set; }
        public string Description { get; set; }
    }

}