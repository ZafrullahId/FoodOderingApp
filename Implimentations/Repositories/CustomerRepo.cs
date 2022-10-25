using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF_Core.Interface.Repositories;
using EF_Core.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace EF_Core.Implimentations.Repositories
{

    public class CustomerRepo : ICustomerRepo
    {
        private readonly AppDbContext _context;
        public CustomerRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Customer> CreateAsync(Customer customer)
        {
            var find = await _context.Users.Where(x => x.Email == customer.User.Email).AnyAsync();
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> CheckIfExist(string email)
        {
            var find = await _context.Users.Where(x => x.Email == email).AnyAsync();
            return find;
        }
        public async Task<bool> UpdateAsync(Customer updatedCustomer)
        {
            _context.Customers.Update(updatedCustomer);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Customer> GetByIdAsync(int customerId)
        {
            return await _context.Customers
            .Include(c => c.User)
            .SingleOrDefaultAsync(x => x.Id == customerId);
        }
        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await _context.Customers
            .Include(c => c.User)
            .SingleOrDefaultAsync(x => x.User.Email == email);
        }
        public async Task<bool> CheckEmailAsnc(string email)
        {
            return await _context.Customers
            .Include(c => c.User)
            .Where(x => x.User.Email == email).AnyAsync();
        }
        public async Task<bool> DeleteAsync(string email)
        {
            var customer = await _context.Users
              .FirstOrDefaultAsync(x => x.Email == email);
              
            _context.Users.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Customer>> ListAsync()
        {
            return await _context.Customers
                .Include(c => c.User)
                .ToListAsync();
        }

    }
}