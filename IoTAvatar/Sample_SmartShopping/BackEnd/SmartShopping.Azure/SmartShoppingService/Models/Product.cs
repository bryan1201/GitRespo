using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SmartShoppingDemoService.Models
{
    public class Product
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public double Price { get; set; }

        public string Image { get; set; }

        public string ProductLink { get; set; }
    }
}