using System;
using System.Collections.Generic;
using System.Linq;

namespace GardeningExpress.DespatchCloudClient.DTO
{
    public class InventorySearchFilters
    {
        public string SKU { get; set; }
        public int Page { get; set; } = 1;
        public InventorySearchSort? Sort { get; set; }

        public string GetQueryString()
        {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrWhiteSpace(SKU))
                queryString.Add("filters[sku]", SKU);

            queryString.Add("page", Page.ToString());

            if (Sort.HasValue)
            {
                queryString.Add("sort", Enum.GetName(typeof(OrderSearchSort), Sort));
            }

            return queryString.ToString();
        }
    }
}