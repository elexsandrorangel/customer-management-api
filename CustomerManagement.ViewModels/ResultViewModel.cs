using System.Text.Json.Serialization;
using System.Text.Json;

namespace CustomerManagement.ViewModels
{
    public class ResultViewModel
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("exceptionType")]
        public string? ExceptionType { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, options: new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }
    }
}
