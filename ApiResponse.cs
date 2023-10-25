using System.Text.Json;

namespace VolgaIT2023
{
    public class ApiResponse
    {
        public ApiResponse(object? data = null, string message = "Success")
        {
            this.data = data;
            this.message = message;
        }
        public string message { get; set; } = "Success";

        public object? data { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
