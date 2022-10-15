using System.Text.Json;

namespace Application.Middlewares.ExceptionMiddlewareModels
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Title { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
