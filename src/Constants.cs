namespace GardeningExpress.DespatchCloudClient
{
    public static class Constants
    {
        public static class OrderStatus
        {
            public const int Draft = 1;
            public const int OnHold = 2;
            public const int DespatchReady = 3;
            public const int Picking = 4;
            public const int Despatched = 5;
            public const int Cancelled = 6;
            public const int Packing = 8;
            public const int Picked = 10;
        }
    }
}