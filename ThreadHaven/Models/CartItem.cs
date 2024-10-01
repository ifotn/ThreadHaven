using System.ComponentModel.DataAnnotations;

namespace ThreadHaven.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string Size { get; set; }

        // parent ref
        public Product? Product { get; set; }
    }
}
