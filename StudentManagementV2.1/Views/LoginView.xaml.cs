using System.Windows;
using System.Windows.Controls;
using StudentManagementV2._1.ViewModels;

namespace StudentManagementV2._1.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        /// <summary>
        /// Constructor for the LoginView
        /// </summary>
        public LoginView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the PasswordChanged event of the PasswordBox
        /// Updates the Password property in the ViewModel
        /// </summary>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                // Update the Password property in the ViewModel
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
