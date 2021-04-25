namespace SoftServe_BackEnd.Services
{
    public class Response<T>
    {
        public string Message { get; set; }
        public string[] Errors { get; set; }
        public bool Succeeded { get; set; }
        public T Data { get; set; }

        public Response()
        {
            
        }

        public Response(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Data = data;
            Errors = null;
        }
    }
}