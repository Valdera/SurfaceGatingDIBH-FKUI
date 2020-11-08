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
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace SurfaceGatingDIBH {
    public class TrackerViewModel : BaseViewModel {
        #region Private Properties
        /// <summary>
        /// Max Value for X Graph
        /// </summary>
        private double _to;

        /// <summary>
        /// Max Value for X Mini Graph
        /// </summary>
        private double _miniTo;

        /// <summary>
        /// Min value for X Graph
        /// </summary>
        private double _from;

        /// <summary>
        /// Min value for X Mini Graph
        /// </summary>
        private double _miniFrom;
        #endregion

        #region Public Properties
        /// <summary>
        /// Yellow Color
        /// </summary>
        public Color ColorYellow { get; set; }

        /// <summary>
        /// Red Color
        /// </summary>
        public Color ColorRed { get; set; }

        /// <summary>
        /// Transparent Color
        /// </summary>
        public Color Transparent { get; set; }

        /// <summary>
        /// Red Brush
        /// </summary>
        public Brush ColorRedBrush { get; set; }

        /// <summary>
        /// Yellow Brush
        /// </summary>
        public Brush ColorYellowBrush { get; set; }

        /// <summary>
        /// Transparent Brush
        /// </summary>
        public Brush TransparentBrush { get; set; }

        /// <summary>
        /// Current Hidden Stroke
        /// </summary>
        public Brush CurrentHiddenStroke { get; set; }

        /// <summary>
        /// Current Stroke
        /// </summary>
        public Brush CurrentStroke { get; set; }

        /// <summary>
        /// The serial port
        /// </summary>
        public String[] Ports { get; set; }

        /// <summary>
        /// Chart values for the Main Graph 
        /// </summary>
        public ChartValues<double> Values { get; set; }

        /// <summary>
        /// Current Displayed Mini Chart
        /// </summary>
        public ChartValues<double> CurrentMiniValues { get; set; }

        /// <summary>
        /// Chart Values for Long Respiration
        /// </summary>
        public ChartValues<double> HiddenValues { get; set; }

        /// <summary>
        /// List for Hidden Values Calculation
        /// </summary>
        public List<int> HiddenList = new List<int>();
        
        /// <summary>
        /// An Array for Mini Graph
        /// </summary>
        public ChartValues<double>[] MiniValuesArray { get; set; }

        /// <summary>
        /// Chart values for the Mini Graph A
        /// </summary>
        public ChartValues<double> MiniValues_A { get; set; }

        /// <summary>
        /// Chart values for the Mini Graph B
        /// </summary>
        public ChartValues<double> MiniValues_B { get; set; }

        /// <summary>
        /// Chart values for the Mini Graph C
        /// </summary>
        public ChartValues<double> MiniValues_C { get; set; }

        /// <summary>
        /// An Array for Graph Name 
        /// </summary>
        public string[] GraphName_Array { get; set; } 

        /// <summary>
        /// Current Mini Graph Name
        /// </summary>
        public string Current_GraphName { get; set; }

        /// <summary>
        /// Name for Mini Graph A
        /// </summary>
        public string GraphName_A { get; set; } = "Graph For Position A";

        /// <summary>
        /// Name for Mini Graph B
        /// </summary>
        public string GraphName_B { get; set; } = "Graph For Position B";

        /// <summary>
        /// Name for Mini Graph C
        /// </summary>
        public string GraphName_C { get; set; } = "Graph For Position C";

        /// <summary>
        /// Max Value in Main Chart Values
        /// </summary>
        public double ValueMax { get; set; } = 0;

        /// <summary>
        /// Max Value in Main Chart Values
        /// </summary>
        public double ValueMin { get; set; } = 0;

        /// <summary>
        /// Max Value in Main Chart Values
        /// </summary>
        public double ValueRate { get; set; } = 0;

        /// <summary>
        /// A flag indicating if the graph is running
        /// </summary>
        public bool PortIsConnecting { get; set; }

        public string PatientName { get; set; }

        public string PatientID { get; set; } 
        public string PatientAge { get; set; }
        public string PatientGender { get; set; }

        public int PatientMin { get; set; }
        public int PatientRate { get; set; }
        public int PatientMax { get; set; }

        public int FillWidth { get; set; }
        public int FillMin { get; set; }


        /// <summary>
        /// Min Value for X Graph
        /// </summary>
        public double From {
            get { return _from; }
            set {
                _from = value;
                OnPropertyChanged("From");
            }
        }

        /// <summary>
        /// Min Value for X Mini Graph
        /// </summary>
        public double MiniFrom {
            get { return _miniFrom; }
            set {
                _miniFrom = value;
                OnPropertyChanged("MiniFrom");
            }
        }

        /// <summary>
        /// Max value for X Graph
        /// </summary>
        public double To {
            get { return _to; }
            set {
                _to = value;
                OnPropertyChanged("To");
            }
        }

        /// <summary>
        /// Max value for X Mini Graph
        /// </summary>
        public double MiniTo {
            get { return _miniTo; }
            set {
                _miniTo = value;
                OnPropertyChanged("MiniTo");
            }
        }

        public ICommand SaveCommand { get; set; }
        public bool SaveIsRunning { get; set; }


        #endregion

        #region Commands


        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public TrackerViewModel() {
            SaveCommand = new RelayParameterizedCommand(async (parameter) => await Save(parameter));

            Ports = SerialPort.GetPortNames();
            // Values for Graph
            CurrentMiniValues = new ChartValues<double>();
            Values = new ChartValues<double>();
            HiddenValues = new ChartValues<double>();
            MiniValues_A = new ChartValues<double>();
            MiniValues_B = new ChartValues<double>();
            MiniValues_C = new ChartValues<double>();

            ColorYellow = (Color)ColorConverter.ConvertFromString("#FFF2CD21");
            ColorRed = (Color)ColorConverter.ConvertFromString("#FFEB1313");
            Transparent = (Color)ColorConverter.ConvertFromString("#0000ffff");

            ColorYellowBrush = new SolidColorBrush(ColorYellow);
            ColorRedBrush = new SolidColorBrush(ColorRed);
            TransparentBrush = new SolidColorBrush(Transparent);

            CurrentStroke = ColorYellowBrush;
            CurrentHiddenStroke = ColorRedBrush;            

            MiniValuesArray = new ChartValues<double>[] { MiniValues_A, MiniValues_B, MiniValues_C };
            GraphName_Array = new string[] { GraphName_A, GraphName_B, GraphName_C };

            Current_GraphName = GraphName_A;
            CurrentMiniValues = MiniValues_A;

            PatientName = CurrentPatient.Name;
            PatientID = CurrentPatient.Id;
            PatientAge = Convert.ToString(CurrentPatient.Age) + " years old";
            PatientGender = CurrentPatient.Gender;
            PatientMin = CurrentPatient.MinGraph;
            PatientMax = CurrentPatient.MaxGraph;
            PatientRate = CurrentPatient.RateGraph;
            FillWidth = PatientMax - PatientMin;
            FillMin = PatientMin;

            // How many Data showing in the table
            MiniFrom = 0;
            MiniTo = 25;

            From = 0;
            To = 50;
        }

        public async Task Save(object parameter) {
            await RunCommand(() => this.SaveIsRunning, async () => {
                try {
                    FirebaseConfiguration.client = new FireSharp.FirebaseClient(FirebaseConfiguration.config);


                    var patient = new Patient {
                        Id = this.PatientID,
                        Name = this.PatientName,
                        Gender = this.PatientGender,
                        Age = CurrentPatient.Age,
                        MaxGraph = this.PatientMax,
                        MinGraph = this.PatientMin,
                        RateGraph = this.PatientRate
                    };


                    SetResponse response = await FirebaseConfiguration.client.SetAsync("Patient/" + PatientID, patient);

                    Patient result = response.ResultAs<Patient>();

                    CurrentPatient.Id = result.Id;
                    CurrentPatient.Name = result.Name;
                    CurrentPatient.Age = result.Age;
                    CurrentPatient.Gender = result.Gender;
                    CurrentPatient.MaxGraph = result.MaxGraph;
                    CurrentPatient.MinGraph = result.MinGraph;
                    CurrentPatient.RateGraph = result.RateGraph;
                    MessageBox.Show("Data has been saved :)");
                    //MainWindow.ChangePage("tracker");
                }
                catch (Exception ex) {
                    MessageBox.Show("A handled exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }
    }


    #endregion
}

