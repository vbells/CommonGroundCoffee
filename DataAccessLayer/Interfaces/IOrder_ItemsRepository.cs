using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IOrder_ItemsRepository
    {
        Task<IEnumerable<Order_Items>> GetOrder_ItemsAsync();
    }
}
