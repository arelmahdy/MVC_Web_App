using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Web_App.Models
{
    public class AppUsers
    {
        [StringLength(450)]
        public string Id { get; set; }

        [StringLength(250),Required(ErrorMessage = "اسم المستخدم مطلوب"), Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }

        [StringLength(50),Required(ErrorMessage = "كلمة المرور مطلوبة "), Display(Name ="كلمة المرور "),DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "كلمتا المرور غير متطابفتين"), StringLength(50),Required(ErrorMessage = "تأكيد كلمة المرور  "), Display(Name="تأكيد كلمة المرور"),DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [StringLength(650),Required(ErrorMessage ="البريد الاكترونى مطلوب"),Display(Name ="البريد الالكترونى"),DataType(DataType.EmailAddress)]
        public string Email  { get; set; }
        
        [Display(Name = "تأكيد البريد الالكترونى")]
        public bool EmailConfirm { get; set; }

        [StringLength(100) ,Display(Name="الهاتف"),DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [ScaffoldColumn(true)]
        public bool LockOut { get; set; }
        [ScaffoldColumn(true)]
        public DateTime? LockTime { get; set; }
        [ScaffoldColumn(true)]
        public int ErrorLogCount { get; set; }

    }
}
