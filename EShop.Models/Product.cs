using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Models
{
    public class Product
    {
        public Product()
        {
            TempSqFt = 1;
        }

        [Key]
        public int Id { get; set; }
        [Display(Name = "Product Name")]
        [Required]
        public string ProductName { get; set; }
        public string ShortDesc { get; set; }

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


        [Display(Name = "Application Type")]
        public int? ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public ApplicationType ApplicationType { get; set; }

        [NotMapped]
        [Range(1,10000)]
        public int TempSqFt { get; set; }
    }
}
