using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Represents a course/subject in the academic curriculum
    /// </summary>
    public class Course
    {
        /// <summary>
        /// Unique identifier for the course
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Course code (e.g., CS101, MATH201)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string CourseCode { get; set; }

        /// <summary>
        /// Name of the course
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Detailed description of the course
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Number of credits for this course
        /// </summary>
        public int Credits { get; set; }

        /// <summary>
        /// Department offering this course
        /// </summary>
        [StringLength(100)]
        public string Department { get; set; }

        /// <summary>
        /// Class sections of this course
        /// </summary>
        public virtual ICollection<ClassSection> ClassSections { get; set; }

        /// <summary>
        /// Constructor to initialize collections
        /// </summary>
        public Course()
        {
            ClassSections = new HashSet<ClassSection>();
        }
    }
}
