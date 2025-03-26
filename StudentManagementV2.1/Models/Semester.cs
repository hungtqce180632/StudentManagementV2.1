using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Represents an academic semester or term
    /// </summary>
    public class Semester
    {
        /// <summary>
        /// Unique identifier for the semester
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the semester (e.g., Fall 2023, Spring 2024)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Start date of the semester
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the semester
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Indicates if this is the current active semester
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Class sections offered in this semester
        /// </summary>
        public virtual ICollection<ClassSection> ClassSections { get; set; }

        /// <summary>
        /// Constructor to initialize collections
        /// </summary>
        public Semester()
        {
            ClassSections = new HashSet<ClassSection>();
        }
    }
}
