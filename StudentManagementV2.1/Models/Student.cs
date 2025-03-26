using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Đại diện cho một học sinh trong hệ thống, kế thừa từ lớp cơ sở User
    /// Chứa các thuộc tính và mối quan hệ đặc thù cho học sinh
    /// </summary>
    public class Student : User
    {
        /// <summary>
        /// Mã số học sinh (khác với ID hệ thống)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string StudentCode { get; set; }

        /// <summary>
        /// Ngày sinh của học sinh
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Địa chỉ hiện tại của học sinh
        /// </summary>
        [StringLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// Số điện thoại của học sinh
        /// </summary>
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Năm học hiện tại của học sinh
        /// </summary>
        public int? CurrentYear { get; set; }

        /// <summary>
        /// Danh sách các lớp học mà học sinh đã đăng ký
        /// </summary>
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        /// <summary>
        /// Constructor để khởi tạo các collection
        /// </summary>
        public Student()
        {
            Role = "Student";
            Enrollments = new HashSet<Enrollment>();
        }
    }
}
