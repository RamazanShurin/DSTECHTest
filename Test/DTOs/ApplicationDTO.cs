using System.ComponentModel.DataAnnotations.Schema;
using Test.DataModels;

namespace Test.DTOs
{
    public class ApplicationDTO
    {
        public Int64 Id { get; set; }
        public string ApplicantFullName { get; set; }
        public string ApplicationNumber { get; set; }
        public string Subdivision { get; set; }

        public Int64 ApplicationStatusId { get; set; }
        public string ApplicationStatus { get; set; }

        public Int64 TypeEquipmentId { get; set; }
        public string TypeEquipment { get; set; }

        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? HandlerComment { get; set; }
    }
}
