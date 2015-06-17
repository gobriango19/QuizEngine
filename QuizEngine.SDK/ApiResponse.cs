using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QuizEngine.Models;

namespace QuizEngine.SDK
{
    public class ApiResponse<T> where T : class, new()
    {
        public bool IsSuccessful { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Model { get; set; }

        public ApiResponse()
        {
            IsSuccessful = false;
            StatusCode = null;
            ErrorMessage = null;
            Model = null;
        }
    }
}
