using System;
using StudentManagementV2._1.Models;

namespace StudentManagementV2._1.Services
{
    /// <summary>
    /// Interface for the authentication service
    /// Handles user authentication and session management
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Event raised when a user successfully logs in
        /// </summary>
        event EventHandler<User> UserLoggedIn;

        /// <summary>
        /// Event raised when a user logs out
        /// </summary>
        event EventHandler UserLoggedOut;

        /// <summary>
        /// Gets the currently logged in user
        /// </summary>
        User CurrentUser { get; }

        /// <summary>
        /// Attempts to login with the provided credentials
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password (plain text)</param>
        /// <returns>True if login successful, false otherwise</returns>
        bool Login(string username, string password);

        /// <summary>
        /// Logs out the current user
        /// </summary>
        void Logout();

        /// <summary>
        /// Checks if the user is in the specified role
        /// </summary>
        /// <param name="role">Role to check</param>
        /// <returns>True if user is in the role, false otherwise</returns>
        bool IsInRole(string role);
    }
}
