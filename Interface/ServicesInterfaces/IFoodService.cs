using EF_Core.Models.Dto;
using EF_Core.Models.DTOs.RequesteModels;
using EF_Core.Models.DTOs.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EF_Core.Interface.Services
{
    public interface IFoodService
    {
        Task<BaseResponse> AddAsync(CreateFoodRequestModel model);
        Task<BaseResponse> DeleteFoodAsync(int foodId);
        Task<List<FoodResponsModel>> GetAllFoodAsync();
        Task<FoodResponsModel> GetByIdAsync(int id);
        Task<BaseResponse> UpdateFoodAsync(UpdateFoodRequestModel model, int id);
        Task UpdateFoodStatusAsync();
    }
}