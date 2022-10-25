namespace EF_Core.Models.Dto
{
    public class OrderDto
    {
        public bool IsDelivered { get;set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int PostalCode { get; set; }
        public CustomerDto customerDto {get;set;}
    }
}