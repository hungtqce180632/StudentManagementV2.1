using StudentManagementV2._1.Commands;
using StudentManagementV2._1.Models;
using StudentManagementV2._1.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StudentManagementV2._1.ViewModels
{
    /// <summary>
    /// ViewModel for the main window, handles navigation and user authentication
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;
        private readonly INavigationService _navigationService;
        
        private bool _isLoggedIn;
        private User _currentUser;
        private object _currentView;
        private ObservableCollection<MenuItem> _menuItems;

        /// <summary>
        /// Indicates if a user is currently logged in
        /// </summary>
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => SetProperty(ref _isLoggedIn, value);
        }

        /// <summary>
        /// Currently logged in user
        /// </summary>
        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        /// <summary>
        /// Currently displayed view in the content area
        /// </summary>
        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        /// <summary>
        /// Menu items in the navigation menu
        /// </summary>
        public ObservableCollection<MenuItem> MenuItems
        {
            get => _menuItems;
            set => SetProperty(ref _menuItems, value);
        }

        /// <summary>
        /// Command to navigate to a different view
        /// </summary>
        public ICommand NavigateCommand { get; }

        /// <summary>
        /// Command to log out the current user
        /// </summary>
        public ICommand LogoutCommand { get; }

        /// <summary>
        /// Constructor for the MainViewModel
        /// </summary>
        public MainViewModel()
        {
            // In a real application, these services would be injected via DI
            _authService = new AuthenticationService();
            _navigationService = new NavigationService();

            // Initialize commands
            NavigateCommand = new RelayCommand(ExecuteNavigate);
            LogoutCommand = new RelayCommand(ExecuteLogout);

            // Initialize properties
            IsLoggedIn = false;
            MenuItems = new ObservableCollection<MenuItem>();

            // Subscribe to navigation and authentication events
            _navigationService.CurrentViewChanged += (sender, view) => CurrentView = view;
            _authService.UserLoggedIn += OnUserLoggedIn;
            _authService.UserLoggedOut += OnUserLoggedOut;
        }

        /// <summary>
        /// Handles user login events
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="user">Logged in user</param>
        private void OnUserLoggedIn(object sender, User user)
        {
            CurrentUser = user;
            IsLoggedIn = true;
            UpdateMenuItems();
            
            // Navigate to default view based on user role
            if (user.Role == "Admin")
                _navigationService.NavigateTo(ViewType.AdminDashboard);
            else if (user.Role == "Teacher")
                _navigationService.NavigateTo(ViewType.TeacherDashboard);
            else if (user.Role == "Student")
                _navigationService.NavigateTo(ViewType.StudentDashboard);
        }

        /// <summary>
        /// Handles user logout events
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void OnUserLoggedOut(object sender, System.EventArgs e)
        {
            CurrentUser = null;
            IsLoggedIn = false;
            MenuItems.Clear();
            CurrentView = null;
        }

        /// <summary>
        /// Updates the navigation menu items based on the current user's role
        /// </summary>
        private void UpdateMenuItems()
        {
            MenuItems.Clear();

            if (CurrentUser == null)
                return;

            switch (CurrentUser.Role)
            {
                case "Admin":
                    // Admin menu items
                    MenuItems.Add(new MenuItem("Dashboard", ViewType.AdminDashboard));
                    MenuItems.Add(new MenuItem("User Management", ViewType.UserManagement));
                    MenuItems.Add(new MenuItem("Course Management", ViewType.CourseManagement));
                    MenuItems.Add(new MenuItem("Class Management", ViewType.ClassManagement));
                    MenuItems.Add(new MenuItem("Room Management", ViewType.RoomManagement));
                    MenuItems.Add(new MenuItem("Schedule Management", ViewType.ScheduleManagement));
                    MenuItems.Add(new MenuItem("Semester Management", ViewType.SemesterManagement));
                    MenuItems.Add(new MenuItem("Announcements", ViewType.AnnouncementManagement));
                    MenuItems.Add(new MenuItem("Exams", ViewType.ExamManagement));
                    MenuItems.Add(new MenuItem("Reports", ViewType.Reports));
                    break;

                case "Teacher":
                    // Teacher menu items
                    MenuItems.Add(new MenuItem("Dashboard", ViewType.TeacherDashboard));
                    MenuItems.Add(new MenuItem("My Classes", ViewType.TeacherClasses));
                    MenuItems.Add(new MenuItem("Attendance", ViewType.TeacherAttendance));
                    MenuItems.Add(new MenuItem("Assignments", ViewType.TeacherAssignments));
                    MenuItems.Add(new MenuItem("Grades", ViewType.TeacherGrades));
                    MenuItems.Add(new MenuItem("Schedule", ViewType.TeacherSchedule));
                    MenuItems.Add(new MenuItem("Announcements", ViewType.TeacherAnnouncements));
                    MenuItems.Add(new MenuItem("Profile", ViewType.TeacherProfile));
                    break;

                case "Student":
                    // Student menu items
                    MenuItems.Add(new MenuItem("Dashboard", ViewType.StudentDashboard));
                    MenuItems.Add(new MenuItem("My Classes", ViewType.StudentClasses));
                    MenuItems.Add(new MenuItem("Schedule", ViewType.StudentSchedule));
                    MenuItems.Add(new MenuItem("Assignments", ViewType.StudentAssignments));
                    MenuItems.Add(new MenuItem("Grades", ViewType.StudentGrades));
                    MenuItems.Add(new MenuItem("Attendance", ViewType.StudentAttendance));
                    MenuItems.Add(new MenuItem("Announcements", ViewType.StudentAnnouncements));
                    MenuItems.Add(new MenuItem("Profile", ViewType.StudentProfile));
                    break;
            }
        }

        /// <summary>
        /// Executes the navigation command
        /// </summary>
        /// <param name="parameter">MenuItem to navigate to</param>
        private void ExecuteNavigate(object parameter)
        {
            if (parameter is MenuItem menuItem)
            {
                // Update selected state
                foreach (var item in MenuItems)
                {
                    item.IsSelected = (item == menuItem);
                }

                // Navigate to the selected view
                _navigationService.NavigateTo(menuItem.ViewType);
            }
        }

        /// <summary>
        /// Executes the logout command
        /// </summary>
        /// <param name="parameter">Command parameter (unused)</param>
        private void ExecuteLogout(object parameter)
        {
            _authService.Logout();
        }
    }

    /// <summary>
    /// Represents a menu item in the navigation menu
    /// </summary>
    public class MenuItem : ViewModelBase
    {
        private string _title;
        private ViewType _viewType;
        private bool _isSelected;

        /// <summary>
        /// Title/text of the menu item
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Type of view this menu item navigates to
        /// </summary>
        public ViewType ViewType
        {
            get => _viewType;
            set => SetProperty(ref _viewType, value);
        }

        /// <summary>
        /// Indicates if this menu item is currently selected
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        /// <summary>
        /// Constructor for MenuItem
        /// </summary>
        /// <param name="title">Title/text of the menu item</param>
        /// <param name="viewType">Type of view this menu item navigates to</param>
        public MenuItem(string title, ViewType viewType)
        {
            Title = title;
            ViewType = viewType;
            IsSelected = false;
        }
    }
}
