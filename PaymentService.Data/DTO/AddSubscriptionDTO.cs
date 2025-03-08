using Iyzipay;
using Iyzipay.Request.V2.Subscription;

namespace PaymentService.Core.DTO
{
    public class AddSubscriptionDTO
    {
        // Payment Information
        public CreateProductRequest Model { get; set; }
        public Options Pos { get; set; }
      

    }
}