using EF_Core.Models.Dto;

namespace EF_Core.Models.DTOs.ResponseModels
{
    public class OrderResponseModel : BaseResponse
    {
        public OrderDto Data {get;set;}
    }
}