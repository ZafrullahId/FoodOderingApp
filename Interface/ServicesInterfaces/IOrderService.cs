using System.Collections.Generic;
using System.Threading.Tasks;
using EF_Core.Models.Dto;
using EF_Core.Models.DTOs.RequesteModels;
using EF_Core.Models.DTOs.ResponseModels;
using EF_Core.Models.Entity;

namespace EF_Core.Interface.Services
{
    public interface IOrderService
    {
        Task<BaseResponse> CalculatePriceAsync(string email, decimal price);
        Task<BaseResponse> CreateOrderAsync(CreateOrderRequestModel model, string email, int id);
        Task<OrderResponseModel> GetOrderById(int id);
        Task<List<FoodOrder>> OrdersAsync();
        Task<BaseResponse> UpdateStatus(int id);
    }
}