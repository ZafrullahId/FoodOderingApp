using EF_Core.Models.Dto;

namespace EF_Core.Models.DTOs.ResponseModels
{
    public class CustomerResponsModel : BaseResponse
    {
        public CustomerDto Data {get;set;}
    }
}