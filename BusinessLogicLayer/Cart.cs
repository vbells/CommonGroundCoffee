using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new();

        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(x => x.Product_ID == item.Product_ID);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(x => x.Product_ID == productId);
            if (item != null)
                Items.Remove(item);
        }

        public decimal GetTotal()
        {
            return Items.Sum(x => x.Price * x.Quantity);
        }
    }
}
