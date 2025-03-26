using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Represents a student's enrollment in a class section
    /// Links students to class sections and stores grade information
    /// </summary>
    public class Enrollment
    {
        /// <summary>
        /// Unique identifier for the enrollment
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date when the student enrolled in the class
        /// </summary>
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Foreign key to Student
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Navigation property to Student
        /// </summary>
        public virtual Student Student { get; set; }

        /// <summary>
        /// Foreign key to ClassSection
        /// </summary>
        public int ClassSectionId { get; set; }

        /// <summary>
        /// Navigation property to ClassSection
        /// </summary>
        public virtual ClassSection ClassSection { get; set; }

        /// <summary>
        /// Final grade for this enrollment (nullable)
        /// </summary>
        public decimal? FinalGrade { get; set; }

        /// <summary>
        /// Indicates if the student has completed the course
        /// </summary>
        public bool IsCompleted { get; set; } = false;
    }
}
