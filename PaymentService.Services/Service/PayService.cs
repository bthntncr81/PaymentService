using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using CP.VPOS;
using CP.VPOS.Models;
using GTBack.Core.Results;
using GTBack.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Core.DTO;

public class PayService : IPaymentService
{
    // private readonly IService<Payment> _paymentService;
    private readonly IMapper _mapper;

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





}