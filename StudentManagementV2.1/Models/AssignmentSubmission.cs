using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Represents a student's submission for an assignment
    /// </summary>
    public class AssignmentSubmission
    {
        /// <summary>
        /// Unique identifier for the submission
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date and time when the submission was made
        /// </summary>
        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Content or notes for the submission
        /// </summary>
        [StringLength(4000)]
        public string Content { get; set; }

        /// <summary>
        /// File path or reference to uploaded file (if any)
        /// </summary>
        [StringLength(500)]
        public string FilePath { get; set; }

        /// <summary>
        /// Score given to this submission
        /// </summary>
        public decimal? Score { get; set; }

        /// <summary>
        /// Teacher's feedback on the submission
        /// </summary>
        [StringLength(1000)]
        public string Feedback { get; set; }

        /// <summary>
        /// Date when the submission was graded
        /// </summary>
        public DateTime? GradedDate { get; set; }

        /// <summary>
        /// Flag indicating if the submission is late
        /// </summary>
        public bool IsLate { get; set; }

        /// <summary>
        /// Foreign key to Student
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Navigation property to Student
        /// </summary>
        public virtual Student Student { get; set; }

        /// <summary>
        /// Foreign key to Assignment
        /// </summary>
        public int AssignmentId { get; set; }

        /// <summary>
        /// Navigation property to Assignment
        /// </summary>
        public virtual Assignment Assignment { get; set; }
    }
}
