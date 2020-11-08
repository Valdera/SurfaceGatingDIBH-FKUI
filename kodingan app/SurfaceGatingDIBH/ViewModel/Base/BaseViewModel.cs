using PropertyChanged;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SurfaceGatingDIBH {
    /// <summary>
    /// A base view model that fires Property Changed events as needed
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged {
        /// <summary>
        /// The event that is fired when any child property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name) {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #region CommandHelpers
        /// <summary>
        /// Runs a command if the updating flag is not set.
        /// If the flag is true (the function is already running) then the action is not run.
        /// If the flag is false (indicating no running function) then the action is run.
        /// Once the action is finished if it was run, then the flag is reset to false
        /// </summary>
        /// <param name="updatingFlag">The boolean property defining if the command is already running</param>
        /// <param name="action">The action if the command is not already running</param>
        /// <returns></returns>
        protected async Task RunCommand(Expression<Func<bool>> updatingFlag, Func<Task> action) {
            // Check if the flag property is true 
            if (updatingFlag.GetPropertyValue())
                return;
            // Set the property flag to true to indicate we are running
            updatingFlag.SetPropertyValue(true);
            try {
                // Run the passed function
                await action();
            }
            finally {
                // Set the property flag back to false now it's finished
                updatingFlag.SetPropertyValue(false);
            }
        }
        #endregion

    }
}
