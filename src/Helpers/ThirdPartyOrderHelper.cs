using GardeningExpress.DespatchCloudClient.DTO.Request;

namespace GardeningExpress.DespatchCloudClient.Helpers
{
    public static class ThirdPartyOrderHelper
    {
        /// <summary>
        /// Map Third party order to CreateOrder request for DespatchCloud
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static OrderCreateRequest ConvertThirdPartyOrderToOrderRequest(ThirdPartyOrderCreateRequest request)
        {
            return new OrderCreateRequest()
            {
                StatusId = 1,
                Email = request.Email,

                ShippingPaid = request.PostagePrice,
                DateReceived = request.RedeemDate,
                TotalPaid = request.Price,
                PaymentCurrency = request.Currency,

                ShippingAddressLineOne = request.House,
                ShippingAddressLineTwo = request.Street,
                ShippingAddressCity = request.City,
                ShippingAddressPostcode = request.Postcode,
                ShippingAddressIso = request.CountryCode,

                InvoiceAddressLineOne = request.House,
                InvoiceAddressLineTwo = request.Street,
                InvoiceAddressCity = request.City,
                InvoiceAddressPostcode = request.Postcode,
                InvoiceAddressIso = request.CountryCode,


            };
        }
    }
}
