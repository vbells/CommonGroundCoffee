namespace CommonGroundCoffee.Models
{
    public class ProductModel
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Description { get; set; } 
        public string Product_Type { get; set; }
        public decimal Unit_Price { get; set; }
        public int Quantity_Available { get; set; }
        public string ImageUrl { get; set; }

    }
}
