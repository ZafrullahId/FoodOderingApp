using System;
using EF_Core.Models.Enum;

namespace EF_Core.Models.DTOs.RequesteModels
{
    public class CreateCustomerRequestModel : CreateUserRequestModel
    {
        public string NextOfKin { get; set; }
        public Gender Gender { get; set; }
    }
}