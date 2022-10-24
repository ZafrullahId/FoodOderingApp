using EF_Core.Models.DTOs.RequesteModels;
using EF_Core.Models.DTOs.ResponseModels;
using EF_Core.Models.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EF_Core.Interface.Services
{
    public interface IStaffService
    {
        Task<BaseResponse> DeleteAsync(int id);
        Task<BaseResponse> EditAsync(int id, UpdateStaffRequestModel model);
        Task<StaffResponsModel> FindAsync(string email, string password);
        Task<StaffResponsModel> FindByIdAsync(int id);
        Task<StaffResponsModel> FindByRoleAsync(Role role);
        Task<List<StaffResponsModel>> GetAllAsync();
        Task<BaseResponse> RegisterAsync(CreateStaffRequestModel model);
        Task<StaffResponsModel> VeiwProfileAsync(int id);
    }
}