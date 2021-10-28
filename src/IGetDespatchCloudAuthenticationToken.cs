using System.Threading.Tasks;

namespace GardeningExpress.DespatchCloudClient
{
    public interface IGetDespatchCloudAuthenticationToken
    {
        Task<string> GetTokenAsync();
    }
}