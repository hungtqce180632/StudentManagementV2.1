using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Represents a specific section/instance of a course offering
    /// Links a course to a teacher, semester, and enrolled students
    /// </summary>
    public class ClassSection
    {
        /// <summary>
        /// Unique identifier for the class section
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Section name/number (e.g., Section 1, Section A)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string SectionName { get; set; }

        /// <summary>
        /// Maximum capacity of students for this section
        /// </summary>
        public int MaxCapacity { get; set; }

        /// <summary>
        /// Current number of enrolled students
        /// </summary>
        public int CurrentEnrollment { get; set; }

        /// <summary>
        /// Foreign key to Course
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// Navigation property to Course
        /// </summary>
        public virtual Course Course { get; set; }

        /// <summary>
        /// Foreign key to Teacher
        /// </summary>
        public int TeacherId { get; set; }

        /// <summary>
        /// Navigation property to Teacher
        /// </summary>
        public virtual Teacher Teacher { get; set; }

        /// <summary>
        /// Foreign key to Semester
        /// </summary>
        public int SemesterId { get; set; }

        /// <summary>
        /// Navigation property to Semester
        /// </summary>
        public virtual Semester Semester { get; set; }

        /// <summary>
        /// Collection of student enrollments in this class section
        /// </summary>
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        /// <summary>
        /// Collection of schedule entries for this class section
        /// </summary>
        public virtual ICollection<ScheduleEntry> ScheduleEntries { get; set; }

        /// <summary>
        /// Collection of assignments for this class section
        /// </summary>
        public virtual ICollection<Assignment> Assignments { get; set; }

        /// <summary>
        /// Collection of attendance records for this class section
        /// </summary>
        public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; }

        /// <summary>
        /// Collection of announcements for this class section
        /// </summary>
        public virtual ICollection<Announcement> Announcements { get; set; }

        /// <summary>
        /// Constructor to initialize collections
        /// </summary>
        public ClassSection()
        {
            Enrollments = new HashSet<Enrollment>();
            ScheduleEntries = new HashSet<ScheduleEntry>();
            Assignments = new HashSet<Assignment>();
            AttendanceRecords = new HashSet<AttendanceRecord>();
            Announcements = new HashSet<Announcement>();
        }
    }
}
