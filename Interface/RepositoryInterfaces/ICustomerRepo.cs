using System.Collections.Generic;
using System.Threading.Tasks;
using EF_Core.Models.Entity;

namespace EF_Core.Interface.Repositories
{
    public interface ICustomerRepo
    {
        Task<bool> CheckEmailAsnc(string email);
         Task<Customer> CreateAsync(Customer customer);
        Task<bool> DeleteAsync(string email);
        Task<Customer> GetByEmailAsync(string email);
        Task<Customer> GetByIdAsync(int customerId);
        Task<List<Customer>> ListAsync();
        Task<bool> CheckIfExist(string email);
        Task<bool> UpdateAsync(Customer updatedCustomer);
    }
}