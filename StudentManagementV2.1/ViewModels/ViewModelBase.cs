using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudentManagementV2._1.ViewModels
{
    /// <summary>
    /// Base class for all ViewModels in the application
    /// Implements INotifyPropertyChanged to support WPF data binding
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that is fired when a property value changes
        /// WPF binding engine listens to this event to update the UI
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for a specific property
        /// </summary>
        /// <param name="propertyName">Name of the property that changed (automatically captured by CallerMemberName)</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets a property value and raises the PropertyChanged event if the value has changed
        /// Returns true if the value was changed, false otherwise
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="storage">Reference to the backing field</param>
        /// <param name="value">New value to set</param>
        /// <param name="propertyName">Name of the property (automatically captured by CallerMemberName)</param>
        /// <returns>True if the value was changed, false otherwise</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            // Check if the value has actually changed
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            // Update the value and raise the event
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Sets a property value, raises the PropertyChanged event if the value has changed,
        /// and executes an additional action if specified
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="storage">Reference to the backing field</param>
        /// <param name="value">New value to set</param>
        /// <param name="onChanged">Action to execute if the value changes</param>
        /// <param name="propertyName">Name of the property (automatically captured by CallerMemberName)</param>
        /// <returns>True if the value was changed, false otherwise</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            // Set the property and check if it changed
            if (SetProperty(ref storage, value, propertyName))
            {
                // Execute the additional action
                onChanged?.Invoke();
                return true;
            }
            return false;
        }
    }
}
