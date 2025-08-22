namespace Template.API.Models.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse()
        {
            Success = true;
        }

        public ApiResponse(T data, string? message = null)
        {
            Success = true;
            Data = data;
            Message = message;
        }

        public ApiResponse(string error)
        {
            Success = false;
            Errors = new List<string> { error };
        }

        public ApiResponse(List<string> errors)
        {
            Success = false;
            Errors = errors;
        }
    }

    public class ApiResponse : ApiResponse<object>
    {
        public ApiResponse() : base() { }
        public ApiResponse(string message) : base()
        {
            Message = message;
        }
        public ApiResponse(string error, bool isError) : base(error)
        {
            if (!isError)
            {
                Success = true;
                Message = error;
                Errors = null;
            }
        }
        public ApiResponse(List<string> errors) : base(errors) { }
    }
}
