using System;

namespace GardeningExpress.DespatchCloudClient.Auth
{
    public class ApiAuthenticationException : Exception
    {
        public ApiAuthenticationException(string message)
            : base(message)
        {
        }

        public ApiAuthenticationException(Exception innerException)
            : base("Error authenticating with DespatchCloud", innerException)
        {
        }
    }
}