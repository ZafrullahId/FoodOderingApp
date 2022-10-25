namespace EF_Core.Models.DTOs.RequesteModels
{
    public class CreateOrderRequestModel
    {
        public int CustomerId { get; set; }    
        public bool IsDelivered { get;set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int PostalCode { get; set; }
    }
}