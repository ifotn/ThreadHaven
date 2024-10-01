using System.ComponentModel.DataAnnotations;

namespace ThreadHaven.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string Size { get; set; }

        // parent ref to Order & Product models
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
