using System.Net;

namespace SproomInbox.API.Utils.ErrorHandling
{
    public class Status <T>     
    {
        public T? _entity { get; private set; }
        public bool Success { get; protected set; }
        public HttpStatusCode StatusCode { get; protected set; }
        public string Message { get; protected set; }

        public Status(T entity)
        {
            Success = true;
            _entity = entity;
            StatusCode = HttpStatusCode.OK;
            Message = string.Empty;
        }

        public Status(HttpStatusCode errorCode, string errorMessage) 
        {
            Success = false;
            _entity = default(T);
            StatusCode = errorCode;
            Message = errorMessage;
        }   
    }
}
