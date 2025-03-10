using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using GTBack.Core.Results;
using GTBack.Core.Services;
using CP.VPOS.Models;

using PaymentService.Core.DTO;
using Iyzipay;
using Iyzipay.Model.V2.Subscription;
using Iyzipay.Request.V2.Subscription;
using Iyzipay.Model.V2;
using Iyzipay.Model;
using CP.VPOS;
using CP.VPOS.Enums;

public class PayService : IPaymentService
{
    // private readonly IService<Payment> _paymentService;
    private readonly IMapper _mapper;

     private readonly string _urlAPITest = "https://sandbox-api.iyzipay.com";
        private readonly string _urlAPILive = "https://api.iyzipay.com";

    public PayService(IMapper mapper)
    {

        _mapper = mapper;
    }


    public async Task<IDataResults<ProcessResponseDTO>> ProcessPayment(PaymentProcessDTO model)
    {
        try
        {
            // Create and configure the VirtualPOSAuth instance
            var virtualPosAuth = new VirtualPOSAuth
            {
                bankCode = model.BankCode,
                merchantID = model.MerchantID,  // Use the appropriate merchant ID
                merchantUser = model.MerchantUser,  // Use the appropriate merchant user
                merchantPassword = model.MerchantPassword,  // Use the appropriate merchant password
                merchantStorekey = model.MerchantStorekey,  // Use the appropriate merchant storekey
                testPlatform = true  // Set true if it's a test environment
            };

            // Create the CustomerInfo object from the PaymentProcessDTO
            var customerInfo = new CustomerInfo
            {
                taxNumber = model.TaxNumber,
                emailAddress = model.EmailAddress,
                name = model.Name,
                surname = model.Surname,
                phoneNumber = model.PhoneNumber,
                addressDesc = model.AddressDesc,
                cityName = model.CityName,
                country = (CP.VPOS.Enums.Country)Enum.Parse(typeof(CP.VPOS.Enums.Country), model.Country),  // Assuming the country is passed in a compatible format
                postCode = model.PostCode,
                taxOffice = model.TaxOffice,
                townName = model.TownName
            };


            // Create the SaleRequest object from the PaymentProcessDTO
            var saleRequest = new SaleRequest
            {
                invoiceInfo = customerInfo,
                shippingInfo = customerInfo,
                saleInfo = new SaleInfo
                {
                    cardNameSurname = model.CardHolderName,
                    cardNumber = model.CardNumber,
                    cardExpiryDateMonth = (short)model.ExpiryMonth,  // Explicit cast to ushort
                    cardExpiryDateYear = (ushort)model.ExpiryYear,  // Explicit cast to ushort
                    amount = model.Amount,
                    cardCVV = model.CardCVV,
                    currency = (CP.VPOS.Enums.Currency)Enum.Parse(typeof(CP.VPOS.Enums.Currency), model.Currency),  // Assuming the currency is passed as a string
                    installment = (sbyte)model.Installment  // Explicit cast to sbyte
                },
                payment3D = new Payment3D
                {
                    confirm = model.Confirm,
                    returnURL = model.ReturnURL
                },
                customerIPAddress = model.CustomerIPAddress,
                orderNumber = Convert.ToInt32((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds).ToString("X")
            };


            // Process the sale request with the configured VirtualPOSAuth instance
            var response = VPOSClient.Sale(saleRequest, virtualPosAuth);

            // Check the response and return an appropriate result
            if (response.statu == CP.VPOS.Enums.SaleResponseStatu.Success || response.statu == CP.VPOS.Enums.SaleResponseStatu.RedirectHTML)
            {
                var res = new ProcessResponseDTO
                {
                    ConversationId = model.OrderNumber,
                    Content = response.message,
                };
                return new SuccessDataResult<ProcessResponseDTO>(res);
            }
            else
            {

                var res = new ProcessResponseDTO
                {
                    ConversationId = model.OrderNumber,
                    Content = response.message,
                };
                return new ErrorDataResults<ProcessResponseDTO>(res);
            }
        }
        catch (Exception ex)
        {
            var res = new ProcessResponseDTO
            {
                ConversationId = "",
                Content = "Error",
            };
            return new ErrorDataResults<ProcessResponseDTO>(res);
        }
    }
    public async Task<IDataResults<BINInstallmentQueryResponse>> GetInstallment(BINInstallmentQueryRequest bin, VirtualPOSAuth pos)
    {




        BINInstallmentQueryResponse vPOSService = VPOSClient.BINInstallmentQuery(bin, pos);

        return new SuccessDataResult<BINInstallmentQueryResponse>(vPOSService);
    }

    public async Task<IDataResults<AllInstallmentQueryResponse>> GetAllInstallment(AllInstallmentQueryRequest bin, VirtualPOSAuth pos)
    {




        AllInstallmentQueryResponse vPOSService = VPOSClient.AllInstallmentQuery(bin, pos);

        return new SuccessDataResult<AllInstallmentQueryResponse>(vPOSService);
    }


    public  async  Task<IDataResults< ResponseData<ProductResource>>> AddSubscription(CreateProductRequest model,Options pos)
    {
     
    
        // 3. API Çağrısını Yap
    var response = await Task.Run(() => Product.Create(model, pos));
        // 4. Sonucu Konsola Yazdır
        Console.WriteLine(response);


        return new SuccessDataResult<ResponseData<ProductResource>>(response);
    }


 public async Task<IDataResults<ProcessResponseDTO>> ProcessSubsPayment(PaymentProcessDTO model)
    {
        try
        {
            // Create and configure the VirtualPOSAuth instance
            var virtualPosAuth = new VirtualPOSAuth
            {
                bankCode = model.BankCode,
                merchantID = model.MerchantID,  // Use the appropriate merchant ID
                merchantUser = model.MerchantUser,  // Use the appropriate merchant user
                merchantPassword = model.MerchantPassword,  // Use the appropriate merchant password
                merchantStorekey = model.MerchantStorekey,  // Use the appropriate merchant storekey
                testPlatform = true  // Set true if it's a test environment
            };

            // Create the CustomerInfo object from the PaymentProcessDTO
            var customerInfo = new CustomerInfo
            {
                taxNumber = model.TaxNumber,
                emailAddress = model.EmailAddress,
                name = model.Name,
                surname = model.Surname,
                phoneNumber = model.PhoneNumber,
                addressDesc = model.AddressDesc,
                cityName = model.CityName,
                country = (CP.VPOS.Enums.Country)Enum.Parse(typeof(CP.VPOS.Enums.Country), model.Country),  // Assuming the country is passed in a compatible format
                postCode = model.PostCode,
                taxOffice = model.TaxOffice,
                townName = model.TownName
            };


            // Create the SaleRequest object from the PaymentProcessDTO
            var saleRequest = new SaleRequest
            {
                invoiceInfo = customerInfo,
                shippingInfo = customerInfo,
                saleInfo = new SaleInfo
                {
                    cardNameSurname = model.CardHolderName,
                    cardNumber = model.CardNumber,
                    cardExpiryDateMonth = (short)model.ExpiryMonth,  // Explicit cast to ushort
                    cardExpiryDateYear = (ushort)model.ExpiryYear,  // Explicit cast to ushort
                    amount = model.Amount,
                    cardCVV = model.CardCVV,
                    currency = (CP.VPOS.Enums.Currency)Enum.Parse(typeof(CP.VPOS.Enums.Currency), model.Currency),  // Assuming the currency is passed as a string
                    installment = (sbyte)model.Installment  // Explicit cast to sbyte
                },
                payment3D = new Payment3D
                {
                    confirm = model.Confirm,
                    returnURL = model.ReturnURL
                },
                customerIPAddress = model.CustomerIPAddress,
                orderNumber = Convert.ToInt32((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds).ToString("X")
            };


            // Process the sale request with the configured VirtualPOSAuth instance
            var response = VPOSClient.Sale(saleRequest, virtualPosAuth);

            // Check the response and return an appropriate result
            if (response.statu == CP.VPOS.Enums.SaleResponseStatu.Success || response.statu == CP.VPOS.Enums.SaleResponseStatu.RedirectHTML)
            {
                var res = new ProcessResponseDTO
                {
                    ConversationId = model.OrderNumber,
                    Content = response.message,
                };
                return new SuccessDataResult<ProcessResponseDTO>(res);
            }
            else
            {

                var res = new ProcessResponseDTO
                {
                    ConversationId = model.OrderNumber,
                    Content = response.message,
                };
                return new ErrorDataResults<ProcessResponseDTO>(res);
            }
        }
        catch (Exception ex)
        {
            var res = new ProcessResponseDTO
            {
                ConversationId = "",
                Content = "Error",
            };
            return new ErrorDataResults<ProcessResponseDTO>(res);
        }
    }

 public async Task<IDataResults<SaleResponse>> SaleSubscription(SubscriptionSaleDTO request)
{

     var virtualPosAuth = new VirtualPOSAuth
            {
                bankCode = request.BankCode,
                merchantID = request.MerchantID,  // Use the appropriate merchant ID
                merchantUser = request.MerchantUser,  // Use the appropriate merchant user
                merchantPassword = request.MerchantPassword,  // Use the appropriate merchant password
                merchantStorekey = request.MerchantStorekey,  // Use the appropriate merchant storekey
                testPlatform = true  // Set true if it's a test environment
            };
    // Subscription request objesi oluşturuluyor
    SubscriptionInitializeRequest req = new SubscriptionInitializeRequest()
    {
        Locale = Locale.TR.ToString(),
        PricingPlanReferenceCode = "64993c4b-7862-402d-8c97-61a939ed39d7", // Plan ID
        SubscriptionInitialStatus = "ACTIVE", // Başlangıç durumu: aktif
        Customer = new CheckoutFormCustomer()
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.EmailAddress,
            GsmNumber = request.PhoneNumber,
            IdentityNumber = request.IdentityNumber, // Kimlik numarası
            BillingAddress = new Address()
            {
                Description = request.AddressDesc,  // Açıklama
                ZipCode = request.PostCode,
                Country =request.Country,
                City = request.CityName,
                ContactName = request.Name  // İletişim kişisi
            },
            ShippingAddress = new Address()
            {
                 Description = request.AddressDesc,  // Açıklama
                ZipCode = request.PostCode,
                Country =request.Country,
                City = request.CityName,
                ContactName = request.Name  // İletişim kişisi
            }
        },
        PaymentCard = new CardInfo()
        {
            CardHolderName = request.CardHolderName,
            CardNumber = request.CardNumber, // Kart numarasını buraya yazın
            ExpireYear = request.ExpiryYear.ToString(), // Son kullanma yılı
            ExpireMonth = request.ExpiryMonth.ToString(), // Son kullanma ayı
            Cvc = request.CardCVV, // CVC kodu
            RegisterConsumerCard = true, // Kartı saklama isteği
        }
    };

    // Set up the options for Iyzipay API
    Options options = GetOptions(virtualPosAuth); // Assuming GetOptions() is implemented elsewhere

    // Call Subscription API to initialize subscription
    ResponseData<SubscriptionCreatedResource> response = Subscription.Initialize(req, options);

    // Create SaleResponse object based on response
    SaleResponse saleResponse = new SaleResponse()
    {
        privateResponse = new Dictionary<string, object>()
    };

    // Check if the response is successful based on the 'statu' field
    if (response.Status == "success")
    {
        Console.WriteLine("Subscription created successfully!");
        Console.WriteLine("Subscription ID: " + response.Data.ReferenceCode);

        // Assuming SaleResponseStatu is an enum and you have a Success status
        saleResponse.statu = SaleResponseStatu.Success;
        saleResponse.message = "Subscription created successfully!";
        saleResponse.orderNumber = response.Data.ReferenceCode; // Assuming this is the order number
        saleResponse.transactionId = response.Data.ReferenceCode; // Add transactionId as needed
    }
    else
    {
        Console.WriteLine("Subscription creation failed: " + response.ErrorMessage);

        saleResponse.statu = SaleResponseStatu.Error;
        saleResponse.message = "Subscription creation failed: " + response.ErrorMessage;
    }

    // Optionally, add private responses to the dictionary
    saleResponse.privateResponse.Add("responseDetails", response);

    // Return the SaleResponse object
    return new SuccessDataResult<SaleResponse>(saleResponse);
}
        private Options GetOptions(VirtualPOSAuth auth)
        {
            Options options = new Options();

            options.ApiKey = auth.merchantUser;
            options.SecretKey = auth.merchantPassword;
            options.BaseUrl = auth.testPlatform ? _urlAPITest : _urlAPILive;

            return options;
        }


}