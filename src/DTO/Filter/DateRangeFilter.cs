using System;

namespace GardeningExpress.DespatchCloudClient.DTO.Filter
{
    public class DateRangeFilter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        internal long StartTimestamp => ConvertToTimestamp(StartDate);
        internal long EndTimestamp => ConvertToTimestamp(EndDate);

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static long ConvertToTimestamp(DateTime value)
        {
            var elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }
    }
}