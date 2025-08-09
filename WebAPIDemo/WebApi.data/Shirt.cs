using System.ComponentModel.DataAnnotations;

namespace WebAPIDemo.Properties.Models
{
    public class Shirt
    {
        public int ShirtId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Color is required.")]
        [StringLength(50, ErrorMessage = "Color cannot exceed 50 characters.")]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "Size is required.")]
        [StringLength(10, ErrorMessage = "Size cannot exceed 10 characters.")]
        public string Size { get; set; } = string.Empty;

        [Range(0.01, 1000.00, ErrorMessage = "Price must be between 0.01 and 1000.00.")]
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Color}, {Size}, ${Price})";
        }
    }
}