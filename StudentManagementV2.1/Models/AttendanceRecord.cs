using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Represents attendance records for a class session
    /// </summary>
    public class AttendanceRecord
    {
        /// <summary>
        /// Unique identifier for the attendance record
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date of the class session
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Foreign key to ClassSection
        /// </summary>
        public int ClassSectionId { get; set; }

        /// <summary>
        /// Navigation property to ClassSection
        /// </summary>
        public virtual ClassSection ClassSection { get; set; }

        /// <summary>
        /// Notes about this class session
        /// </summary>
        [StringLength(500)]
        public string Notes { get; set; }

        /// <summary>
        /// Individual student attendance entries for this session
        /// </summary>
        public virtual ICollection<StudentAttendance> StudentAttendances { get; set; }

        /// <summary>
        /// Constructor to initialize collections
        /// </summary>
        public AttendanceRecord()
        {
            StudentAttendances = new HashSet<StudentAttendance>();
        }
    }

    /// <summary>
    /// Represents an individual student's attendance status for a class session
    /// </summary>
    public class StudentAttendance
    {
        /// <summary>
        /// Unique identifier for the student attendance
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Attendance status (Present, Absent, Late, Excused)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        /// <summary>
        /// Optional comment about the student's attendance
        /// </summary>
        [StringLength(200)]
        public string Comment { get; set; }

        /// <summary>
        /// Foreign key to Student
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Navigation property to Student
        /// </summary>
        public virtual Student Student { get; set; }

        /// <summary>
        /// Foreign key to AttendanceRecord
        /// </summary>
        public int AttendanceRecordId { get; set; }

        /// <summary>
        /// Navigation property to AttendanceRecord
        /// </summary>
        public virtual AttendanceRecord AttendanceRecord { get; set; }
    }
}
