using LiveCharts;
using System;
using System.Collections.Generic;
using System.Data.Linq;
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
    public partial class LoginPage : BasePage<LoginViewModel> {
        #region Private Properties
        /// <summary>
        /// Tracker Viem Model
        /// </summary>
        private LoginViewModel model;

        #endregion


        #region Constructor
        /// <summary>
        /// Default Constuctor
        /// </summary>
        public LoginPage() {
            InitializeComponent();
            model = new LoginViewModel();
            DataContext = model;
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e) {
            MainWindow.ChangePage("create");
        }
    }
}
