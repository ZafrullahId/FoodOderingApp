using System.Collections.Generic;
using System.Threading.Tasks;
using EF_Core.Models.Entity;

namespace EF_Core.Interface.Repositories
{
    public interface IOrderRepo
    {
        Task<Order> CreateAsync(Order order);
        Task<bool> InsertAsync(FoodOrder foodOrder);
        Task<List<FoodOrder>> OrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<bool> UpdateAsync(Order order);
    }
}