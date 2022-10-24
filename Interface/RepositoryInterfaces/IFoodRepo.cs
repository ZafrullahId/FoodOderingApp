using System.Collections.Generic;
using System.Threading.Tasks;
using EF_Core.Models.Entity;

namespace EF_Core.Interface.Repositories
{
    public interface IFoodRepo
    {
        Task<bool> CheckIfExist(string type);
        Task<Food> CreateAsync(Food food);
        Task<Food> DeleteAsync(int foodId);
        Task<Food> GetByIdAsync(int id);
        Task<Food> GetByPriceAsync(decimal price);
        Task<List<Food>> ListAsync();
        Task<Food> UpdateAsync(Food food);
    }
}