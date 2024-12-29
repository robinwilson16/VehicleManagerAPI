using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VehicleManagerAPI.Models
{
    public class MessageTemplateModel
    {
        [Key]
        public int MessageTemplateID { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? TemplateContent { get; set; }
        public int? Sequence { get; set; }
        public bool? IsEnabled { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        [JsonIgnore]
        public ICollection<MessageModel>? Messages { get; set; }
    }
}
