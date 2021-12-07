using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Web_App.Models
{
    public class UserProfile
    {
        [Display(Name="معرف الملف الشخصى")]
        public long Id { get; set; }
        [Display(Name ="محل الاقامة"),StringLength(100)]
        public string Country { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUsers AppUsers { get; set; }
        [Display(Name ="تاريخ الميلاد"),DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(200),Display(Name ="رابط موقعك"),DataType(DataType.Url)]
        public string PersonalWebUrl { get; set; }
        [StringLength(400),Display(Name ="الصورة")]
        public string UrlImage { get; set; }

    }
}
