using System.Windows;
using System.Data.SqlClient;

namespace StudentManagementV2._1;

/// <summary>
/// Logic tương tác cho App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        // Kiểm tra kết nối đến cơ sở dữ liệu
        CheckDatabaseConnection();
    }
    
    /// <summary>
    /// Kiểm tra kết nối đến cơ sở dữ liệu
    /// </summary>
    private void CheckDatabaseConnection()
    {
        string connectionString = "Server=localhost;Database=StudentManagementDb;User Id=sa;Password=123;TrustServerCertificate=True;";
        
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                MessageBox.Show("Đã kết nối thành công đến cơ sở dữ liệu", 
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (System.Exception ex)
        {
            MessageBox.Show($"Không thể kết nối đến cơ sở dữ liệu: {ex.Message}\n\n" +
                $"Vui lòng chạy script SQL trong thư mục Database để tạo cơ sở dữ liệu.\n\n" +
                $"Ứng dụng vẫn sẽ chạy nhưng sẽ sử dụng dữ liệu mẫu thay vì cơ sở dữ liệu thực tế.",
                "Lỗi kết nối cơ sở dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}

