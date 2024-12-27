using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace VehicleManagerAPI.Models
{
    [Keyless]
    [Index(nameof(ConfigID), IsUnique = true)]
    public class ConfigModel
    {
        [MaxLength(50)]
        public required string ConfigID { get; set; }

        [MaxLength(50)]
        public string? Value { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
