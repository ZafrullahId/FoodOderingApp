namespace EF_Core.Models.Dto
{
    public class OrderDto
    {
        public bool IsDelivered { get;set; }
        public CustomerDto customerDto {get;set;}
    }
}