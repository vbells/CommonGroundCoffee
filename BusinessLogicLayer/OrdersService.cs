using DataAccessLayer.Interfaces;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        
        public OrdersService(IOrdersRepository ordersRepository)
        {
            this._ordersRepository = ordersRepository;
        }

        public async Task<IEnumerable<Orders>> GetOrdersAsync()
        {
            return await _ordersRepository.GetOrdersAsync();
        }
    }
}
