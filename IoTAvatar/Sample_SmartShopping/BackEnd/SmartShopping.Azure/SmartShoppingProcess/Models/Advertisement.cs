using System;
using System.ComponentModel.DataAnnotations;

namespace SmartShoppingDemoProcess.Models
{
    public class Advertisement
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string BeaconId { get; set; }

        [Required]
        [StringLength(50)]
        public string TargetDeviceId { get; set; }

        public int SignalStrength { get; set; }

        public DateTime Timestamp { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}