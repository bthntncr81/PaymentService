using CP.VPOS;
using CP.VPOS.Models;
using GTBack.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PaymentService.Core.DTO;
using Server.Hubs;

namespace GTBack.WebAPI.Controllers.Ecommerce
{
    public class PaymentController : CustomEcomController
    {
        private readonly IPaymentService _paymentService;
        private readonly IHubContext<PayHub> _hubContext;

        public PaymentController(IPaymentService paymentService, IHubContext<PayHub> hubContext)
        {
            _paymentService = paymentService;
            _hubContext = hubContext;
        }

        // Handle 3D Payment Response
        [HttpPost("VirtualPOS3DResponse")]
        public async Task<IActionResult> VirtualPOS3DResponse(PaymentProcessDTO model)
        {

            var response = await _paymentService.ProcessPayment(model);

            return ApiResult(response);
        }

        [HttpPost("PaymentCallBack")]
        public async Task<IActionResult> PaymentCallBack([FromQuery] string bankCode, string merchantID, string merchantUser, string merchantPassword, string merchantStorekey, string orderNumber)
        {
            // Create and configure the VirtualPOSAuth instance
            var virtualPosAuth = new VirtualPOSAuth
            {
                bankCode = bankCode,
                merchantID = merchantID,  // Use the appropriate merchant ID
                merchantUser = merchantUser,  // Use the appropriate merchant user
                merchantPassword = merchantPassword,  // Use the appropriate merchant password
                merchantStorekey = merchantStorekey,  // Use the appropriate merchant storekey
                testPlatform = true  // Set true if it's a test environment
            };



            Dictionary<string, object>? pairs = null;

            if (Request.Method == "GET")
                pairs = Request.Query.Keys.ToDictionary(k => k, v => (object)Request.Query[v]);
            else
                pairs = Request.Form.Keys.ToDictionary(k => k, v => (object)Request.Form[v]);

            SaleResponse response = VPOSClient.Sale3DResponse(new Sale3DResponseRequest
            {
                responseArray = pairs
            }, virtualPosAuth);


            try
            {
                await _hubContext.Clients.Client(PayHub.TransactionConnections[orderNumber])
                                       .SendAsync("Receive", response);
            }
            catch (Exception ex)
            {
                // Log the exception

                // Return a meaningful error response or rethrow the exception
                return StatusCode(500, "An error occurred while processing the callback.");
            }

            return Ok();

        }


        [HttpPost("GetInstallment")]

        public async Task<IActionResult> GetInstallment(BINInstallmentQueryRequest model, [FromQuery] string bankCode, string merchantID, string merchantUser, string merchantPassword, string merchantStorekey, string orderNumber)
        {


            // Create and configure the VirtualPOSAuth instance
            var virtualPosAuth = new VirtualPOSAuth
            {
                bankCode = bankCode,
                merchantID = merchantID,  // Use the appropriate merchant ID
                merchantUser = merchantUser,  // Use the appropriate merchant user
                merchantPassword = merchantPassword,  // Use the appropriate merchant password
                merchantStorekey = merchantStorekey,  // Use the appropriate merchant storekey
                testPlatform = true  // Set true if it's a test environment
            };
            var response = await _paymentService.GetInstallment(model, virtualPosAuth);

            return ApiResult(response);

        }




        [HttpPost("GetAllInstallment")]

        public async Task<IActionResult> GetAllInstallment(AllInstallmentQueryRequest model, [FromQuery] string bankCode, string merchantID, string merchantUser, string merchantPassword, string merchantStorekey, string orderNumber)
        {


            // Create and configure the VirtualPOSAuth instance
            var virtualPosAuth = new VirtualPOSAuth
            {
                bankCode = bankCode,
                merchantID = merchantID,  // Use the appropriate merchant ID
                merchantUser = merchantUser,  // Use the appropriate merchant user
                merchantPassword = merchantPassword,  // Use the appropriate merchant password
                merchantStorekey = merchantStorekey,  // Use the appropriate merchant storekey
                testPlatform = false  // Set true if it's a test environment
            };
            var response = await _paymentService.GetAllInstallment(model, virtualPosAuth);

            return ApiResult(response);

        }

    }
}