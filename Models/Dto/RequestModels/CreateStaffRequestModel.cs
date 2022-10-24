using System;
using EF_Core.Models.Enum;

namespace EF_Core.Models.DTOs.RequesteModels
{
    public class CreateStaffRequestModel : CreateUserRequestModel
    {
        public string NextOfKin { get; set; }
        public DateTime DathOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Role Role { get; set; }
    }
}