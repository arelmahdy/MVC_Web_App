using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Web_App.Models
{
    public class Post
    {
        public int Id { get; set; }
        [StringLength(100), Required(ErrorMessage = "عنوان الموضوع مطلوب"), Display(Name = "عنوان الموضوع")]
        public string Title { get; set; }
        [StringLength(4000), Required(ErrorMessage = "الموضوع مطلوب"), Display(Name = "الموضوع"), DataType(DataType.MultilineText)]
        public string PostContent { get; set; }
        [StringLength(1000), Display(Name = "الصورة"), DataType(DataType.Upload)]
        public string PostImage { get; set; }
        [StringLength(250), Required(ErrorMessage = "اسم كاتب الموضوع مطلوب"), Display(Name = "كاتب الموضوع")]
        public string Author { get; set; }

        [Display(Name = "تاريخ الموضوع")]
        public DateTime PostDate { get; set; }

        [Display(Name = "المشاهدات")]
        public int PostViews { get; set; }

        [ScaffoldColumn(true), Display(Name = "أعجبنى")]
        public int PostLike { get; set; }

        [Display(Name = "اسم المعجب")]
        public string LikeUserName { get; set; }

        [Display(Name = "التصنيف الفرعي"), Required(ErrorMessage = "اسم التصنيف الفرعي مطلوب")]
        public int SubId { get; set; }

        [ForeignKey("SubId")]
        public SubCategory SubCategory { get; set; }

        [Display(Name = "نشر")]
        public bool IsPublish { get; set; }

        [Display(Name = "اسم المنتج"), StringLength(70)]
        public string ProductName { get; set; }

        [Display(Name = "السعر")]
        public double? Price { get; set; }

        [Display(Name = "الخصم")]
        public int? Discount { get; set; }

    }
}
