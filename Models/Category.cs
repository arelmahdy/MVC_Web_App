using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Web_App.Models
{
    public class Category
    {
        public int Id { get; set; }
        [StringLength(50),Display(Name ="التصنيفات الاساسية"),Required(ErrorMessage ="اسم التصنيف الاساسى مطلوب")]
        public string CatName { get; set; }
    }
}
