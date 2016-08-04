using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SmartShoppingDemoService.Models
{
    public class Device
    {
        [Key]
        public long DevId { get; set; }

        [Required]
        [StringLength(50)]
        public string DeviceId { get; set; }

        public string DeviceKey { get; set; }

        public string ConnectionString { get; set; }

        public bool IsUsed { get; set; }

        public bool IsActive { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}