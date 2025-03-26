using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Represents a teacher in the system, inherits from User base class
    /// Contains teacher-specific properties and relationships
    /// </summary>
    public class Teacher : User
    {
        /// <summary>
        /// Teacher ID/code (different from system ID)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string TeacherCode { get; set; }

        /// <summary>
        /// Department the teacher belongs to
        /// </summary>
        [StringLength(100)]
        public string Department { get; set; }

        /// <summary>
        /// Teacher's qualification or highest degree
        /// </summary>
        [StringLength(100)]
        public string Qualification { get; set; }

        /// <summary>
        /// Teacher's contact phone number
        /// </summary>
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Date when the teacher joined the institution
        /// </summary>
        public DateTime JoinDate { get; set; }

        /// <summary>
        /// Classes taught by this teacher
        /// </summary>
        public virtual ICollection<ClassSection> ClassesTaught { get; set; }

        /// <summary>
        /// Constructor to initialize collections and set role
        /// </summary>
        public Teacher()
        {
            Role = "Teacher";
            ClassesTaught = new HashSet<ClassSection>();
        }
    }
}
