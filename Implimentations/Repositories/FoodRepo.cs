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
    public class FoodRepo : IFoodRepo
    {
        public readonly AppDbContext _context;
        public FoodRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Food> CreateAsync(Food food)
        {
            await _context.Foods.AddAsync(food);
            _context.SaveChanges();
            return food;
        }

        public async Task<bool> CheckIfExist(string type)
        {
            return await _context.Foods.Where(x => x.Type == type).AnyAsync();
        }
        public async Task<Food> UpdateAsync(Food food)
        {
            _context.Foods.Update(food);
            await _context.SaveChangesAsync();
            return food;
        }

        public async Task<Food> GetByIdAsync(int id)
        {
            return await _context.Foods
            .SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Food> GetByPriceAsync(decimal price)
        {
            return await _context.Foods
            .SingleOrDefaultAsync(x => x.Price >= price);
        }
        public async Task<Food> DeleteAsync(int foodId)
        {
            var food = await _context.Foods
            .FirstOrDefaultAsync(x => x.Id == foodId);
            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();
            return food;
        }
        public async Task<List<Food>> ListAsync()
        {
            return await _context.Foods
                .ToListAsync();
        }

    }
}