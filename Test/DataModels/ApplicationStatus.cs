using System.ComponentModel.DataAnnotations;

namespace Test.DataModels
{
    public class ApplicationStatus
    {
        [Key]
        public Int64 Id { get; set; }
        public string Name { get; set; }
    }
}
