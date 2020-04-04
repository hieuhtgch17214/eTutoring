using System.ComponentModel.DataAnnotations.Schema;

namespace eTutoring.Models
{
    [Table("TutorAllocations")]
    public class TutorAllocationModel
    {
        public int ID { get; set; }
        public string TutorId { get; set; }

        [Index(IsUnique = true)]
        public string StudentId { get; set; }

        // The actual properties
        public virtual ApplicationUser Tutor { get; set; }
        public virtual ApplicationUser Student { get; set; }
    }
}