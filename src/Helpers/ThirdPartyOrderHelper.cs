using GardeningExpress.DespatchCloudClient.DTO.Request;
using GardeningExpress.DespatchCloudClient.Model.GoGroopie;
using System.Collections.Generic;
using static GardeningExpress.DespatchCloudClient.Constants;

namespace GardeningExpress.DespatchCloudClient.Helpers
{
    public static class ThirdPartyOrderHelper
    {
        /// <summary>
        /// Map GoGroopieProduct product to CreateOrder request for DespatchCloud
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static OrderCreateRequest ConvertGoGroopieProductToOrderRequest(Product request)
        {
            var countryFullName = GlobalisationHelper.GetCountryNameFromIsoTwoCode(request.CountryCode);
            var orderCreate = new OrderCreateRequest()
            {
                // Defaults
                StatusId = OrderStatus.Draft, // TODO Chad should new orders default as draft?
                TotalDiscount = "0",
                TotalTax = "0",
                ManualChannelId = 1,

                Email = request.Email,
                ShippingPaid = request.PostagePrice,
                TotalPaid = request.Price,
                PaymentCurrency = request.Currency,
                PaymentMethod = request.Platform, // TODO Chad confirm payment type as it is required
                PaymentRef = request.VoucherCode, // TODO Chad should the payment ref be voucher code
                PhoneOne = request.Phone,

                // TODO Chad what should shipping method be set as?
                ShippingName = request.FullName,
                ShippingAddressLineOne = request.House,
                ShippingAddressLineTwo = request.Street,
                ShippingAddressCity = request.City,
                ShippingAddressPostcode = request.Postcode,
                ShippingAddressIso = request.CountryCode,
                    
                InvoiceName = request.FullName,
                InvoiceAddressLineOne = request.House,
                InvoiceAddressLineTwo = request.Street,
                InvoiceAddressCity = request.City,
                InvoiceAddressPostcode = request.Postcode,
                InvoiceAddressIso = request.CountryCode,

                // Data not mapped to fields added to notes for reference
                Notes = new List<string>()
                {
                    $"{request.Platform} deal_id: {request.DealId}",
                    $"{request.Platform} voucher_code: {request.VoucherCode}",
                    $"{request.Platform} redeem_date: {request.RedeemDate}",
                    $"{request.Platform} order_id: {request.OrderId}",
                    $"{request.Platform} pipe_deal_id: {request.PipeDealId}",
                    $"{request.Platform} net_merchant_return: {request.NetMerchantReturn}",
                }

            };

            if (countryFullName != null )
            {

                orderCreate.ShippingAddressCountry = countryFullName;
                orderCreate.InvoiceAddressCountry = countryFullName;
            }

            return orderCreate;
        }
    }
}
