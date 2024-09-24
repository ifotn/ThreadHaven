namespace ThreadHaven.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        // child reference to Product model
        public List<Product>? Products { get; set; }
    }
}
