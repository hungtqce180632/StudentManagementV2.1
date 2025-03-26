using System;
using StudentManagementV2._1.ViewModels;

namespace StudentManagementV2._1.Services
{
    /// <summary>
    /// Service for navigating between different views in the application
    /// </summary>
    public class NavigationService : INavigationService
    {
        /// <summary>
        /// Event raised when the current view changes
        /// </summary>
        public event EventHandler<object> CurrentViewChanged;

        /// <summary>
        /// Navigates to the specified view type
        /// </summary>
        /// <param name="viewType">Type of view to navigate to</param>
        public void NavigateTo(ViewType viewType)
        {
            // Create the appropriate view based on the view type
            object view = CreateView(viewType);

            // Raise the CurrentViewChanged event
            CurrentViewChanged?.Invoke(this, view);
        }

        /// <summary>
        /// Creates and returns the appropriate view model for the specified view type
        /// </summary>
        /// <param name="viewType">Type of view to create</param>
        /// <returns>The created view model</returns>
        private object CreateView(ViewType viewType)
        {
            // In a real implementation, this would create and return actual view models
            // For now, this is a placeholder that returns a string representing the view
            switch (viewType)
            {
                case ViewType.Login:
                    return "This is the Login view";
                
                // Admin views
                case ViewType.AdminDashboard:
                    return "This is the Admin Dashboard view";
                case ViewType.UserManagement:
                    return "This is the User Management view";
                case ViewType.CourseManagement:
                    return "This is the Course Management view";
                case ViewType.ClassManagement:
                    return "This is the Class Management view";
                case ViewType.RoomManagement:
                    return "This is the Room Management view";
                case ViewType.ScheduleManagement:
                    return "This is the Schedule Management view";
                case ViewType.SemesterManagement:
                    return "This is the Semester Management view";
                case ViewType.AnnouncementManagement:
                    return "This is the Announcement Management view";
                case ViewType.ExamManagement:
                    return "This is the Exam Management view";
                case ViewType.Reports:
                    return "This is the Reports view";
                
                // Teacher views
                case ViewType.TeacherDashboard:
                    return "This is the Teacher Dashboard view";
                case ViewType.TeacherClasses:
                    return "This is the Teacher Classes view";
                case ViewType.TeacherAttendance:
                    return "This is the Teacher Attendance view";
                case ViewType.TeacherAssignments:
                    return "This is the Teacher Assignments view";
                case ViewType.TeacherGrades:
                    return "This is the Teacher Grades view";
                case ViewType.TeacherSchedule:
                    return "This is the Teacher Schedule view";
                case ViewType.TeacherAnnouncements:
                    return "This is the Teacher Announcements view";
                case ViewType.TeacherProfile:
                    return "This is the Teacher Profile view";
                
                // Student views
                case ViewType.StudentDashboard:
                    return "This is the Student Dashboard view";
                case ViewType.StudentClasses:
                    return "This is the Student Classes view";
                case ViewType.StudentSchedule:
                    return "This is the Student Schedule view";
                case ViewType.StudentAssignments:
                    return "This is the Student Assignments view";
                case ViewType.StudentGrades:
                    return "This is the Student Grades view";
                case ViewType.StudentAttendance:
                    return "This is the Student Attendance view";
                case ViewType.StudentAnnouncements:
                    return "This is the Student Announcements view";
                case ViewType.StudentProfile:
                    return "This is the Student Profile view";
                
                default:
                    return "Unknown view";
            }
        }
    }
}
