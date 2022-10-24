using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF_Core.Models.Enum;
using Microsoft.AspNetCore.Http;

namespace EF_Core.Models.Dto
{
    public class FoodDto
    {
        public int Id {get;set;}
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int NumberOfPlates { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public DateTime AvailableTime { get;set; }
    }
}