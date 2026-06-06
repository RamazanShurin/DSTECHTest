using System.ComponentModel.DataAnnotations;

namespace Test.DataModels
{
    public class TypeEquipment
    {
        [Key]
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
