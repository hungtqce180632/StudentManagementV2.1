using System;
using System.Windows;
using System.Windows.Input;
using StudentManagementV2._1.Commands;
using StudentManagementV2._1.Services;

namespace StudentManagementV2._1.ViewModels
{
    /// <summary>
    /// ViewModel cho màn hình đăng nhập
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;
        
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _hasError;

        /// <summary>
        /// Tên đăng nhập được nhập bởi người dùng
        /// </summary>
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        /// <summary>
        /// Mật khẩu được nhập bởi người dùng
        /// Không phải là property với thông báo thay đổi vì nó được thiết lập trực tiếp từ PasswordBox
        /// </summary>
        public string Password
        {
            get => _password;
            set => _password = value;
        }

        /// <summary>
        /// Thông báo lỗi để hiển thị
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        /// <summary>
        /// Chỉ ra nếu có lỗi để hiển thị
        /// </summary>
        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        /// <summary>
        /// Command để thực hiện đăng nhập
        /// </summary>
        public ICommand LoginCommand { get; }

        /// <summary>
        /// Constructor cho LoginViewModel
        /// </summary>
        public LoginViewModel()
        {
            // Trong ứng dụng thực tế, dịch vụ xác thực sẽ được tiêm vào qua DI
            _authService = new AuthenticationService();
            
            // Khởi tạo login command
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            
            // Giá trị mặc định
            Username = "admin"; // Thiết lập giá trị mặc định để dễ dàng kiểm tra
        }

        /// <summary>
        /// Kiểm tra xem login command có thể được thực thi hay không
        /// </summary>
        /// <param name="parameter">Tham số command (không sử dụng)</param>
        /// <returns>True nếu command có thể được thực thi, ngược lại là false</returns>
        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        /// <summary>
        /// Thực thi login command
        /// </summary>
        /// <param name="parameter">Tham số command (không sử dụng)</param>
        private void ExecuteLogin(object parameter)
        {
            HasError = false;
            ErrorMessage = string.Empty;

            try
            {
                // Hiển thị thông báo debug
                MessageBox.Show($"Đang thử đăng nhập với:\nUsername: {Username}\nPassword: {Password}", 
                    "Thông tin đăng nhập", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Cố gắng đăng nhập
                bool isSuccess = _authService.Login(Username, Password);

                if (!isSuccess)
                {
                    HasError = true;
                    ErrorMessage = "Tên đăng nhập hoặc mật khẩu không hợp lệ. Vui lòng thử lại.";
                }
                else
                {
                    MessageBox.Show($"Đăng nhập thành công với vai trò: {_authService.CurrentUser.Role}", 
                        "Đăng nhập thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = $"Lỗi đăng nhập: {ex.Message}";
                MessageBox.Show($"Lỗi đăng nhập: {ex.Message}\n\nChi tiết: {ex.ToString()}", 
                    "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
