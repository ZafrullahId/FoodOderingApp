using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF_Core.Interface.Repositories;
using EF_Core.Models.Entity;
using EF_Core.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace EF_Core.Implimentations.Repositories
{

    public class StaffRepo : IStaffRepo
    {
        private readonly AppDbContext _context;
        public StaffRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateAsync(Staff staff)
        {
            if (staff == null)
            {
                return false;
            }
            var find = await _context.Users.Where(x => x.Email == staff.User.Email).AnyAsync();
            if (find)
            {
                return false;
            }
           await _context.Staffs.AddAsync(staff);
          await  _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(Staff updatedstaff)
        {
            _context.Staffs.Update(updatedstaff);
           await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Staff> GetByIdAsync(int staffId)
        {
            return await _context.Staffs
            .Include(c => c.User)
            .ThenInclude(x => x.Address)
            .SingleOrDefaultAsync(x => x.Id == staffId);
        }
        public async Task<Staff> GetByEmailAsync(string email)
        {
            return await _context.Staffs
            .Include(c => c.User.Address).Include(x => x.User)
            .SingleOrDefaultAsync(x => x.User.Email == email);
        }
        public async Task<Staff> GetByRoleAsync(Role role)
        {
            return await _context.Staffs
            .Include(c => c.User)
            .SingleOrDefaultAsync(x => x.Role == role);
        }
        public async Task<bool> DeleteAsync(Staff staff)
        {
            // var staff = await _context.Staffs
            // .FirstOrDefaultAsync(x => x.Id == staffId);
            _context.Staffs.Remove(staff);
          await  _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Staff>> ListAsync()
        {
            return await _context.Staffs
                .Include(c => c.User)
                .ThenInclude(x => x.Address)
                .ToListAsync();
        }


    }
}