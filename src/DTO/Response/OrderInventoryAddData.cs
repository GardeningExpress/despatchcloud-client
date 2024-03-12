using Newtonsoft.Json;


namespace GardeningExpress.DespatchCloudClient.DTO.Response
{
    public class OrderInventoryAddData
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
