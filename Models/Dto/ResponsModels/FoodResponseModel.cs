using EF_Core.Models.Dto;

namespace EF_Core.Models.DTOs.ResponseModels
{
    public class FoodResponsModel : BaseResponse
    {
        public FoodDto Data {get;set;}
    }
}