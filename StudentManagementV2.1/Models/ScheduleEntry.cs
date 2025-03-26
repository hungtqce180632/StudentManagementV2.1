using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Represents a scheduled class meeting (time and location)
    /// Links class sections to classrooms at specific time slots
    /// </summary>
    public class ScheduleEntry
    {
        /// <summary>
        /// Unique identifier for the schedule entry
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Day of the week (e.g., Monday, Tuesday)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string DayOfWeek { get; set; }

        /// <summary>
        /// Foreign key to ClassSection
        /// </summary>
        public int ClassSectionId { get; set; }

        /// <summary>
        /// Navigation property to ClassSection
        /// </summary>
        public virtual ClassSection ClassSection { get; set; }

        /// <summary>
        /// Foreign key to TimeSlot
        /// </summary>
        public int TimeSlotId { get; set; }

        /// <summary>
        /// Navigation property to TimeSlot
        /// </summary>
        public virtual TimeSlot TimeSlot { get; set; }

        /// <summary>
        /// Foreign key to Classroom
        /// </summary>
        public int ClassroomId { get; set; }

        /// <summary>
        /// Navigation property to Classroom
        /// </summary>
        public virtual Classroom Classroom { get; set; }
    }
}
