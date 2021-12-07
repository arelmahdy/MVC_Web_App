using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Web_App.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        [StringLength(100),Display(Name ="التصنيفات الفرعية"),Required(ErrorMessage ="اسم التصنيف الفرعى مطلوب")]
        public string SubCatName { get; set; }
        public int CatId { get; set; }
        [ForeignKey("CatId")]
        public Category Category { get; set; }
    }
}
