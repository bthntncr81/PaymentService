namespace PaymentService.Core.DTO
{
    public class SubscriptionSaleDTO
    {
        // Payment Information
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CardHolderName { get; set; }
        public string CardCVV { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int Installment { get; set; }
        public string OrderNumber { get; set; }

        // Customer Information
        public string TaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public string IdentityNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressDesc { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string TaxOffice { get; set; }
        public string TownName { get; set; }

        // Payment 3D Information
        public bool Confirm { get; set; }
        public string ReturnURL { get; set; }

        // Customer IP Address (for fraud prevention, etc.)
        public string CustomerIPAddress { get; set; }
        public string BankCode { get; set; }
        public string MerchantID { get; set; }
        public string MerchantUser { get; set; }
        public string MerchantPassword { get; set; }
        public string MerchantStorekey { get; set; }
        public bool TestPlatform { get; set; }


    }
}