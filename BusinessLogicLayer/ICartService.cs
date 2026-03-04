using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface ICartService
    {
        Cart GetCart();
        void AddToCart(int productId, int quantity);
        void RemoveFromCart(int productId);
    }
}
