using System.ComponentModel.DataAnnotations;

namespace ThreadHaven.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // display using MS weird current format "c" for currency   
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }
        public string Size { get; set; }
        public string? Photo { get; set; }

        // Foreign Key
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        // Parent reference to Category model
        public Category? Category { get; set; }

        // child refs to CartItem & OrderDetail models
        public List<CartItem>? CartItems { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
