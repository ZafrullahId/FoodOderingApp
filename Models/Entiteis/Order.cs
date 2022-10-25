using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 namespace EF_Core.Models.Entity
 {
    public class Order : BaseEntity
    {
        public int CustomerId { get; set; }    
        public bool IsDelivered { get;set; }
        public Customer Customer { get; set; }
        public Address Address {get;set;}
        public List<FoodOrder> FoodOrders { get;set; }
    }
 }