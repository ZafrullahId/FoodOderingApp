using EF_Core.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using EF_Core.Interface.Services;
using EF_Core.Models.DTOs.RequesteModels;

namespace MVC_Project.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IFoodService _foodService;
        public CustomerController(ICustomerService customerService, IFoodService foodService)
        {
            _customerService = customerService;
            _foodService = foodService;
        }
        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            if(HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("LogIn");
            }
            await _foodService.UpdateFoodStatusAsync();
            return View(await _foodService.GetAllFoodAsync());
        }
        [ActionName("Register")]
        public async Task<IActionResult> RegisterAsync(CreateCustomerRequestModel model)
        {
             if(HttpContext.Request.Method == "POST")
            {
                var result = await _customerService.RegisterAsync(model);
                 if (result.Success == true)
                {
                    return RedirectToAction("LogIn");
                }
                return StatusCode(406,"Registration Failed. Email may exist already");
            }
            return View();
        }
        [ActionName("LogIn")]
        public async Task<IActionResult> LogInAsync(string email, string password)
        {
            if(HttpContext.Request.Method == "POST")
            {
                var result = await _customerService.FindAsync(email,password);
                if (result.Success == true)
                {
                     HttpContext.Session.SetString("Email", email);
                    return RedirectToAction("Index");
                }
                return StatusCode(406,"Login Failed");
            }
            return View();
        }
        [ActionName("FundWallet")]
        public async Task<IActionResult> FundWalletAsync(decimal amount)
        {
            if(HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("LogIn");
            }
            if(HttpContext.Request.Method == "POST")
            {                
                var ans = await _customerService.FundWalletAsync(HttpContext.Session.GetString("Email"),amount);
                if(ans.Success == true)
                {
                    return StatusCode(201,"Wallet Successfully Updated");
                }
                    return StatusCode(406,"Wallet Updated Failed");
            }
            return View();
        }
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync()
        {
            if(HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("LogIn");
            }
            if(HttpContext.Request.Method == "POST")
            {                
                var ans = await _customerService.DeleteAsync(HttpContext.Session.GetString("Email"));
                if(ans.Success == true)
                {
                    return StatusCode(201,"Account Successfully Deleted");
                }
                    return StatusCode(406,"Process Failed");
            }
            return View();
        }

        [ActionName("Edit")]
        public async Task<IActionResult> EditAsync(UpdateCustomerRequestModel model)
        {
            if(HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("LogIn");
            }
            
            if(HttpContext.Request.Method == "POST")
            {
                var result = await _customerService.EditAsync(HttpContext.Session.GetString("Email"), model);
                return StatusCode(201,"Profile Successfully Updated");
            }
            var customer = await _customerService.FindByEmailAsync(HttpContext.Session.GetString("Email"));
            if (customer == null)
            {
                return StatusCode(404, "Customer Not Found");
            }
            return View(customer);
        }
        [ActionName("Profile")]
        public async Task<IActionResult> ProfileAsync()
        {
            if(HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("LogIn");
            }
            var info = await _customerService.VeiwProfileAsync(HttpContext.Session.GetString("Email"));
            return View(info);
        } 
        //  [ActionName("CustomerOrderProfile")]
        //  public async Task<IActionResult> CustomerOrderProfileAsync(string email)
        // {
        //     var info = await _customerService.VeiwProfileAsync(email);
        //     return View(info);
        // }
        

        
    }
}