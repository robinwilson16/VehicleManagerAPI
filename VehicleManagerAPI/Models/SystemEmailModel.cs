using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleManagerAPI.Models
{
    [NotMapped]
    public class SystemEmailModel
    {
        [Key]
        public int SystemEmailID { get; set; }
        public string? EmailFrom { get; set; }
        public string? EmailFromName { get; set; }
        public string? EmailTo { get; set; }
        public string? EmailToName { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailCC { get; set; }
        public string? EmailBCC { get; set; }
        public string? EmailMessage { get; set; }
        public bool? IsEmailMessageHTML { get; set; }
        public string? EmailKey { get; set; }
        public bool? IsSent { get; set; }
        public string? EmailStatus { get; set; }
    }
}
