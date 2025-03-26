using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Represents a time slot in the academic schedule
    /// Defines start and end times for class periods
    /// </summary>
    public class TimeSlot
    {
        /// <summary>
        /// Unique identifier for the time slot
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the time slot (e.g., "Period 1", "Morning Session")
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Start time of the slot
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// End time of the slot
        /// </summary>
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Optional description for the time slot
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// Schedule entries using this time slot
        /// </summary>
        public virtual ICollection<ScheduleEntry> ScheduleEntries { get; set; }

        /// <summary>
        /// Constructor to initialize collections
        /// </summary>
        public TimeSlot()
        {
            ScheduleEntries = new HashSet<ScheduleEntry>();
        }
    }
}
