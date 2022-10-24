using EF_Core.Models.Dto;

namespace EF_Core.Models.DTOs.ResponseModels
{
    public class StaffResponsModel : BaseResponse
    {
        public StaffDto Data {get;set;}
    }
}