using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF_Core.Interface.Repositories;
using EF_Core.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace EF_Core.Implimentations.Repositories
{

    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _context;

        public OrderRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Order> CreateAsync(Order order)
        {
           await _context.Orders.AddAsync(order);
           await _context.SaveChangesAsync();
            return order;
        }
        public async Task<bool> InsertAsync(FoodOrder foodOrder)
        {
           await _context.foodOrder.AddAsync(foodOrder);
           await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FoodOrder>> OrdersAsync()
        {
            return await _context.foodOrder
            .Include(c => c.Food)
            .Include(x => x.Order)
            .ThenInclude(y => y.Customer)
            .ThenInclude(z => z.User).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}