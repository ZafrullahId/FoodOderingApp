using System;
using EF_Core.Models.Enum;

namespace EF_Core.Models.DTOs.RequesteModels
{
    public class UpdateCustomerRequestModel : UpdateUserRequestModels
    {
        public string NextOfKin { get; set; }
        public decimal Wallet { get;set; }
        public DateTime DathOfBirth { get; set; }
        public Gender Gender { get; set; }
    }
}