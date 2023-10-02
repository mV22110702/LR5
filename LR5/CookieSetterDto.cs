using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace LR5
{
    public class CookieSetterDto
    {
        private static JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static bool ValidateDto(CookieSetterDto? dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("Invalid data!");
            }
            if (dto.Key == String.Empty)
            {
                throw new ArgumentException("Key is empty!");
            }
            if (dto.Value == String.Empty)
            {
                throw new ArgumentException("Value is empty!");
            }
            if (dto.ExpDateTime < DateTime.UtcNow.AddMinutes(-1))
            {
                throw new ArgumentException("DateTime is earlier than now!");
            }
            return true;
        }

        public async static Task<CookieSetterDto?> DeserializeJSON(Stream stream)
        {
            var reader = new StreamReader(stream);
            var jsonPayload = await reader.ReadToEndAsync();
            var dto = JsonSerializer.Deserialize<CookieSetterDto>(jsonPayload, JsonSerializerOptions);
            CookieSetterDto.ValidateDto(dto);
            return dto;
        }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime ExpDateTime { get; set; }
        public CookieSetterDto()
        {
            this.Key = String.Empty;
            this.Value = String.Empty;
            this.ExpDateTime = DateTime.MinValue;
        }
    }
}
