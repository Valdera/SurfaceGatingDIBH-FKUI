using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        
        private static WindowViewModel model;

        public MainWindow() {
            InitializeComponent();
            model = new WindowViewModel(this);
            this.DataContext = model;
        }

        public static void ChangePage(string page) {
            if(page == "tracker") {
                model.CurrentPage = ApplicationPage.Tracker;
            } else if (page == "login") {
                model.CurrentPage = ApplicationPage.Login;
            } else if (page == "create") {
                model.CurrentPage = ApplicationPage.Create;
            } else if (page == "setting") {
                model.CurrentPage = ApplicationPage.Settings;
            }else if(page == "patient") {
                model.CurrentPage = ApplicationPage.Patient;
            }
        }

        private void Button_List_Click(object sender, RoutedEventArgs e) {
            ChangePage("patient");
        }
        private void Button_Settings_Click(object sender, RoutedEventArgs e) {
            ChangePage("setting");
        }
        private void Button_Patient_Click(object sender, RoutedEventArgs e) {
            ChangePage("login");
        }

    }
}
