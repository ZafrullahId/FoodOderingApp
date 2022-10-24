using System;
using EF_Core.Models.Enum;
using Microsoft.AspNetCore.Http;

namespace EF_Core.Models.DTOs.RequesteModels
{
    public class CreateFoodRequestModel
    {
         public string Type { get; set; }
        public decimal Price { get; set; }
        public int NumberOfPlates { get; set; }
        public IFormFile ImageUrl { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public DateTime AvailableTime { get;set; }
    }
}