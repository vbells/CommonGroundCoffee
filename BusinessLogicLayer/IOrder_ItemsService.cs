using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IOrder_ItemsService
    {
        Task<IEnumerable<Order_Items>> GetOrder_ItemsAsync();
    }
}
