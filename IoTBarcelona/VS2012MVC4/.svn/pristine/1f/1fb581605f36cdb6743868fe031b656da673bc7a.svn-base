using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Barcelona.Models
{
    public class RoleModels
    {
        
    }

    public class Roles
    { 
    //RoleId, ApplicationId, RoleName, Description
        [Key]
        [Required]
        [Column(Order = 0)]
        public Guid RoleId { get; set; }

        [Key]
        [Required]
        [Column(Order = 1)]
        public Guid ApplicationId { get; set; }

        public string RoleName { get; set; }
        public string Description { get; set; }
    }

    public class UsersInRoles
    {
        [Key]
        [Required]
        [Column(Order = 0)]
        public Guid RoleId { get; set; }

        [Key]
        [Required]
        [Column(Order = 1)]
        public Guid ApplicationId { get; set; }
    }

    public class vUser
    {
        [Key]
        public Guid UserId { get; set; }
        public Guid ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string AppDescription { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}