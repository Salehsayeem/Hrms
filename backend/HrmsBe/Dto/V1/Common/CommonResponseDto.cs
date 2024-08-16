using System.Text.Json;

namespace HrmsBe.Dto.V1.Common
{
    public class CommonResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = "";
        public bool Succeed { get; set; }
        public dynamic? Data { get; set; } = null;
        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(this, options);
        }
    }
}
