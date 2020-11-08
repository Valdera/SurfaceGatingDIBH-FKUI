using FireSharp.Response;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SurfaceGatingDIBH {
    public class LoginViewModel : BaseViewModel {

        public string PatientID { get; set; }
        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }
        public bool LoginIsRunning { get; set; }


        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel() {
            LoginCommand = new RelayParameterizedCommand(async (parameter) => await Login(parameter));
        }

        public async Task Login(object parameter) {
            await RunCommand(() => this.LoginIsRunning, async () => {
                try {
                    
                    FirebaseConfiguration.client = new FireSharp.FirebaseClient(FirebaseConfiguration.config);

                    FirebaseResponse response = await FirebaseConfiguration.client.GetAsync("Patient/" + PatientID);
                    Patient result = response.ResultAs<Patient>();
                    if (!(result.Name == null)) {

                        CurrentPatient.Id = result.Id;
                        CurrentPatient.Name = result.Name;
                        CurrentPatient.Age = result.Age;
                        CurrentPatient.Gender = result.Gender;
                        CurrentPatient.MaxGraph = result.MaxGraph;
                        CurrentPatient.MinGraph = result.MinGraph;
                        CurrentPatient.RateGraph = result.RateGraph;


                        MainWindow.ChangePage("tracker");
                    }
                    else {
                        MessageBox.Show("Incorrect ID");
                    }

                }
                catch (Exception ex) {
                    MessageBox.Show("A handled exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                }



            });
        }
    }


    #endregion
}

