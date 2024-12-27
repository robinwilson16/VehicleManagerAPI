using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleManagerAPI.Models
{
    [NotMapped]
    [Keyless]
    public class ModelResultModel
    {
        public ModelResultAction Action { get; set; }
        public string? Function { get; set; }
        public bool? IsSuccessful { get; set; }
        public string? RecordID { get; set; }
        public int? ReturnCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? StackTrace { get; set; }
    }

    public enum ModelResultAction
    {
        Get = 1,
        Add = 2,
        Update = 3,
        Delete = 4,
    }
}
