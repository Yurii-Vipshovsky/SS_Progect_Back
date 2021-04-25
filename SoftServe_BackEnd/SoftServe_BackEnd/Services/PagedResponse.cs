namespace SoftServe_BackEnd.Services
{
    public class PagedResponse<T>: Response<T>
    {
        public int PageNumber;
        public int PageSize;
        public int TotalPages;
        public int TotalRecords;

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