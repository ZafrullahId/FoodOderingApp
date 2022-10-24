using EF_Core.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EF_Core.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using EF_Core.Models.Enum;
using System;
using EF_Core.Interface.Services;
using EF_Core.Models.DTOs.RequesteModels;

namespace MVC_Project.Controllers
{
    public class OrderController : Controller
    { 
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IFoodService _foodService;
        public OrderController(IStaffService staffService,IOrderService orderService,ICustomerService customerService,IFoodService foodService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _foodService = foodService;
            
        }
        [ActionName("Orders")]
        public async Task<IActionResult> OrdersAsync()
        {
            if(HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AdminLogin","Staff");
            }
            return View(await _orderService.OrdersAsync());
        } 
        [ActionName("Order")]
        public async Task<IActionResult> OrderAsync(int id,CreateOrderRequestModel model)
        {
                if(HttpContext.Session.GetString("Email") == null)
                {
                    return RedirectToAction("LogIn","Customer");
                }
            if(HttpContext.Request.Method == "POST")
            {
                var food = await _foodService.GetByIdAsync(id);
                if(food == null || food.Data.Status == Status.NotAvailable || food.Data.Status == Status.Processing)
                {
                    return Content("Food not available at the moment");
                }
                else
                {
                    // var customer = await _customerService.FindByEmailAsync(HttpContext.Session.GetString("Email"));
                   var response = await _orderService.CalculatePriceAsync(HttpContext.Session.GetString("Email"),food.Data.Price);
                    if(response.Success == false)
                    {
                        return Content(response.Message);
                    }
                    else
                    {
                        // var custom = await _customerService.CheckWalletAsync(HttpContext.Session.GetString("Email"));
                    //    await _orderService.CalculatePriceAsync(HttpContext.Session.GetString("Email"),food.Data.Price);
                        // custom.Wallet -= food.Price;
                        // updateCustomerDto.Wallet = custom.Wallet;
                        // await  _customerService.WalletUpdateAsnc(HttpContext.Session.GetString("Email"),updateCustomerDto);

                        // var customer1 = await _customerService.FindByEmailAsync(HttpContext.Session.GetString("Email"));
                       var order = await _orderService.CreateOrderAsync(model,HttpContext.Session.GetString("Email"),id);
                        // var order = new Order()
                        // {
                        //     CreatedAt = DateTime.Now,
                        //     UpdatedAt = DateTime.Now,
                        //     IsDelivered = false,
                        //     CustomerId = customer1.Id
                        // };
                        // await _orderService.CreateOrderAsync(order);
                        // var order = _orderService.CreateOrderAsync(HttpContext.Session.GetString("Email"));
                        //   var foodOrder = new FoodOrder()
                        // {
                        //     UpdatedAt = DateTime.Now,
                        //     CreatedAt = DateTime.Now,
                        //     FoodId = food.Id,
                        //     OrderId = order.Id,
                        // };
                    //   await  _orderService.CreateFoodOrderAsync(foodOrder);
                        return Content(order.Message + "," + response.Message);
                    }
                }
            }
           return View(await _foodService.GetByIdAsync(id));
        }

        public async Task<IActionResult> DeliveryStatus(int id)
        {
           var order = await _orderService.UpdateStatus(id);
           if(order.Success == false)
           {
             return Content(order.Message);
           }
           return RedirectToAction("Orders");
        //    order.Data.IsDelivered = true;
        //    await _orderService.UpdateStatus(order);
        }
    }
}