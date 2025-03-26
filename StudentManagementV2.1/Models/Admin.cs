using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Represents an administrator in the system, inherits from User base class
    /// Contains admin-specific properties
    /// </summary>
    public class Admin : User
    {
        /// <summary>
        /// Administrative position or title
        /// </summary>
        [StringLength(100)]
        public string Position { get; set; }

        /// <summary>
        /// Admin's office location
        /// </summary>
        [StringLength(100)]
        public string OfficeLocation { get; set; }

        /// <summary>
        /// Admin's contact phone number
        /// </summary>
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Constructor to set the role
        /// </summary>
        public Admin()
        {
            Role = "Admin";
        }
    }
}
