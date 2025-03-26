using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Represents an announcement for students and teachers
    /// Can be general (for everyone) or specific to a class section
    /// </summary>
    public class Announcement
    {
        /// <summary>
        /// Unique identifier for the announcement
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the announcement
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// Content of the announcement
        /// </summary>
        [Required]
        [StringLength(4000)]
        public string Content { get; set; }

        /// <summary>
        /// Date when the announcement was posted
        /// </summary>
        public DateTime PostedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Optional expiration date for the announcement
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Foreign key to User (who posted the announcement)
        /// </summary>
        public int PostedById { get; set; }

        /// <summary>
        /// Navigation property to User
        /// </summary>
        public virtual User PostedBy { get; set; }

        /// <summary>
        /// Null for general announcements, set for class-specific announcements
        /// </summary>
        public int? ClassSectionId { get; set; }

        /// <summary>
        /// Navigation property to ClassSection (null for general announcements)
        /// </summary>
        public virtual ClassSection ClassSection { get; set; }

        /// <summary>
        /// Target audience for the announcement (e.g., "All", "Students", "Teachers")
        /// </summary>
        [Required]
        [StringLength(20)]
        public string TargetAudience { get; set; } = "All";

        /// <summary>
        /// Indicates if this is an important/highlighted announcement
        /// </summary>
        public bool IsImportant { get; set; } = false;
    }
}
