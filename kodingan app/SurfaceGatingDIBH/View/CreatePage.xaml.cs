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
    public partial class CreatePage : BasePage<CreateViewModel> {
        #region Private Properties
        /// <summary>
        /// Tracker Viem Model
        /// </summary>
        private CreateViewModel model;
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constuctor
        /// </summary>
        public CreatePage() {
            InitializeComponent();
            model = new CreateViewModel();

            DataContext = model;
        }
        #endregion


    }
}
