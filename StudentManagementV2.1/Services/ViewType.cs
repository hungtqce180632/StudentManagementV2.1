namespace StudentManagementV2._1.Services
{
    /// <summary>
    /// Enum representing the different types of views in the application
    /// Used for navigation between views
    /// </summary>
    public enum ViewType
    {
        // Login view
        Login,
        
        // Admin views
        AdminDashboard,
        UserManagement,
        CourseManagement,
        ClassManagement,
        RoomManagement,
        ScheduleManagement,
        SemesterManagement,
        AnnouncementManagement,
        ExamManagement,
        Reports,
        
        // Teacher views
        TeacherDashboard,
        TeacherClasses,
        TeacherAttendance,
        TeacherAssignments,
        TeacherGrades,
        TeacherSchedule,
        TeacherAnnouncements,
        TeacherProfile,
        
        // Student views
        StudentDashboard,
        StudentClasses,
        StudentSchedule,
        StudentAssignments,
        StudentGrades,
        StudentAttendance,
        StudentAnnouncements,
        StudentProfile
    }
}
