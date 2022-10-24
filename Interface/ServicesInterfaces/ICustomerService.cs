using EF_Core.Models.Dto;
using EF_Core.Models.DTOs.RequesteModels;
using EF_Core.Models.DTOs.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EF_Core.Interface.Services
{
    public interface ICustomerService
    {
        Task<CustomerResponsModel> CheckWalletAsync(string email);
        Task<BaseResponse> DeleteAsync(string email);
        Task<BaseResponse> EditAsync(string email, UpdateCustomerRequestModel model);
        Task<CustomerResponsModel> FindAsync(string email, string password);
        Task<CustomerResponsModel> FindByEmailAsync(string email);
        Task<BaseResponse> FundWalletAsync(string email, decimal amount);
        Task<List<CustomerResponsModel>> GetAllAsync();
        Task<BaseResponse> RegisterAsync(CreateCustomerRequestModel model);
        Task<BaseResponse> UpdateWalletAsync(string email, UpdateCustomerRequestModel model);
        Task<CustomerResponsModel> VeiwProfileAsync(string email);
    }
}
