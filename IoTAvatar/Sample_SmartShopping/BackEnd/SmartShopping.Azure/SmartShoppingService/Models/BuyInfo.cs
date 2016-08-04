using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SmartShoppingDemoService.Models
{
    public class BuyInfo
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string DeviceId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public double Amount { get; set; }

        public DateTime BuyTime { get; set; }
    }
}