using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleManagerAPI.Models
{
    [NotMapped]
    [Keyless]
    public class GraphAPIEmailContentModel
    {
        public string? Subject { get; set; }
        public virtual GraphAPIEmailContentBodyModel? Body { get; set; }
        public virtual ICollection<GraphAPIEmailContentRecipientModel>? ToRecipients { get; set; }
        public virtual ICollection<GraphAPIEmailContentRecipientModel>? CCRecipients { get; set; }
    }
}
