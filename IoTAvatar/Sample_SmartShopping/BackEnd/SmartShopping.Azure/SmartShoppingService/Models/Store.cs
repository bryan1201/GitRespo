using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SmartShoppingDemoService.Models
{
    public class Store
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string StoreId { get; set; }

        [Required]
        public string StoreName { get; set; }
    }
}