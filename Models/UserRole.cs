using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Web_App.Models
{
    public class UserRole
    {
        [StringLength(450)]
        public string Id { get; set; }
        [StringLength(450)]
        public string RoleId { get; set; }
        [ForeignKey("RoleId")]
        public AppRole appRole { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUsers appUsers { get; set; }
    }
}
