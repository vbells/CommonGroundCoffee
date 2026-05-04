namespace CommonGroundCoffee.Models
{
    public class OrdersModel
    {
        public int Order_ID { get; set; }
        public int Customer_ID { get; set; }
        public DateTime Order_Date { get; set; }
        public decimal Total_Price { get; set; }

    }
}
