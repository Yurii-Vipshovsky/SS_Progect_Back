namespace SoftServe_BackEnd.Services
{
    public class PagedResponse<T>: Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            Message = string.Empty;
            Succeeded = true;
            Errors = null;
        }
    }
}