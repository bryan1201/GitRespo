using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SmartShoppingDemoProcess.Models
{
    public class Beacon
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string BeaconId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string StoreId { get; set; }

        public int? InFilter { get; set; }

        public int? OutFilter { get; set; }

        public int? Xaxis { get; set; }

        public int? Yaxis { get; set; }

        public double? Longitude { get; set; }

        public double? LatuLatitude { get; set; }
    }
}