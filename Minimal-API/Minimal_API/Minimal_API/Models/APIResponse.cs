using System.Net;

namespace Minimal_API.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>();
        }
        public bool IsSuccess { get; set; }
        public object Result { get; set; }  //call result object
        public HttpStatusCode StatusCode { get; set; }  //for status code
        public List<string> ErrorMessages { get; set; } //for error msg


    }
}
