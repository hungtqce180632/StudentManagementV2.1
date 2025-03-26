using System;

namespace StudentManagementV2._1.Services
{
    /// <summary>
    /// Interface for the navigation service
    /// Handles navigation between different views in the application
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Event raised when the current view changes
        /// </summary>
        event EventHandler<object> CurrentViewChanged;

        /// <summary>
        /// Navigates to the specified view type
        /// </summary>
        /// <param name="viewType">Type of view to navigate to</param>
        void NavigateTo(ViewType viewType);
    }
}
