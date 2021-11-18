namespace GardeningExpress.DespatchCloudClient
{
    public class DespatchCloudConfig
    {
        private string _apiBaseUrl;

        public string ApiBaseUrl
        {
            //ensure ends with /
            get
            {
                if (string.IsNullOrEmpty(_apiBaseUrl))
                    return _apiBaseUrl;

                return _apiBaseUrl.EndsWith('/')
                    ? _apiBaseUrl
                    : $"{_apiBaseUrl}/";
            }
            set => _apiBaseUrl = value;
        }

        public string LoginEmailAddress { get; set; }
        public string LoginPassword { get; set; }

        public DespatchCloudEnvironment Environment { get; set; }
    }

    public enum DespatchCloudEnvironment
    {
        Sandbox,
        Live
    }
}