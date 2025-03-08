
using CP.VPOS.Models;
using GTBack.Core.Results;
using Iyzipay;
using Iyzipay.Model.V2;
using Iyzipay.Model.V2.Subscription;
using Iyzipay.Request.V2.Subscription;
using PaymentService.Core.DTO;

namespace GTBack.Core.Services;

public interface IPaymentService
{
    Task<IDataResults<ProcessResponseDTO>> ProcessPayment(PaymentProcessDTO model);
    Task<IDataResults<BINInstallmentQueryResponse>> GetInstallment(BINInstallmentQueryRequest bin, VirtualPOSAuth pos);
    Task<IDataResults<AllInstallmentQueryResponse>> GetAllInstallment(AllInstallmentQueryRequest bin, VirtualPOSAuth pos);

    Task<IDataResults< ResponseData<ProductResource>>> AddSubscription(CreateProductRequest model,Options pos);

    Task<IDataResults<SaleResponse>> SaleSubscription(SubscriptionSaleDTO request);

}