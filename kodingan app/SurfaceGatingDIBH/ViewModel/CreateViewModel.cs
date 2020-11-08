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
    public class CreateViewModel : BaseViewModel {

        #region Public Propperties
        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        public bool CreateIsRunning { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand CreateCommand { get; set; }
        public string PatientAge { get; set; }
        public string PatientGender { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }




        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public CreateViewModel() {
            CreateCommand = new RelayParameterizedCommand(async (parameter) => await Create(parameter));
        }

        #endregion

        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
        /// <returns></returns>
        public async Task Create(object parameter) {
            await RunCommand(() => this.CreateIsRunning, async () => {
                try {
                    FirebaseConfiguration.client = new FireSharp.FirebaseClient(FirebaseConfiguration.config);


                    var patient = new Patient {
                        Id = this.PatientID,
                        Name = this.PatientName,
                        Age = Convert.ToInt32(this.PatientAge),
                        Gender = this.PatientGender,
                        MaxGraph = 0,
                        MinGraph = 0,
                        RateGraph = 0
                    };

                    bool flag = FirebaseConfiguration.CheckData(patient);

                    if (flag){
                        SetResponse response = await FirebaseConfiguration.client.SetAsync("Patient/" + PatientID, patient);
                        Patient result = response.ResultAs<Patient>();

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
                        MessageBox.Show("Please insert correct data");
                    }
                    
                }
                catch(Exception ex) {
                    MessageBox.Show("A handled exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }
    }



}

