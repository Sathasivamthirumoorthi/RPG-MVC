using System.Text.Json.Serialization;

namespace web.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RpgClass
    {
        Knight = 1,
        Mage = 2,
        cleric = 3
    }
}