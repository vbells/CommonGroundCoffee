using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class Order_ItemsService : IOrder_ItemsService
    {
        private readonly IOrder_ItemsRepository _order_ItemRepository;

        public Order_ItemsService(IOrder_ItemsRepository order_ItemsRepository)
        {
            this._order_ItemRepository = order_ItemsRepository;
        }

        public async Task<IEnumerable<Order_Items>> GetOrder_ItemsAsync()
        {
            return await _order_ItemRepository.GetOrder_ItemsAsync();
        }
    }
}
