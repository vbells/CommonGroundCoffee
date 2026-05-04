namespace CommonGroundCoffee.Models
{
    public class Order_ItemsModel
    {
        public int Order_Item_ID { get; set; }
        public int Order_ID { get; set; }
        public int Product_ID { get; set; }
        public int Quantity { get; set; }
        public decimal Price_at_Purchase { get; set; }

    }
}
