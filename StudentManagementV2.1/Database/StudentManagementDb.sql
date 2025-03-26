-- Ensure execution context is master to check/create the database
USE master;
GO

-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'StudentManagementDb')
BEGIN
    PRINT 'Creating database StudentManagementDb...';
    CREATE DATABASE StudentManagementDb;
END
ELSE
BEGIN
    PRINT 'Database StudentManagementDb already exists.';
END
GO

-- Switch to the target database
USE StudentManagementDb;
GO

PRINT 'Dropping existing objects if they exist...';
GO -- Ensure dropping happens in its own batch

-- Drop dependent objects first
IF OBJECT_ID('dbo.StudentAttendance', 'U') IS NOT NULL DROP TABLE dbo.StudentAttendance;
IF OBJECT_ID('dbo.AttendanceRecord', 'U') IS NOT NULL DROP TABLE dbo.AttendanceRecord;
IF OBJECT_ID('dbo.AssignmentSubmission', 'U') IS NOT NULL DROP TABLE dbo.AssignmentSubmission;
IF OBJECT_ID('dbo.Assignment', 'U') IS NOT NULL DROP TABLE dbo.Assignment;
IF OBJECT_ID('dbo.Announcement', 'U') IS NOT NULL DROP TABLE dbo.Announcement;
IF OBJECT_ID('dbo.ScheduleEntry', 'U') IS NOT NULL DROP TABLE dbo.ScheduleEntry;
IF OBJECT_ID('dbo.Enrollment', 'U') IS NOT NULL DROP TABLE dbo.Enrollment;
IF OBJECT_ID('dbo.ClassSection', 'U') IS NOT NULL DROP TABLE dbo.ClassSection;

-- Drop independent tables / tables depended upon
IF OBJECT_ID('dbo.Classroom', 'U') IS NOT NULL DROP TABLE dbo.Classroom;
IF OBJECT_ID('dbo.TimeSlot', 'U') IS NOT NULL DROP TABLE dbo.TimeSlot;
IF OBJECT_ID('dbo.Course', 'U') IS NOT NULL DROP TABLE dbo.Course;
IF OBJECT_ID('dbo.Semester', 'U') IS NOT NULL DROP TABLE dbo.Semester;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users; -- Users table is depended upon by many

-- Drop other objects (Views, Procedures, Triggers)
IF OBJECT_ID('dbo.UserLoginView', 'V') IS NOT NULL DROP VIEW dbo.UserLoginView;
IF OBJECT_ID('dbo.EnrollStudent', 'P') IS NOT NULL DROP PROCEDURE dbo.EnrollStudent;
IF OBJECT_ID('dbo.TR_Enrollment_Insert', 'TR') IS NOT NULL DROP TRIGGER dbo.TR_Enrollment_Insert;
IF OBJECT_ID('dbo.TR_Enrollment_Delete', 'TR') IS NOT NULL DROP TRIGGER dbo.TR_Enrollment_Delete;
GO -- Separate batch for dropping objects

PRINT 'Creating tables...';
GO -- Separate batch for message

-- Create Users table (with TPH inheritance)
CREATE TABLE dbo.Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Admin', 'Teacher', 'Student')),

    -- Admin properties
    Position NVARCHAR(100) NULL,
    OfficeLocation NVARCHAR(100) NULL,

    -- Teacher properties
    TeacherCode NVARCHAR(20) NULL,
    Department NVARCHAR(100) NULL,
    Qualification NVARCHAR(100) NULL,
    JoinDate DATETIME2 NULL,

    -- Student properties
    StudentCode NVARCHAR(20) NULL,
    DateOfBirth DATETIME2 NULL,
    Address NVARCHAR(200) NULL,
    CurrentYear INT NULL,

    -- Common property for both Teacher and Student
    PhoneNumber NVARCHAR(20) NULL
);
GO

-- Create a unique index on username
CREATE UNIQUE INDEX IX_Users_Username ON dbo.Users(Username);
GO

-- Create Semester table
CREATE TABLE dbo.Semester (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    IsActive BIT NOT NULL DEFAULT 0
);
GO

-- Create Course table
CREATE TABLE dbo.Course (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CourseCode NVARCHAR(20) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(1000) NULL,
    Credits INT NOT NULL DEFAULT 3,
    Department NVARCHAR(100) NULL
);
GO

