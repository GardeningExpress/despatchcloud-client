using System;
using System.Collections.Generic;
using System.Linq;

namespace GardeningExpress.DespatchCloudClient.DTO
{
    public class InventorySearchFilters
    {
        public string Search { get; set; }
        public string SKU { get; set; }
        public IEnumerable<InventorySearchProductType> ProductTypes { get; set; }
        public IEnumerable<int> LocationIds { get; set; }
        public int Page { get; set; } = 1;
        public InventorySearchSort? Sort { get; set; }

        public string GetQueryString()
        {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrWhiteSpace(SKU))
                queryString.Add("filters[sku]", SKU);

            if (!string.IsNullOrWhiteSpace(Search))
                queryString.Add("filters[search]", Search);

            if (ProductTypes != null && ProductTypes.Any())
            {
                if (ProductTypes.Count() == 1)
                    queryString.Add("filters[product_type]", ProductTypes.ToString());
                else
                    queryString.Add("filters[product_types]",
                        string.Concat(ProductTypes.Select(x => x.ToString()), ','));
            }

            if (LocationIds != null && LocationIds.Any())
            {
                if (LocationIds.Count() == 1)
                    queryString.Add("filters[location]", LocationIds.ToString());
                else
                    queryString.Add("filters[location]",
                        string.Concat(LocationIds.Select(x => x.ToString()), ','));
            }

            queryString.Add("page", Page.ToString());

            if (Sort.HasValue)
            {
                queryString.Add("sort", Enum.GetName(typeof(OrderSearchSort), Sort));
            }

            return queryString.ToString();
        }
    }

    public enum InventorySearchProductType
    {
        Product = 1,
        Group = 2,
        Component = 3
    }

    public enum InventorySearchSort
    {
        name_az,
        name_za,
        sku_az,
        sku_za,
        stock_level_available_hl,
        stock_level_available_lh,
        stock_level_open_hl,
        stock_level_open_lh,
        stockwarn_hl,
        stockwarn_lh,
        syncstock_yn,
        syncstock_ny,
        productweight_hl,
        productweight_lh,
        updated_at_pr,
        updated_at_rp
    }
}