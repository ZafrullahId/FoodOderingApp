using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF_Core.Models.Enum;

namespace EF_Core.Models.Entity
{
    public class Customer : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string NextOfKin { get; set; }
        public decimal Wallet { get; set; }
        public Gender Gender { get; set; }
        public List<Order> Order { get; set; }
    }
}