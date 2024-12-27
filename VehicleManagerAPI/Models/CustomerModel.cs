using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleManagerAPI.Models
{
    [NotMapped]
    public class CustomerModel
    {
        [Key]
        public string? CustomerEmail { get; set; }
        public virtual ICollection<VehicleModel>? Vehicles { get; set; }

        [ForeignKey("To")]
        public virtual ICollection<MessageModel>? ReceivedMessages { get; set; }

        [ForeignKey("From")]
        public virtual ICollection<MessageModel>? SentMessages { get; set; }
        public virtual ICollection<NoteModel>? Notes { get; set; }
    }
}
