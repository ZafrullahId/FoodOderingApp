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
                   var response = await _orderService.CalculatePriceAsync(HttpContext.Session.GetString("Email"),food.Data.Price);
                    if(response.Success == false)
                    {
                        return Content(response.Message);
                    }
                    else
                    {
                       var order = await _orderService.CreateOrderAsync(model,HttpContext.Session.GetString("Email"),id);
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
        }
        [ActionName("CustomerOrderProfile")]
         public async Task<IActionResult> CustomerOrderProfileAsync(int id)
        {
            var info = await _orderService.GetOrderById(id);
            return View(info);
        }
    }
}