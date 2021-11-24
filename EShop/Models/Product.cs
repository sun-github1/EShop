using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Product Name")]
        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price should be greater than 0")]
        public double Price { get; set; }

        public string ImagePath { get; set; }
        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
