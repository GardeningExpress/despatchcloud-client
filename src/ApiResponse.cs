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
        public ListResponse(PagedResult<T> pagedData)
        {
            PagedData = pagedData;
        }

        public ListResponse(string errorMessage)
            : base(errorMessage)
        {
        }

        public PagedResult<T> PagedData { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse(string errorMessage)
            : base(errorMessage)
        { }

        public ApiResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}