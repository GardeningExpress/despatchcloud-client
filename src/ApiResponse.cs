using GardeningExpress.DespatchCloudClient.DTO;

namespace GardeningExpress.DespatchCloudClient
{
    public abstract class ApiResponse
    {
        protected ApiResponse()
        {
            IsSuccess = true;
        }

        protected ApiResponse(string errorMessage)
        {
            Error = errorMessage;
            IsSuccess = false;
        }

        public bool IsSuccess { get; private set; }

        public string Error { get; set; }
    }

    public class ListResponse<T> : ApiResponse
    {
        public ListResponse(PagedResult<T> pagedResult)
        {
            PagedResult = pagedResult;
        }

        public ListResponse(string errorMessage)
            : base(errorMessage)
        {
        }

        public PagedResult<T> PagedResult { get; set; }
    }

    public class GetResponse<T> : ApiResponse
    {
        public T Result { get; set; }
    }
}