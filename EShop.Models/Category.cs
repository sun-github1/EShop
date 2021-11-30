using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Models
{
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Display(Name = "Category Name")]
        [Required]
        public string CategoryName { get; set; }
        [Display(Name = "Display Order")]
        [Range(1, int.MaxValue, ErrorMessage = "Display Order must be greater than 0")]
        public int DisplayOrder { get; set; }
    }
}
