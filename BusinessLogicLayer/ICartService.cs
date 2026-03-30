namespace BusinessLogicLayer
{
    public interface ICartService
    {
        Cart GetCart();
        Task AddToCart(int productId, int quantity);
        void RemoveFromCart(int productId);
        void UpdateQuantity(int productId, string direction); // ← new
    }
}