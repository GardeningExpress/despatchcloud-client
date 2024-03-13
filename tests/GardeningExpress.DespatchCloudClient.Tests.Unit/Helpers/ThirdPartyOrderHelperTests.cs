using GardeningExpress.DespatchCloudClient.Helpers;
using GardeningExpress.DespatchCloudClient.Model.GoGroopie;
using Newtonsoft.Json;
using NUnit.Framework;
using static GardeningExpress.DespatchCloudClient.Constants;

namespace GardeningExpress.DespatchCloudClient.Tests.Unit.Helpers
{
    public class ThirdPartyOrderServiceTests
    {
        private readonly string thirdPartyOrderJson = "{ \"platform\": \"GoGroopie.com\", \"deal_id\": \"11111\", \"product\": \"Test deal 4 Colours\", \"voucher_code\": \"1112223334\", \"redeem_date\": \"19-10-2019\", \"order_id\": \"1111111111\", \"price_options\": \"Coffee\", \"price\": \"10.99\", \"currency\": \"GBP\", \"full_name\": \"John Doe\", \"email\": \"john.doe@domain.com\", \"phone\": \"+333331111111\", \"house\": \"11\", \"street\": \"test street\", \"city\": \"London\", \"postcode\": \"SE15LB\", \"country_code\": \"GB\", \"sku\": \"01-0111\", \"pipe_deal_id\": \"11055\", \"postage_price\": \"2.99\", \"net_merchant_return\": \"5.99\"  }";

        [Test]
        public void ConvertGoGroopieProductToOrderRequest_ShouldConvertToOrderRequest() 
        {
            // ARRANGE
            var product = JsonConvert.DeserializeObject<Product>(thirdPartyOrderJson);
            var countryName = GlobalisationHelper.GetCountryNameFromIsoTwoCode(product.CountryCode);

            // ACT
            var result = ThirdPartyOrderHelper.ConvertGoGroopieProductToOrderRequest(product);

            // ASSERT
            // Defaults
            Assert.AreEqual((int)OrderStatus.Draft, result.StatusId);
            Assert.AreEqual("0", result.TotalDiscount);
            Assert.AreEqual("0", result.TotalTax);
            Assert.AreEqual(1, result.ManualChannelId);

            // Mappings
            // Pricing and customer
            Assert.AreEqual(product.Currency, result.PaymentCurrency);
            Assert.AreEqual(product.Price, result.TotalPaid);
            Assert.AreEqual(product.Email, result.Email);
            Assert.AreEqual(product.Phone, result.PhoneOne);
            Assert.AreEqual(product.PostagePrice, result.ShippingPaid);
            Assert.AreEqual(product.Platform, result.PaymentMethod);
            Assert.AreEqual(product.VoucherCode, result.PaymentRef); 
            Assert.AreEqual(product.FullName, result.InvoiceName);

            // Address
            Assert.AreEqual(product.FullName, result.ShippingName);
            Assert.AreEqual(product.House, result.ShippingAddressLineOne);
            Assert.AreEqual(product.Street, result.ShippingAddressLineTwo);
            Assert.AreEqual(product.Postcode, result.ShippingAddressPostcode);
            Assert.AreEqual(product.City, result.ShippingAddressCity);
            Assert.AreEqual(product.CountryCode, result.ShippingAddressIso);
            Assert.AreEqual(countryName, result.ShippingAddressCountry);

            Assert.AreEqual(product.FullName, result.InvoiceName);
            Assert.AreEqual(product.House, result.InvoiceAddressLineOne);
            Assert.AreEqual(product.Street, result.InvoiceAddressLineTwo);
            Assert.AreEqual(product.Postcode, result.InvoiceAddressPostcode);
            Assert.AreEqual(product.City, result.InvoiceAddressCity);
            Assert.AreEqual(product.CountryCode, result.InvoiceAddressIso);
            Assert.AreEqual(countryName, result.InvoiceAddressCountry);

            // Notes
            Assert.Contains($"{product.Platform} deal_id: {product.DealId}", result.Notes);
            Assert.Contains($"{product.Platform} voucher_code: {product.VoucherCode}", result.Notes);
            Assert.Contains($"{product.Platform} order_id: {product.OrderId}", result.Notes);
            Assert.Contains($"{product.Platform} pipe_deal_id: {product.PipeDealId}", result.Notes);

        }
    }
}
