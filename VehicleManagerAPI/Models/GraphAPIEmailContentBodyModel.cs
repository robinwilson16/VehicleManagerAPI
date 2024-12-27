using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleManagerAPI.Models
{
    [NotMapped]
    [Keyless]
    public class GraphAPIEmailContentBodyModel
    {
        public string? ContentType { get; set; }
        public string? Content { get; set; }
    }
}
