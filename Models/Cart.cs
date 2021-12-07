using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Web_App.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [StringLength(70),Display(Name ="اسم المنتج")]
        public string ProductName { get; set; }
        [Display(Name ="السعر"),DataType(DataType.Currency)]
        public double? Price { get; set; }
        [Display(Name ="الخصم")]
        public int? Discount { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUsers appUser { get; set; }
    }
}
