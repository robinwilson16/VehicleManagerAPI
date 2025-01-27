using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleManagerAPI.Models
{
    public class MessageModel
    {
        [Key]
        [DisplayName("ID")]
        public int MessageID { get; set; }

        [DisplayName("Unique ID")]
        public Guid? MessageGUID { get; set; }

        [DisplayName("Type")]
        public MessageType? MessageTypeID { get; set; }

        [DisplayName("Template")]
        public MessageTemplateModel? MessageTemplate { get; set; }
        public string? Subject { get; set; }
        public string? SubjectProcessed { get; set; }
        public string? Message { get; set; }
        public string? MessageProcessed { get; set; }
        public bool? MessageIsHTML { get; set; }
        public bool? IncludeAcceptRejectLinks { get; set; }
        public int? VehicleID { get; set; }
        public virtual VehicleModel? Vehicle { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        [DataType(DataType.Currency)]
        public decimal? AmountOffered { get; set; }
        public bool? IsAccepted { get; set; }
        public string? From { get; set; }
        public string? FromName { get; set; }
        public string? To { get; set; }
        public string? ToName { get; set; }
        public string? CC { get; set; }
        public string? BCC { get; set; }
        public bool? IsSent { get; set; }
        public DateTime? SentDate { get; set; }
        public MessageStatus? MessageStatusID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }

    public enum MessageType
    {
        ToCustomer = 1,
        ToCompany = 2
    }

    public enum MessageStatus
    {
        Pending = 0,
        Sent = 1,
        Failed = 2
    }
}
