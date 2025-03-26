using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Represents an assignment given to students in a class
    /// </summary>
    public class Assignment
    {
        /// <summary>
        /// Unique identifier for the assignment
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the assignment
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// Detailed description of the assignment
        /// </summary>
        [StringLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// Date when the assignment was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Due date for the assignment
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Maximum possible points for this assignment
        /// </summary>
        public decimal MaxPoints { get; set; }

        /// <summary>
        /// Weight of this assignment in the overall grade calculation
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Foreign key to ClassSection
        /// </summary>
        public int ClassSectionId { get; set; }

        /// <summary>
        /// Navigation property to ClassSection
        /// </summary>
        public virtual ClassSection ClassSection { get; set; }

        /// <summary>
        /// Collection of submissions for this assignment
        /// </summary>
        public virtual ICollection<AssignmentSubmission> Submissions { get; set; }

        /// <summary>
        /// Constructor to initialize collections
        /// </summary>
        public Assignment()
        {
            Submissions = new HashSet<AssignmentSubmission>();
        }
    }
}
