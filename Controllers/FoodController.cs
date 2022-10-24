using EF_Core.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using EF_Core.Interface.Services;
using EF_Core.Models.DTOs.RequesteModels;

namespace MVC_Project.Controllers
{
    public class FoodController : Controller
    {
        private readonly IFoodService _foodService;
        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }
        [ActionName("AddFood")]
        public async Task<IActionResult> AddFoodAsync(CreateFoodRequestModel model)
        {
             if(HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AdminLogin","Staff");
            }
            if(HttpContext.Request.Method == "POST")
            {
              var food = await _foodService.AddAsync(model);
                if(food.Success == true)
                {
                 return StatusCode(201,food.Message);
                }
                 return StatusCode(406,food.Message);
            }
            return View();
        }
        [ActionName("Foods")]
        public async Task<IActionResult> FoodsAsync()
        {
             if(HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AdminLogin","Staff");
            }
           await _foodService.UpdateFoodStatusAsync();
            return View(await _foodService.GetAllFoodAsync());
        }
        [ActionName("DeleteFood")]
          public async Task<IActionResult> DeleteFoodAsync(int id)
        {
             if(HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AdminLogin","Staff");
            }
            if(HttpContext.Request.Method == "POST")
            {                
                var ans = await _foodService.DeleteFoodAsync(id);
                if(ans.Success == true)
                {
                    return StatusCode(201,ans.Message);
                }
                return StatusCode(406,"Food not Deleted");
            }
            return View(await _foodService.GetByIdAsync(id));
        }
        [ActionName("EditFood")]
        public async Task<IActionResult> EditFoodAsync(UpdateFoodRequestModel model, int id)
        {
             if(HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AdminLogin","Staff");
            }
            if(HttpContext.Request.Method == "POST")
            {                
                var ans = await _foodService.UpdateFoodAsync(model,id);
                if(ans.Success == true)
                {
                    return StatusCode(201,ans.Message);
                }
                return StatusCode(406,ans.Message);
            }
            var result = await _foodService.GetByIdAsync(id);
            if(result.Success == false)
            {
                return Content(result.Message);
            }
            return View(result);
        }
    }
}