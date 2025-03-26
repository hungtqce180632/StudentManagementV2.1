using Microsoft.EntityFrameworkCore;
using StudentManagementV2._1.Models;
using System;

namespace StudentManagementV2._1.Data
{
    /// <summary>
    /// DbContext của Entity Framework Core cho ứng dụng
    /// Quản lý kết nối cơ sở dữ liệu và cấu hình mô hình
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Người dùng trong hệ thống (lớp cơ sở)
        /// </summary>
        public DbSet<User> Users { get; set; }
        
        /// <summary>
        /// Người dùng quản trị viên
        /// </summary>
        public DbSet<Admin> Admins { get; set; }
        
        /// <summary>
        /// Người dùng giáo viên
        /// </summary>
        public DbSet<Teacher> Teachers { get; set; }
        
        /// <summary>
        /// Người dùng học sinh
        /// </summary>
        public DbSet<Student> Students { get; set; }
        
        /// <summary>
        /// Các khóa học/môn học
        /// </summary>
        public DbSet<Course> Courses { get; set; }
        
        /// <summary>
        /// Các lớp học
        /// </summary>
        public DbSet<ClassSection> ClassSections { get; set; }
        
        /// <summary>
        /// Các học kỳ
        /// </summary>
        public DbSet<Semester> Semesters { get; set; }
        
        /// <summary>
        /// Constructor với các tùy chọn kết nối
        /// </summary>
        /// <param name="options">Tùy chọn DbContext</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Ghi đè phương thức này để thêm cấu hình SQL Server với tài khoản sa
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Kết nối SQL Server với tài khoản "sa" và mật khẩu "123"
                optionsBuilder.UseSqlServer(
                    "Server=localhost;Database=StudentManagementDb;User Id=sa;Password=123;TrustServerCertificate=True;");
            }
            
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Cấu hình mô hình và các mối quan hệ
        /// </summary>
        /// <param name="modelBuilder">Model builder cho Fluent API</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình kế thừa table-per-hierarchy (TPH) cho các loại User
            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasDiscriminator<string>("Role")
                .HasValue<Admin>("Admin")
                .HasValue<Teacher>("Teacher")
                .HasValue<Student>("Student");

            // Cấu hình ràng buộc username là duy nhất
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Cấu hình ràng buộc cho Course
            modelBuilder.Entity<Course>()
                .HasIndex(c => c.CourseCode)
                .IsUnique();

            // Cấu hình kiểu dữ liệu cho các thuộc tính decimal
            // Assignment - MaxPoints: Điểm tối đa cho bài tập (ví dụ: 100.00)
            modelBuilder.Entity<Assignment>()
                .Property(a => a.MaxPoints)
                .HasColumnType("decimal(10, 2)");

            // Assignment - Weight: Trọng số của bài tập trong tính điểm (ví dụ: 0.25 tương đương 25%)
            modelBuilder.Entity<Assignment>()
                .Property(a => a.Weight)
                .HasColumnType("decimal(5, 2)");

            // AssignmentSubmission - Score: Điểm số cho bài nộp (ví dụ: 85.50)
            modelBuilder.Entity<AssignmentSubmission>()
                .Property(a => a.Score)
                .HasColumnType("decimal(10, 2)");

            // Enrollment - FinalGrade: Điểm cuối cùng cho môn học (ví dụ: 8.75)
            modelBuilder.Entity<Enrollment>()
                .Property(e => e.FinalGrade)
                .HasColumnType("decimal(5, 2)");

            // Cấu hình mối quan hệ cho ClassSection
            modelBuilder.Entity<ClassSection>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.ClassSections)
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassSection>()
                .HasOne(cs => cs.Teacher)
                .WithMany(t => t.ClassesTaught)
                .HasForeignKey(cs => cs.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassSection>()
                .HasOne(cs => cs.Semester)
                .WithMany(s => s.ClassSections)
                .HasForeignKey(cs => cs.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Kiểm tra xem đã có bất kỳ dữ liệu nào trong bảng Users
            // Nếu không có, tiến hành thêm dữ liệu mẫu
            try
            {
                // Seed dữ liệu ban đầu cho tài khoản admin
                var adminId = 1;
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin123"); // Trong ứng dụng thực tế, sử dụng tiện ích mã hóa mật khẩu phù hợp

                modelBuilder.Entity<Admin>().HasData(
                    new Admin
                    {
                        Id = adminId,
                        Username = "admin",
                        PasswordHash = hashedPassword,
                        FirstName = "System",
                        LastName = "Administrator",
                        Email = "admin@school.edu",
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        Position = "Quản trị viên hệ thống",
                        OfficeLocation = "Văn phòng chính",
                        PhoneNumber = "123-456-7890"
                    }
                );
            }
            catch
            {
                // Bỏ qua lỗi khi thêm dữ liệu mẫu
                // Điều này có thể xảy ra nếu dữ liệu đã tồn tại
            }
        }
    }
}
