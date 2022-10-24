using EF_Core.Models.Dto;
using EF_Core.Models.Enum;
using System;
using EF_Core.Models.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF_Core.Interface.Services;
using EF_Core.Interface.Repositories;
using EF_Core.Models.DTOs.ResponseModels;
using EF_Core.Models.DTOs.RequesteModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace EF_Core.Implimentations.Repositories
{

    public class FoodService : IFoodService
    {
        private readonly IFoodRepo _foodRepo;
        private readonly  IWebHostEnvironment _webHostEnvironment;
        public FoodService(IFoodRepo foodRepo, IWebHostEnvironment webHostEnvironment)
        {
            _foodRepo = foodRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task UpdateFoodStatusAsync()
        {
            var foods = await _foodRepo.ListAsync();
            foreach (var food in foods)
            {
                if (food.AvailableTime <= DateTime.Now && food.Status != Status.NotAvailable)
                {
                    food.Status = Status.Available;
                    var response = await _foodRepo.UpdateAsync(food);
                }
                
            }
        }
        public async Task<BaseResponse> AddAsync(CreateFoodRequestModel model)
        {
           var fd = await _foodRepo.CheckIfExist(model.Type);
           if(fd == true)
           {
                return new BaseResponse
                {
                    Message = "Food Already Exist",
                    Success = false
                };
           }
            var imageName = "";
            if(model.ImageUrl != null)
            {
                var imgPath = _webHostEnvironment.WebRootPath;
                var imagePath  = Path.Combine(imgPath,"Images");
                Directory.CreateDirectory(imagePath);
                var imageType = model.ImageUrl.ContentType.Split('/')[1];
                imageName = $"{Guid.NewGuid()}.{imageType}";
                var fullPath = Path.Combine(imagePath,imageName);
                using(var fileStream = new FileStream(fullPath,FileMode.Create))
                {
                    model.ImageUrl.CopyTo(fileStream);
                }
            }
            var food = new Food
            {
                Type = model.Type,
                Price = model.Price,
                ImageUrl = imageName,
                Status = model.Status,
                Description = model.Description,
                AvailableTime = model.AvailableTime,
                NumberOfPlates = model.NumberOfPlates,
            };
           await _foodRepo.CreateAsync(food);
           return new BaseResponse
           {
            Message = "Food Successfully Added",
            Success = true
           };
        }

        public async Task<BaseResponse> DeleteFoodAsync(int foodId)
        {
           var food = await _foodRepo.DeleteAsync(foodId);
            return new BaseResponse
            {
                Message = "Food Deleted Successfully",
                Success = true
            };
        }

        public async Task<List<FoodResponsModel>> GetAllFoodAsync()
        {
            var foods = await _foodRepo.ListAsync();
            if(foods == null)
            {
                return null;
            }

            return foods.Select(x => new FoodResponsModel
            {
                Message = "These are the foods",
                Success = true,
                Data = new FoodDto
                {
                    Id = x.Id,
                    Type = x.Type,
                    Price = x.Price,
                    Status = x.Status,
                    ImageUrl = x.ImageUrl,
                    Description = x.Description,
                    AvailableTime = x.AvailableTime,
                    NumberOfPlates = x.NumberOfPlates
                }
            }).ToList();
        }

        public async Task<FoodResponsModel> GetByIdAsync(int id)
        {
            var food = await _foodRepo.GetByIdAsync(id);
            if(food == null)
            {
                return new FoodResponsModel
                {
                    Message = "Food Not Found",
                    Success = false
                };
            }

            return new FoodResponsModel
            {
                Message = "Food Found",
                Success = true,
                Data = new FoodDto
                {
                    Id = food.Id,
                    Type = food.Type,
                    Price = food.Price,
                    Status = food.Status,
                    ImageUrl = food.ImageUrl,
                    Description = food.Description,
                    AvailableTime = food.AvailableTime,
                    NumberOfPlates = food.NumberOfPlates
                }
            };
        }

        public async Task<BaseResponse> UpdateFoodAsync(UpdateFoodRequestModel model, int id)
        {
             var food = await _foodRepo.GetByIdAsync(id);
            if (food == null)
            {
                return new BaseResponse
                {
                    Message = "Food not found",
                    Success = false
                };
            }
            
            var imageName = "";
            if(model.ImageUrl != null)
            {
                var imgPath = _webHostEnvironment.WebRootPath;
                var imagePath  = Path.Combine(imgPath,"Images");
                Directory.CreateDirectory(imagePath);
                var imageType = model.ImageUrl.ContentType.Split('/')[1];
                imageName = $"{Guid.NewGuid()}.{imageType}";
                var fullPath = Path.Combine(imagePath,imageName);
                using(var fileStream = new FileStream(fullPath,FileMode.Create))
                {
                    model.ImageUrl.CopyTo(fileStream);
                }
            }
            food.Type = model.Type;
            food.Price = model.Price;
            food.ImageUrl = imageName;
            food.Status = model.Status;
            food.UpdatedAt = DateTime.Now;
            food.AvailableTime = model.AvailableTime;
            food.NumberOfPlates = model.NumberOfPlates;
             await _foodRepo.UpdateAsync(food);
            return new BaseResponse
            {
                Message = "Food Updated Successfully",
                Success = true
            };
        }
    }
}