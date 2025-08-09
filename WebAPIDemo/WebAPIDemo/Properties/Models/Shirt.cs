using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIDemo.Properties.Models.Validations;

namespace WebAPIDemo.Properties.Models
{
    [Table("shirts")] // This maps the entity to the lowercase "shirts" table
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
        [Shirt_EnsureCorrectSizingAttribute]
        public string Size { get; set; } = string.Empty;

        [Range(0.01, 1000.00, ErrorMessage = "Price must be between 0.01 and 1000.00.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(20, ErrorMessage = "Gender cannot exceed 20 characters.")]
        public string Gender { get; set; } = string.Empty; // e.g., "Men" or "Women"

        public override string ToString()
        {
            return $"{Name} ({Color}, {Size}, ${Price}, {Gender})";
        }
    }
}