-- Create a unique index on course code
CREATE UNIQUE INDEX IX_Course_CourseCode ON dbo.Course(CourseCode);
GO

-- Create TimeSlot table
CREATE TABLE dbo.TimeSlot (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    Description NVARCHAR(200) NULL
);
GO

-- Create Classroom table
CREATE TABLE dbo.Classroom (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoomNumber NVARCHAR(20) NOT NULL,
    Building NVARCHAR(100) NULL,
    Floor INT NULL,
    Capacity INT NOT NULL DEFAULT 30,
    RoomType NVARCHAR(50) NULL,
    Facilities NVARCHAR(500) NULL
);
GO

-- Create ClassSection table
CREATE TABLE dbo.ClassSection (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SectionName NVARCHAR(20) NOT NULL,
    MaxCapacity INT NOT NULL DEFAULT 30,
    CurrentEnrollment INT NOT NULL DEFAULT 0,
    CourseId INT NOT NULL,
    TeacherId INT NOT NULL,
    SemesterId INT NOT NULL,
    CONSTRAINT FK_ClassSection_Course FOREIGN KEY (CourseId) REFERENCES dbo.Course (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_ClassSection_Teacher FOREIGN KEY (TeacherId) REFERENCES dbo.Users (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_ClassSection_Semester FOREIGN KEY (SemesterId) REFERENCES dbo.Semester (Id) ON DELETE NO ACTION
);
GO

-- Create Enrollment table
CREATE TABLE dbo.Enrollment (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EnrollmentDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    StudentId INT NOT NULL,
    ClassSectionId INT NOT NULL,
    FinalGrade DECIMAL(5, 2) NULL,
    IsCompleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Enrollment_Student FOREIGN KEY (StudentId) REFERENCES dbo.Users (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Enrollment_ClassSection FOREIGN KEY (ClassSectionId) REFERENCES dbo.ClassSection (Id) ON DELETE NO ACTION,
    CONSTRAINT UQ_Enrollment_Student_Section UNIQUE (StudentId, ClassSectionId)
);
GO

-- Create ScheduleEntry table
CREATE TABLE dbo.ScheduleEntry (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DayOfWeek NVARCHAR(10) NOT NULL,
    ClassSectionId INT NOT NULL,
    TimeSlotId INT NOT NULL,
    ClassroomId INT NOT NULL,
    CONSTRAINT FK_ScheduleEntry_ClassSection FOREIGN KEY (ClassSectionId) REFERENCES dbo.ClassSection (Id) ON DELETE CASCADE,
    CONSTRAINT FK_ScheduleEntry_TimeSlot FOREIGN KEY (TimeSlotId) REFERENCES dbo.TimeSlot (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_ScheduleEntry_Classroom FOREIGN KEY (ClassroomId) REFERENCES dbo.Classroom (Id) ON DELETE NO ACTION
);
GO

-- Create Assignment table
CREATE TABLE dbo.Assignment (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(2000) NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    DueDate DATETIME2 NOT NULL,
    MaxPoints DECIMAL(10, 2) NOT NULL DEFAULT 100.00,
    Weight DECIMAL(5, 2) NOT NULL DEFAULT 1.00,
    ClassSectionId INT NOT NULL,
    CONSTRAINT FK_Assignment_ClassSection FOREIGN KEY (ClassSectionId) REFERENCES dbo.ClassSection (Id) ON DELETE CASCADE
);
GO

-- Create AssignmentSubmission table
CREATE TABLE dbo.AssignmentSubmission (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SubmissionDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    Content NVARCHAR(4000) NULL,
    FilePath NVARCHAR(500) NULL,
    Score DECIMAL(10, 2) NULL,
    Feedback NVARCHAR(1000) NULL,
    GradedDate DATETIME2 NULL,
    IsLate BIT NOT NULL DEFAULT 0,
    StudentId INT NOT NULL,
    AssignmentId INT NOT NULL,
    CONSTRAINT FK_AssignmentSubmission_Student FOREIGN KEY (StudentId) REFERENCES dbo.Users (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_AssignmentSubmission_Assignment FOREIGN KEY (AssignmentId) REFERENCES dbo.Assignment (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Submission_Student_Assignment UNIQUE (StudentId, AssignmentId)
);
GO

-- Create AttendanceRecord table
CREATE TABLE dbo.AttendanceRecord (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Date DATETIME2 NOT NULL,
    ClassSectionId INT NOT NULL,
    Notes NVARCHAR(500) NULL,
    CONSTRAINT FK_AttendanceRecord_ClassSection FOREIGN KEY (ClassSectionId) REFERENCES dbo.ClassSection (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_AttendanceRecord_Date_Section UNIQUE (Date, ClassSectionId)
);
GO

-- Create StudentAttendance table
CREATE TABLE dbo.StudentAttendance (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Present', 'Absent', 'Late', 'Excused')),
    Comment NVARCHAR(200) NULL,
    StudentId INT NOT NULL,
    AttendanceRecordId INT NOT NULL,
    CONSTRAINT FK_StudentAttendance_Student FOREIGN KEY (StudentId) REFERENCES dbo.Users (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_StudentAttendance_AttendanceRecord FOREIGN KEY (AttendanceRecordId) REFERENCES dbo.AttendanceRecord (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_StudentAttendance_Student_Record UNIQUE (StudentId, AttendanceRecordId)
);
GO

-- Create Announcement table
CREATE TABLE dbo.Announcement (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Content NVARCHAR(4000) NOT NULL,
    PostedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ExpiryDate DATETIME2 NULL,
    PostedById INT NOT NULL,
    ClassSectionId INT NULL, -- NULL for general announcements
    TargetAudience NVARCHAR(20) NOT NULL DEFAULT 'All' CHECK (TargetAudience IN ('All', 'Students', 'Teachers')),
    IsImportant BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Announcement_PostedBy FOREIGN KEY (PostedById) REFERENCES dbo.Users (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Announcement_ClassSection FOREIGN KEY (ClassSectionId) REFERENCES dbo.ClassSection (Id) ON DELETE CASCADE
);
GO

PRINT 'Inserting seed data...';
GO -- Separate batch for message

-- Insert seed data
BEGIN TRY -- Wrap seed data in TRY/CATCH to see potential issues

    -- Insert admin user
    INSERT INTO dbo.Users (Username, PasswordHash, FirstName, LastName, Email, IsActive, Role, Position, OfficeLocation, PhoneNumber)
    VALUES ('admin', '$2a$11$gYPllSCX7k3AMY0qzjHyKuEA1D9v.z6G3e2yK8X5vUgPnQn8zAIYy', N'Quản trị', N'Hệ thống', 'admin@school.edu', 1, 'Admin', N'Quản trị viên hệ thống', N'Văn phòng chính', '123-456-7890');

    -- Insert teacher user
    INSERT INTO dbo.Users (Username, PasswordHash, FirstName, LastName, Email, IsActive, Role, TeacherCode, Department, Qualification, PhoneNumber, JoinDate)
    VALUES ('teacher', '$2a$11$A2wjx/lNlJJn32hFyoJ/GOFsF/IwJYHnGXnFt1xOQiTITYWGvq/P2', N'Nguyễn', N'Văn A', 'teacher@school.edu', 1, 'Teacher', 'TC001', N'Công nghệ thông tin', N'Tiến sĩ', '987-654-3210', '2020-01-15');

    -- Insert student user
    INSERT INTO dbo.Users (Username, PasswordHash, FirstName, LastName, Email, IsActive, Role, StudentCode, DateOfBirth, Address, PhoneNumber, CurrentYear)
    VALUES ('student', '$2a$11$krT3RXJPKKMEz1QNdFXvOeduWGchpCbNO4OZnNt0U.i1lrF4vJhYS', N'Trần', N'Thị B', 'student@school.edu', 1, 'Student', 'ST001', '2000-05-20', N'123 Đường Nguyễn Huệ, TP.HCM', '555-123-4567', 2);

    -- Insert semesters
    INSERT INTO dbo.Semester (Name, StartDate, EndDate, IsActive)
    VALUES
    (N'Học kỳ 1 - 2023-2024', '2023-09-01', '2024-01-15', 0),
    (N'Học kỳ 2 - 2023-2024', '2024-02-01', '2024-06-15', 1);

    -- Insert courses
    INSERT INTO dbo.Course (CourseCode, Name, Description, Credits, Department)
    VALUES
    ('CS101', N'Nhập môn lập trình', N'Giới thiệu về các khái niệm cơ bản trong lập trình', 3, N'Công nghệ thông tin'),
    ('CS201', N'Cấu trúc dữ liệu và giải thuật', N'Học về các cấu trúc dữ liệu và thuật toán cơ bản', 4, N'Công nghệ thông tin'),
    ('MATH101', N'Giải tích 1', N'Giới thiệu về phép tính vi phân và tích phân', 3, N'Toán học'),
    ('ENG101', N'Tiếng Anh học thuật', N'Phát triển kỹ năng tiếng Anh học thuật', 2, N'Ngoại ngữ');

    -- Insert time slots
    INSERT INTO dbo.TimeSlot (Name, StartTime, EndTime, Description)
    VALUES
    (N'Tiết 1-3', '07:00:00', '09:30:00', N'Buổi sáng sớm'),
    (N'Tiết 4-6', '09:45:00', '12:15:00', N'Buổi sáng muộn'),
    (N'Tiết 7-9', '12:30:00', '15:00:00', N'Buổi chiều sớm'),
    (N'Tiết 10-12', '15:15:00', '17:45:00', N'Buổi chiều muộn');

    -- Insert classrooms
    INSERT INTO dbo.Classroom (RoomNumber, Building, Floor, Capacity, RoomType, Facilities)
    VALUES
    ('A101', N'Tòa nhà A', 1, 40, N'Phòng học thường', N'Máy chiếu, Bảng trắng'),
    ('A102', N'Tòa nhà A', 1, 40, N'Phòng học thường', N'Máy chiếu, Bảng trắng'),
    ('B201', N'Tòa nhà B', 2, 30, N'Phòng lab', N'Máy tính, Máy chiếu, Bảng trắng'),
    ('C301', N'Tòa nhà C', 3, 100, N'Hội trường', N'Âm thanh, Máy chiếu, Bảng trắng');

    -- Insert class sections (Assuming Teacher ID 2 and Student ID 3 correspond to the inserted users)
    DECLARE @TeacherId INT = (SELECT Id FROM dbo.Users WHERE Username = 'teacher');
    DECLARE @StudentId INT = (SELECT Id FROM dbo.Users WHERE Username = 'student');
    DECLARE @AdminId INT = (SELECT Id FROM dbo.Users WHERE Username = 'admin');
    DECLARE @SemesterId INT = (SELECT Id FROM dbo.Semester WHERE Name = N'Học kỳ 2 - 2023-2024');
    DECLARE @CourseCS101Id INT = (SELECT Id FROM dbo.Course WHERE CourseCode = 'CS101');
    DECLARE @CourseCS201Id INT = (SELECT Id FROM dbo.Course WHERE CourseCode = 'CS201');
    DECLARE @CourseMATH101Id INT = (SELECT Id FROM dbo.Course WHERE CourseCode = 'MATH101');
    DECLARE @CourseENG101Id INT = (SELECT Id FROM dbo.Course WHERE CourseCode = 'ENG101');

    INSERT INTO dbo.ClassSection (SectionName, MaxCapacity, CurrentEnrollment, CourseId, TeacherId, SemesterId)
    VALUES
    ('CS101-01', 40, 0, @CourseCS101Id, @TeacherId, @SemesterId),
    ('CS201-01', 30, 0, @CourseCS201Id, @TeacherId, @SemesterId),
    ('MATH101-01', 50, 0, @CourseMATH101Id, @TeacherId, @SemesterId),
    ('ENG101-01', 35, 0, @CourseENG101Id, @TeacherId, @SemesterId);

    DECLARE @ClassSectionCS101_01 INT = (SELECT Id FROM dbo.ClassSection WHERE SectionName = 'CS101-01' AND SemesterId = @SemesterId);
    DECLARE @ClassSectionCS201_01 INT = (SELECT Id FROM dbo.ClassSection WHERE SectionName = 'CS201-01' AND SemesterId = @SemesterId);
    DECLARE @ClassSectionMATH101_01 INT = (SELECT Id FROM dbo.ClassSection WHERE SectionName = 'MATH101-01' AND SemesterId = @SemesterId);
    DECLARE @ClassSectionENG101_01 INT = (SELECT Id FROM dbo.ClassSection WHERE SectionName = 'ENG101-01' AND SemesterId = @SemesterId);

    -- Insert enrollments (Triggers will update CurrentEnrollment)
    INSERT INTO dbo.Enrollment (StudentId, ClassSectionId, FinalGrade, IsCompleted)
    VALUES
    (@StudentId, @ClassSectionCS101_01, NULL, 0),
    (@StudentId, @ClassSectionCS201_01, NULL, 0);

    DECLARE @TimeSlot1_3 INT = (SELECT Id FROM dbo.TimeSlot WHERE Name = N'Tiết 1-3');
    DECLARE @TimeSlot4_6 INT = (SELECT Id FROM dbo.TimeSlot WHERE Name = N'Tiết 4-6');
    DECLARE @ClassroomA101 INT = (SELECT Id FROM dbo.Classroom WHERE RoomNumber = 'A101');
    DECLARE @ClassroomB201 INT = (SELECT Id FROM dbo.Classroom WHERE RoomNumber = 'B201');

    -- Insert schedule entries
    INSERT INTO dbo.ScheduleEntry (DayOfWeek, ClassSectionId, TimeSlotId, ClassroomId)
    VALUES
    (N'Thứ 2', @ClassSectionCS101_01, @TimeSlot1_3, @ClassroomA101),
    (N'Thứ 4', @ClassSectionCS101_01, @TimeSlot1_3, @ClassroomA101),
    (N'Thứ 3', @ClassSectionCS201_01, @TimeSlot4_6, @ClassroomB201),
    (N'Thứ 5', @ClassSectionCS201_01, @TimeSlot4_6, @ClassroomB201);

    -- Insert assignments
    INSERT INTO dbo.Assignment (Title, Description, CreatedDate, DueDate, MaxPoints, Weight, ClassSectionId)
    VALUES
    (N'Bài tập 1 - Giới thiệu lập trình', N'Viết một chương trình đơn giản để in ra "Hello World"', GETDATE(), DATEADD(DAY, 14, GETDATE()), 100.00, 0.10, @ClassSectionCS101_01),
    (N'Bài tập 2 - Vòng lặp cơ bản', N'Viết các chương trình sử dụng vòng lặp for, while, do-while', GETDATE(), DATEADD(DAY, 21, GETDATE()), 100.00, 0.15, @ClassSectionCS101_01),
    (N'Bài tập 1 - Danh sách liên kết', N'Triển khai danh sách liên kết đơn và các thao tác cơ bản', GETDATE(), DATEADD(DAY, 14, GETDATE()), 100.00, 0.20, @ClassSectionCS201_01);

    -- Insert announcements
    INSERT INTO dbo.Announcement (Title, Content, PostedById, ClassSectionId, TargetAudience, IsImportant)
    VALUES
    (N'Chào mừng đến với học kỳ mới', N'Chào mừng tất cả sinh viên đến với học kỳ mới. Chúc các bạn học tập tốt!', @AdminId, NULL, 'All', 1),
    (N'Thay đổi lịch học CS101', N'Lớp CS101 sẽ được chuyển sang phòng A102 từ tuần sau.', @TeacherId, @ClassSectionCS101_01, 'Students', 0),
    (N'Buổi kiểm tra giữa kỳ CS201', N'Buổi kiểm tra giữa kỳ cho môn CS201 sẽ diễn ra vào ngày 15/04/2024.', @TeacherId, @ClassSectionCS201_01, 'Students', 1);

    PRINT 'Seed data inserted successfully.';

END TRY
BEGIN CATCH
    PRINT 'Error inserting seed data:';
    PRINT ERROR_MESSAGE();
    -- Optionally rethrow the error if needed
    -- THROW;
END CATCH
GO -- End of seed data batch

PRINT 'Creating triggers...';
GO -- Separate batch for message

-- Create a trigger to update CurrentEnrollment when a student enrolls in a class
-- **Ensure GO is BEFORE this statement**
GO
CREATE TRIGGER TR_Enrollment_Insert
ON dbo.Enrollment
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cs
    SET cs.CurrentEnrollment = ISNULL((SELECT COUNT(*) FROM dbo.Enrollment e WHERE e.ClassSectionId = cs.Id), 0)
    FROM dbo.ClassSection cs
    INNER JOIN inserted i ON cs.Id = i.ClassSectionId;
END
GO -- **Ensure GO is AFTER this statement**

-- Create a trigger to update CurrentEnrollment when a student un-enrolls from a class
-- **Ensure GO is BEFORE this statement**
GO
CREATE TRIGGER TR_Enrollment_Delete
ON dbo.Enrollment
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cs
    SET cs.CurrentEnrollment = ISNULL((SELECT COUNT(*) FROM dbo.Enrollment e WHERE e.ClassSectionId = cs.Id), 0)
    FROM dbo.ClassSection cs
    INNER JOIN deleted d ON cs.Id = d.ClassSectionId;
END
GO -- **Ensure GO is AFTER this statement**

PRINT 'Creating stored procedure EnrollStudent...';
GO -- Separate batch for message

-- Create stored procedure for student enrollment
-- **Ensure GO is BEFORE this statement**
GO
CREATE PROCEDURE dbo.EnrollStudent
    @StudentId INT,
    @ClassSectionId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if student exists and is a student
    IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Id = @StudentId AND Role = 'Student' AND IsActive = 1)
    BEGIN
        RAISERROR('Active student not found.', 16, 1);
        RETURN -1; -- Indicate error
    END

    -- Check if class section exists
    DECLARE @CurrentEnrollment INT, @MaxCapacity INT, @SemesterIsActive BIT;
    SELECT @CurrentEnrollment = cs.CurrentEnrollment,
           @MaxCapacity = cs.MaxCapacity,
           @SemesterIsActive = s.IsActive
    FROM dbo.ClassSection cs
    JOIN dbo.Semester s ON cs.SemesterId = s.Id
    WHERE cs.Id = @ClassSectionId;

    IF @@ROWCOUNT = 0 -- Check if the SELECT found a row
    BEGIN
        RAISERROR('Class section not found.', 16, 1);
        RETURN -2; -- Indicate error
    END

    -- Optional: Check if the semester is active for new enrollments
    -- IF @SemesterIsActive = 0
    -- BEGIN
    --     RAISERROR('Cannot enroll in a class section for an inactive semester.', 16, 1);
    --     RETURN -3; -- Indicate error
    -- END

    -- Check if student is already enrolled
    IF EXISTS (SELECT 1 FROM dbo.Enrollment WHERE StudentId = @StudentId AND ClassSectionId = @ClassSectionId)
    BEGIN
        RAISERROR('Student is already enrolled in this class section.', 16, 1);
        RETURN -4; -- Indicate error
    END

    -- Check if class is full
    IF @CurrentEnrollment >= @MaxCapacity
    BEGIN
        RAISERROR('Class section is already full.', 16, 1);
        RETURN -5; -- Indicate error
    END

    -- Enroll student (The trigger TR_Enrollment_Insert will handle updating CurrentEnrollment)
    BEGIN TRY
        INSERT INTO dbo.Enrollment (StudentId, ClassSectionId, EnrollmentDate)
        VALUES (@StudentId, @ClassSectionId, GETDATE());

        -- Success
        PRINT 'Student enrolled successfully.'; -- Use PRINT instead of SELECT for messages
        RETURN 0; -- Indicate success
    END TRY
    BEGIN CATCH
        -- If any error occurs during insert (e.g., constraint violation)
        DECLARE @ErrorMessage NVARCHAR(MAX) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN -99; -- Indicate generic error during insert
    END CATCH
END
GO -- **Ensure GO is AFTER this statement**

PRINT 'Creating view UserLoginView...';
GO -- Separate batch for message

-- Create a login view for easier user authentication
-- **Ensure GO is BEFORE this statement**
GO
CREATE VIEW dbo.UserLoginView
AS
SELECT
    Id,
    Username,
    PasswordHash,
    FirstName,
    LastName,
    Email,
    Role,
    IsActive
FROM dbo.Users;
GO -- **Ensure GO is AFTER this statement**

PRINT 'Database setup script completed successfully.';
GO