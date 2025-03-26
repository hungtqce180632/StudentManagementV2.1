using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Represents a physical classroom where classes are held
    /// </summary>
    public class Classroom
    {
        /// <summary>
        /// Unique identifier for the classroom
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Room number or name
        /// </summary>
        [Required]
        [StringLength(20)]
        public string RoomNumber { get; set; }

        /// <summary>
        /// Building name where the classroom is located
        /// </summary>
        [StringLength(100)]
        public string Building { get; set; }

        /// <summary>
        /// Floor number
        /// </summary>
        public int? Floor { get; set; }

        /// <summary>
        /// Maximum capacity of the room
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Type of room (e.g., Lecture Hall, Lab, Seminar Room)
        /// </summary>
        [StringLength(50)]
        public string RoomType { get; set; }

        /// <summary>
        /// Additional facilities or equipment available in the room
        /// </summary>
        [StringLength(500)]
        public string Facilities { get; set; }

        /// <summary>
        /// Schedule entries for this classroom
        /// </summary>
        public virtual ICollection<ScheduleEntry> ScheduleEntries { get; set; }

        /// <summary>
        /// Constructor to initialize collections
        /// </summary>
        public Classroom()
        {
            ScheduleEntries = new HashSet<ScheduleEntry>();
        }
    }
}
