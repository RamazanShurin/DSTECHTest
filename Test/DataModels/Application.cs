using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.DataModels
{
    [Index("CreatedDate")]
    [Index("TypeEquipmentId")]
    [Index("Subdivision")]
    public class Application
    {
        [Key]
        public Int64 Id { get; set; }
        public string ApplicantFullName { get; set; }
        public string Subdivision { get; set; }

        public Int64 ApplicationStatusId { get; set; }
        [ForeignKey(nameof(ApplicationStatusId))]
        public ApplicationStatus ApplicationStatus { get; set; }

        public Int64 TypeEquipmentId { get; set; }
        [ForeignKey(nameof(TypeEquipmentId))]
        public TypeEquipment TypeEquipment { get; set; }

        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public string? HandlerComment { get; set; }
    }
}
