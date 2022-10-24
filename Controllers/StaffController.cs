using EF_Core.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EF_Core.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using EF_Core.Interface.Services;
using EF_Core.Models.DTOs.RequesteModels;

namespace MVC_Project.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly IFoodService _foodService;
        public StaffController(IStaffService staffService,IFoodService foodService)
        {
            _staffService = staffService;
            _foodService = foodService;
        }
        [ActionName("Index")]
         public async Task<IActionResult> IndexAsync()
        {
             if(HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AdminLogin");
            }
            return View(await _staffService.GetAllAsync());
        }
        [ActionName("AddStaff")]
        public async Task<IActionResult> AddStaffAsync(CreateStaffRequestModel model)
        {
             if(HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AdminLogin");
            }
             if(HttpContext.Request.Method == "POST")
            {
                var result = await _staffService.RegisterAsync(model);
                if (result.Success == true)
                {
                    return Content("Staff Added Successfully");
                }
                return StatusCode(406,"Registration Failed. Email may exist already");
            }
            return View();
        }
        [ActionName("AdminLogin")]
        public async Task<IActionResult> AdminLoginAsync(string email,string password)
        {
             if(HttpContext.Request.Method == "POST")
            {
                var result = await _staffService.FindAsync(email,password);
                if (result.Success == true)
                {
                    HttpContext.Session.SetString("Role", result.Data.Role.ToString());
                    return RedirectToAction("Index");
                }
                return StatusCode(406,"Login Failed");
            }
            return View();
        }
        [ActionName("EditStaff")]
        public async Task<IActionResult> EditStaffAsync(int id, UpdateStaffRequestModel model)
        {
             if(HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AdminLogin");
            }
            if(HttpContext.Request.Method == "POST")
            {
                var result = await _staffService.EditAsync(id,model);
                if(result.Success == true)
                {
                     return StatusCode(201,result.Message);
                }
                return Content("Something went wrong");
            }
            var staff = await _staffService.FindByIdAsync(id);
            if (staff == null)
            {
                return StatusCode(404, "Staff Not Found");
            }
            return View(staff);
        }
         [ActionName("DeleteStaff")]
         public async Task<IActionResult> DeleteStaffAsync(int id)
        {
             if(HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AdminLogin");
            }
            if(HttpContext.Request.Method == "POST")
            {                
                var ans = await _staffService.DeleteAsync(id);
                if(ans.Success == true)
                {
                    return StatusCode(201,"Staff Account Successfully Deleted");
                }
                    return StatusCode(406,"Staff not Deleted");
            }
            return View();
        }
        [ActionName("StaffProfile")]
         public async Task<IActionResult> StaffProfileAsync(int id)
        {
             if(HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("AdminLogin");
            }
            var info = await _staffService.VeiwProfileAsync(id);
            return View(info);
        } 
        // public IActionResult AddFood(CreateFoodDto createFoodDto)
        // {
        //      if(HttpContext.Session.GetString("Role") != "Admin")
        //     {
        //         return RedirectToAction("AdminLogin");
        //     }
        //     if(HttpContext.Request.Method == "POST")
        //     {
        //       var food = _foodService.Add(createFoodDto);
        //          if(food)
        //         {
        //          return StatusCode(201,"Food Successfully Added");
        //         }
        //          return StatusCode(406,"Food not Added. Food May Exist Alreay");
        //     }
        //     return View();
        // }

        // public IActionResult Foods()
        // {
        //      if(HttpContext.Session.GetString("Role") != "Admin")
        //     {
        //         return RedirectToAction("AdminLogin");
        //     }
        //     _foodService.UpdateFoodStatus();
        //     return View(_foodService.GetAllFood());
        // }

        //   public IActionResult DeleteFood(int id)
        // {
        //      if(HttpContext.Session.GetString("Role") != "Admin")
        //     {
        //         return RedirectToAction("AdminLogin");
        //     }
        //     if(HttpContext.Request.Method == "POST")
        //     {                
        //         var ans = _foodService.DeleteFood(id);
        //         if(ans)
        //         {
        //             return StatusCode(201,"Food Successfully Deleted");
        //         }
        //             return StatusCode(406,"Food not Deleted");
        //     }
        //     return View(_foodService.GetById(id));
        // }

        // public IActionResult EditFood(UpdateFoodDto updateFoodDto, int id)
        // {
        //      if(HttpContext.Session.GetString("Role") != "Admin")
        //     {
        //         return RedirectToAction("AdminLogin");
        //     }
        //     if(HttpContext.Request.Method == "POST")
        //     {                
        //         var ans = _foodService.UpdateFood(updateFoodDto,id);
        //         if(ans)
        //         {
        //             return StatusCode(201,"Food Successfully Updated");
        //         }
        //         return StatusCode(406,"Food not Updated");
        //     }
        //     var result = _foodService.GetById(id);
        //     if(result == null)
        //     {
        //         return Content("Food Not Found");
        //     }
        //     return View(result);
        // }
        
        // public IActionResult Orders()
        // {
        //     if(HttpContext.Session.GetString("Role") != "Admin")
        //     {
        //         return RedirectToAction("AdminLogin");
        //     }
        //     return View(_staffService.Orders());
        // }       
    }
}