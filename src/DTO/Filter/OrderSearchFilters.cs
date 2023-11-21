using System;
using System.Linq;

namespace GardeningExpress.DespatchCloudClient.DTO.Filter
{
    public class OrderSearchFilters
    {
        public string Search { get; set; }
        public DateRangeFilter DateRange { get; set; }
        public OrderSearchFieldFilters SearchField { get; set; }
        public OrderSearchSort Sort { get; set; }
        public int Page { get; set; } = 1;
        public string Channel { get; set; }

        public string GetQueryString()
        {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("filters[search]", Search);

            if (SearchField != 0)
            {
                var fieldFilters = string.Join(",", Enum.GetValues(typeof(OrderSearchFieldFilters))
                    .Cast<OrderSearchFieldFilters>()
                    .Where(s => SearchField.HasFlag(s)));

                queryString.Add("filters[search_field]", fieldFilters);
            }

            if (DateRange != null)
            {
                var timestamp = string.Join(",", DateRange.StartTimestamp,
                    DateRange.EndTimestamp);

                queryString.Add("filters[date_range]", timestamp);
            }

            if (!string.IsNullOrEmpty(Channel))
            {
                queryString.Add("filters[sales_channel]", Channel);
            }

            if (Sort != 0)
            {
                queryString.Add("sort", Enum.GetName(typeof(OrderSearchSort), Sort));
            }

            queryString.Add("page", Page.ToString());
            return queryString.ToString();
        }
    }

    public enum OrderSearchSort
    {
        name_az = 1,
        name_za,
        totalpaid_hl,
        totalpaid_lh,
        datereceived_pr,
        datereceived_rp,
        datedispatched_pr,
        datedispatched_rp,
        zone_az,
        zone_za
    }

    public enum OrderSearchFieldFilters
    {
        search_sku = 1 << 0,
        search_order_id = 1 << 1,
        search_channel_alt_id = 1 << 2,
        search_name = 1 << 3,
        search_email = 1 << 4,
        search_postcode = 1 << 5,
        search_dc_code = 1 << 6,
        search_country = 1 << 7,
        search_iso = 1 << 8,
        search_shipping = 1 << 9
    }
}