using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StudentManagementV2._1.Data
{
    /// <summary>
    /// Factory để tạo đối tượng AppDbContext cho migrations và runtime
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        /// <summary>
        /// Tạo một instance mới của AppDbContext
        /// </summary>
        /// <param name="args">Các đối số dòng lệnh</param>
        /// <returns>AppDbContext đã được cấu hình</returns>
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            
            // Cấu hình kết nối SQL Server với tài khoản "sa" và mật khẩu "123"
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=StudentManagementDb;User Id=sa;Password=123;TrustServerCertificate=True;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
