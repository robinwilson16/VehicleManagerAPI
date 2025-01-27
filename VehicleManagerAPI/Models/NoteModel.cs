using System.ComponentModel.DataAnnotations;

namespace VehicleManagerAPI.Models
{
    public class NoteModel
    {
        [Key]
        public int NoteID { get; set; }
        public virtual CustomerModel? Customer { get; set; }
        public virtual VehicleModel? Vehicle { get; set; }
        public string? NoteText { get; set; }
        public bool? IsAlert { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
