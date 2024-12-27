
using CP.VPOS.Models;
using GTBack.Core.Results;
using PaymentService.Core.DTO;

namespace GTBack.Core.Services;

public interface IPaymentService
{
    Task<IDataResults<ProcessResponseDTO>> ProcessPayment(PaymentProcessDTO model);
    Task<IDataResults<BINInstallmentQueryResponse>> GetInstallment(BINInstallmentQueryRequest bin, VirtualPOSAuth pos);
    Task<IDataResults<AllInstallmentQueryResponse>> GetAllInstallment(AllInstallmentQueryRequest bin, VirtualPOSAuth pos);

}