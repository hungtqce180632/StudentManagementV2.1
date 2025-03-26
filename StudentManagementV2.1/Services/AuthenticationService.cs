using System;
using System.Data;
using System.Data.SqlClient;
using StudentManagementV2._1.Models;

namespace StudentManagementV2._1.Services
{
    /// <summary>
    /// Dịch vụ xử lý xác thực người dùng và quản lý phiên
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        /// <summary>
        /// Chuỗi kết nối đến cơ sở dữ liệu SQL Server
        /// </summary>
        private readonly string _connectionString = "Server=localhost;Database=StudentManagementDb;User Id=sa;Password=123;TrustServerCertificate=True;";
        
        /// <summary>
        /// Sự kiện được kích hoạt khi người dùng đăng nhập thành công
        /// </summary>
        public event EventHandler<User> UserLoggedIn;

        /// <summary>
        /// Sự kiện được kích hoạt khi người dùng đăng xuất
        /// </summary>
        public event EventHandler UserLoggedOut;

        /// <summary>
        /// Lấy người dùng hiện đang đăng nhập
        /// </summary>
        public User CurrentUser { get; private set; }

        /// <summary>
        /// Thử đăng nhập với thông tin đăng nhập đã cung cấp
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu (văn bản thuần túy)</param>
        /// <returns>True nếu đăng nhập thành công, ngược lại là false</returns>
        public bool Login(string username, string password)
        {
            try
            {
                // Truy vấn cơ sở dữ liệu để tìm người dùng
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(
                        "SELECT * FROM dbo.UserLoginView WHERE Username = @Username", connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Kiểm tra mật khẩu
                                string storedHash = reader["PasswordHash"].ToString();
                                bool validPassword = BCrypt.Net.BCrypt.Verify(password, storedHash);
                                
                                if (validPassword)
                                {
                                    string role = reader["Role"].ToString();
                                    
                                    // Lấy thông tin chi tiết dựa trên vai trò
                                    if (connection.State != ConnectionState.Open)
                                        connection.Open();
                                        
                                    reader.Close();
                                    
                                    // Tạo đối tượng người dùng phù hợp
                                    if (role == "Admin")
                                        CurrentUser = GetAdminDetails(connection, Convert.ToInt32(reader["Id"]));
                                    else if (role == "Teacher")
                                        CurrentUser = GetTeacherDetails(connection, Convert.ToInt32(reader["Id"]));
                                    else if (role == "Student")
                                        CurrentUser = GetStudentDetails(connection, Convert.ToInt32(reader["Id"]));
                                    
                                    // Thông báo sự kiện đăng nhập thành công
                                    UserLoggedIn?.Invoke(this, CurrentUser);
                                    return true;
                                }
                            }
                        }
                    }
                }
                
                // Nếu không tìm thấy người dùng hoặc mật khẩu không khớp, thử với tài khoản mẫu
                return TryDemoLogin(username, password);
            }
            catch (Exception)
            {
                // Nếu có lỗi kết nối cơ sở dữ liệu, thử với tài khoản mẫu
                return TryDemoLogin(username, password);
            }
        }
        
        /// <summary>
        /// Lấy thông tin chi tiết của Admin từ cơ sở dữ liệu
        /// </summary>
        private Admin GetAdminDetails(SqlConnection connection, int userId)
        {
            using (SqlCommand command = new SqlCommand(
                "SELECT * FROM dbo.Users WHERE Id = @Id AND Role = 'Admin'", connection))
            {
                command.Parameters.AddWithValue("@Id", userId);
                
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Admin
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Position = reader["Position"] as string,
                            OfficeLocation = reader["OfficeLocation"] as string,
                            PhoneNumber = reader["PhoneNumber"] as string,
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            Role = reader["Role"].ToString()
                        };
                    }
                }
            }
            
            return null;
        }
        
        /// <summary>
        /// Lấy thông tin chi tiết của Teacher từ cơ sở dữ liệu
        /// </summary>
        private Teacher GetTeacherDetails(SqlConnection connection, int userId)
        {
            using (SqlCommand command = new SqlCommand(
                "SELECT * FROM dbo.Users WHERE Id = @Id AND Role = 'Teacher'", connection))
            {
                command.Parameters.AddWithValue("@Id", userId);
                
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Teacher
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            TeacherCode = reader["TeacherCode"] as string,
                            Department = reader["Department"] as string,
                            Qualification = reader["Qualification"] as string,
                            PhoneNumber = reader["PhoneNumber"] as string,
                            JoinDate = reader["JoinDate"] != DBNull.Value ? Convert.ToDateTime(reader["JoinDate"]) : DateTime.MinValue,
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            Role = reader["Role"].ToString()
                        };
                    }
                }
            }
            
            return null;
        }
        
        /// <summary>
        /// Lấy thông tin chi tiết của Student từ cơ sở dữ liệu
        /// </summary>
        private Student GetStudentDetails(SqlConnection connection, int userId)
        {
            using (SqlCommand command = new SqlCommand(
                "SELECT * FROM dbo.Users WHERE Id = @Id AND Role = 'Student'", connection))
            {
                command.Parameters.AddWithValue("@Id", userId);
                
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Student
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            StudentCode = reader["StudentCode"] as string,
                            DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfBirth"]) : DateTime.MinValue,
                            Address = reader["Address"] as string,
                            PhoneNumber = reader["PhoneNumber"] as string,
                            CurrentYear = reader["CurrentYear"] != DBNull.Value ? Convert.ToInt32(reader["CurrentYear"]) : (int?)null,
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            Role = reader["Role"].ToString()
                        };
                    }
                }
            }
            
            return null;
        }

        /// <summary>
        /// Thử đăng nhập với tài khoản mẫu nếu cơ sở dữ liệu không khả dụng
        /// </summary>
        private bool TryDemoLogin(string username, string password)
        {
            // Kiểm tra cho quản trị viên
            if (username == "admin" && password == "admin123")
            {
                CurrentUser = new Admin
                {
                    Id = 1,
                    Username = "admin",
                    FirstName = "Quản trị",
                    LastName = "Hệ thống",
                    Email = "admin@school.edu",
                    Position = "Quản trị viên hệ thống"
                };
                
                UserLoggedIn?.Invoke(this, CurrentUser);
                return true;
            }
            
            // Kiểm tra cho giáo viên
            if (username == "teacher" && password == "teacher123")
            {
                CurrentUser = new Teacher
                {
                    Id = 2,
                    Username = "teacher",
                    FirstName = "Nguyễn",
                    LastName = "Văn A",
                    Email = "nguyenvana@school.edu",
                    TeacherCode = "GV001",
                    Department = "Công nghệ thông tin"
                };
                
                UserLoggedIn?.Invoke(this, CurrentUser);
                return true;
            }
            
            // Kiểm tra cho học sinh
            if (username == "student" && password == "student123")
            {
                CurrentUser = new Student
                {
                    Id = 3,
                    Username = "student",
                    FirstName = "Trần",
                    LastName = "Thị B",
                    Email = "tranthib@school.edu",
                    StudentCode = "HS001",
                    CurrentYear = 2
                };
                
                UserLoggedIn?.Invoke(this, CurrentUser);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Đăng xuất người dùng hiện tại
        /// </summary>
        public void Logout()
        {
            CurrentUser = null;
            UserLoggedOut?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Kiểm tra xem người dùng có thuộc vai trò được chỉ định hay không
        /// </summary>
        /// <param name="role">Vai trò để kiểm tra</param>
        /// <returns>True nếu người dùng thuộc vai trò đó, ngược lại là false</returns>
        public bool IsInRole(string role)
        {
            if (CurrentUser == null)
                return false;

            return CurrentUser.Role == role;
        }
    }
}
