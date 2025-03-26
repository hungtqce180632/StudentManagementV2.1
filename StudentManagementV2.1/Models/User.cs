using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementV2._1.Models
{
    /// <summary>
    /// Mô hình người dùng cơ sở đại diện cho người dùng trong hệ thống
    /// Chứa các thuộc tính chung cho tất cả các loại người dùng (Admin, Giáo viên, Học sinh)
    /// </summary>
    public class User
    {
        /// <summary>
        /// Định danh duy nhất cho người dùng
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên đăng nhập cho mục đích đăng nhập, phải là duy nhất
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        /// <summary>
        /// Mật khẩu đã được mã hóa để bảo mật
        /// Mật khẩu không bao giờ được lưu trữ dưới dạng văn bản thuần túy
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Tên của người dùng
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Họ của người dùng
        /// </summary>
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Địa chỉ email của người dùng
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Chỉ ra liệu tài khoản có đang hoạt động hay không
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Ngày và giờ khi người dùng được tạo
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Vai trò của người dùng trong hệ thống (Admin, Giáo viên, Học sinh)
        /// </summary>
        [Required]
        public string Role { get; set; }

        /// <summary>
        /// Lấy họ tên đầy đủ của người dùng (FirstName + LastName)
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";
    }
}
