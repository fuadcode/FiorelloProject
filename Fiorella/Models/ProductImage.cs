namespace Fiorello.Models
{
    public class ProductImage : BaseEntity
    {
        public bool IsMain { get; set; }
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
