using GardeningExpress.DespatchCloudClient.DTO.Request;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration.Utils
{
    public class TestUtils
    {
        #region TestData

        public static OrderInventoryAddRequest GetOrderInventoryAddRequest() => new OrderInventoryAddRequest
        {
            Items = new List<OrderInventoryItem> {
                new OrderInventoryItem
                {
                    InventoryId = "3333",
                    Name = "MACBOOKAIR",
                    SKU = "MACBOOKAIR",
                    Quantity = 1,
                    UnitPrice = 1000.22M,
                    LineTotalDiscount = 2,
                    Options = "Colour: Pink",
                    Notes = "TEst 1",
                    HsCode = "code33",
                    CountryOfOrigin = "Japan",
                    ImageUrl = "https://media-exp1.licdn.com/dms/image/C4D0BAQFenp-2o4YZpA/company-logo_200_200/0/1594636780827?e=2147483647&v=beta&t=_UDcVrneIwMgxwvV3x8pEJifJSxkTW52B0bbln2Q86I"
                }
            }
        };

        public static OrderCreateRequest GetCreateOrderRequest() => new OrderCreateRequest
        {
            StatusId = 1,
            ShippingMethodRequested = "Test",
            PaymentMethod = "CC",
            PaymentRef = "CC-123",
            PaymentCurrency = "GBP",
            TotalPaid = "100",
            TotalDiscount = "0",
            TotalTax = "0",
            TotalWeight = "100",
            Email = "demo@despatchcloud.com",
            PhoneOne = "+905559998877",
            VatNumber = "V1",
            EoriNumber = "E1",
            TaxId = "T1",
            ShippingName = "Test Customer 2",
            ShippingAddressLineOne = "SAL 1",
            ShippingAddressLineTwo = "SAL 2",
            ShippingAddressCity = "Izmir",
            ShippingAddressCounty = "Ege",
            ShippingAddressCountry = "Turkey",
            ShippingAddressPostcode = "35090",
            ShippingAddressIso = "TR",
            InvoiceName = "Integration Tests:",
            InvoiceAddressLineOne = "IAL 1",
            InvoiceAddressLineTwo = "IAL 2",
            InvoiceAddressCity = "Bursa",
            InvoiceAddressCounty = "Marmara",
            InvoiceAddressCountry = "Turkey",
            InvoiceAddressPostcode = "16300",
            InvoiceAddressIso = "TR",
            CustomerComments = "Test 1 Comment",
            Notes = new List<string> {
                    "Test 1 Note"
                },
            CustomFields = new Dictionary<string, string>() {
                    { "test1-1","Test Data" }
                },
            FulfilmentClientId = null,
            ShippingPaid = "2.00",
            ManualChannelId = 1
        };
        
        #endregion

        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            if (obj == null || propertyName == null)
            {
                throw new ArgumentNullException(nameof(obj) + " or " + nameof(propertyName) + " or " + nameof(value));
            }

            // Get the type of the object
            Type type = obj.GetType();

            // Find the property by name
            PropertyInfo propertyInfo = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{type.Name}'.");
            }

            // Check if the property is writable
            if (!propertyInfo.CanWrite)
            {
                throw new ArgumentException($"Property '{propertyName}' on type '{type.Name}' is read-only.");
            }

            // Set the property value
            propertyInfo.SetValue(obj, value);
        }

    }
}
