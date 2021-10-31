using System.Threading.Tasks;

namespace GardeningExpress.DespatchCloudClient.Auth
{
    public interface IGetDespatchCloudAuthenticationToken
    {
        Task<string> GetTokenAsync();
    }
}