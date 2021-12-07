using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Web_App.Models
{
    public class AppConfrim
    {
        [StringLength(450)]
        public string Id { get; set; }
        [StringLength(450), Required, Display(Name = "معرف المستخدم")]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public  AppUsers AppUser { get; set; }
        [Required,Display(Name ="التاريخ"),DataType(DataType.DateTime)]
        public DateTime DateConfrim { get; set; }
    }
}
