namespace EF_Core.Models.DTOs.RequesteModels
{
    public class CreateOrderRequestModel
    {
        public int CustomerId { get; set; }    
        public bool IsDelivered { get;set; }
    }
}