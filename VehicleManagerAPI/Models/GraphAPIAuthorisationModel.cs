using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VehicleManagerAPI.Models
{
    [NotMapped]
    [Keyless]
    public class GraphAPIAuthorisationModel
    {
        [JsonPropertyName("client_id")]
        public string? ClientID { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("client_secret")]
        public string? ClientSecret { get; set; }

        [JsonPropertyName("grant_type")]
        public string? GrantType { get; set; }
    }
}
