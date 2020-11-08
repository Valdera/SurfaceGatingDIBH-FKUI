using LiveCharts;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SurfaceGatingDIBH {
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class SettingsPage : BasePage<SettingsViewModel> {
        #region Private Properties
        /// <summary>
        /// Tracker Viem Model
        /// </summary>
        private SettingsViewModel model;
        #endregion

        private void Submit_Settings(object sender, RoutedEventArgs e) {
            FirebaseConfiguration.BasePathURL = model.DatabaseURL;
            FirebaseConfiguration.AuthSecretCode = model.DatabaseAuth;
            MessageBox.Show(model.DatabaseAuth);
        }

        #region Constructor
        /// <summary>
        /// Default Constuctor
        /// </summary>
        public SettingsPage() {
            InitializeComponent();
            model = new SettingsViewModel();
            DataContext = model;
        }
        #endregion

    
    }
}
