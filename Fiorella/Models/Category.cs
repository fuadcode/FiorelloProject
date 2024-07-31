using System.ComponentModel.DataAnnotations;

namespace Fiorello.Models
{
    public class Category : BaseEntity
    {
        [Required, StringLength(25)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public List<Product> Products { get; set; }
    }
}
