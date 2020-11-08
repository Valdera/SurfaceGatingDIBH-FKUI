using System.Windows.Controls;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System;


namespace SurfaceGatingDIBH {
    /// <summary>
    /// A base page for all pages to gain base functionality
    /// </summary>
    public class BasePage<VM> : Page
        where VM : BaseViewModel, new() {
        #region Private Member
        /// <summary>
        /// The view model associated with this page
        /// </summary>
        private VM mViewModel;
        #endregion

        #region Public Properties
        /// <summary>
        /// The animation to play when the page is loaded
        /// </summary>
        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;

        /// <summary>
        /// The animation to play when the page is unloaded
        /// </summary>
        public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToLeft;

        /// <summary>
        /// The time any slide animation takes to complete
        /// </summary>
        public float SlideSeconds { get; set; } = 0.8f;

        /// <summary>
        /// The model associated with this page
        /// </summary>
        public VM ViewModel {
            get { return mViewModel; }
            set {
                // If nothing has changed
                if (mViewModel == value)
                    return;
                // update the value
                mViewModel = value;

                // Set the data context for this page
                this.DataContext = mViewModel;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor 
        /// </summary>
        public BasePage() {
            // If we are animating in, hide to begin with
            if (this.PageLoadAnimation != PageAnimation.None)
                this.Visibility = Visibility.Collapsed;
            // Listen out for the page loading
            this.Loaded += BasePage_Loaded;

            // Create a default view model
            this.ViewModel = new VM();
        }
        #endregion

        #region Animation Load / Unload
        /// <summary>
        /// Once the page is Loaded, perform any required animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BasePage_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            // Animate the page in
            await AnimateIn();
        }

        /// <summary>
        /// Animate the page in
        /// </summary>
        public async Task AnimateIn() {
            // Make sure we have to do something
            if (this.PageLoadAnimation == PageAnimation.None)
                return;
            switch (this.PageLoadAnimation) {
                case PageAnimation.SlideAndFadeInFromRight:
                    // Start the animation
                    await this.SlideAndFadeInFromRight(this.SlideSeconds);
                    break;
            }
        }

        /// <summary>
        /// Animate the page out
        /// </summary>
        public async Task AnimateOut() {
            // Make sure we have to do something
            if (this.PageUnloadAnimation == PageAnimation.None)
                return;
            switch (this.PageUnloadAnimation) {
                case PageAnimation.SlideAndFadeOutToLeft:
                    // Start the animation
                    await this.SlideAndFadeOutToLeft(this.SlideSeconds);
                    break;
            }
        }

        #endregion
    }
}
