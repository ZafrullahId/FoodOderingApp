using System.Collections.Generic;
using System.Threading.Tasks;
using EF_Core.Models.Entity;
using EF_Core.Models.Enum;

namespace EF_Core.Interface.Repositories
{
    public interface IStaffRepo
    {
        Task<bool> CreateAsync(Staff staff);
        Task<bool> DeleteAsync(Staff staff);
        Task<Staff> GetByEmailAsync(string email);
        Task<Staff> GetByIdAsync(int staffId);
        Task<Staff> GetByRoleAsync(Role role);
        Task<List<Staff>> ListAsync();
        Task<bool> UpdateAsync(Staff updatedstaff);
    }
}