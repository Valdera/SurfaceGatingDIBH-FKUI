using LiveCharts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace SurfaceGatingDIBH {
    /// <summary>
    /// Interaction logic for TrackerPage.xaml
    /// </summary>
    public partial class TrackerPage : BasePage<TrackerViewModel> {
        #region Private Properties
        /// <summary>
        /// Tracker Viem Model
        /// </summary>
        private TrackerViewModel model;

        /// <summary>
        /// Switch Index for the mini graph
        /// </summary>
        private int switchIndex = 0;

        /// <summary>
        /// Thread for connecting to Arduino
        /// </summary>
        private Thread masterthread;

        /// <summary>
        /// Current Serial Port
        /// </summary>
        private SerialPort mSerialPort;

        /// <summary>
        /// Flag for stoping thread
        /// </summary>
        private bool mStopThread;

        /// <summary>
        /// Flag for serial port choosen
        /// </summary>
        private bool mAlreadyCreated = false;

        /// <summary>
        /// Counter for sensor values
        /// </summary>
        private int counter = 0;
        #endregion


        #region Constructor
        /// <summary>
        /// Default Constuctor
        /// </summary>
        public TrackerPage() {
            InitializeComponent();

            model = new TrackerViewModel();
            DataContext = model;

        }
        #endregion

        #region Functionality
        /// <summary>
        /// When Start Button was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGraph_Click(object sender, RoutedEventArgs e) {
            mSerialPort = new SerialPort(ArduinoComboBox.Text);
            mSerialPort.Close();
            mSerialPort.Dispose();
            mSerialPort.Open();
            if (mAlreadyCreated == false) {
                masterthread = new Thread(Main);
                masterthread.Start();
                mAlreadyCreated = true;
            }
            mStopThread = false;
            StartGraph.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// When stop button was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopGraph_Click(object sender, RoutedEventArgs e) {
            mStopThread = true;
            mSerialPort.Close();
            StartGraph.Visibility = Visibility.Visible;
            StopGraph.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// When Clicked Select Next Graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNext_Click(object sender, RoutedEventArgs e) {
            if (switchIndex < model.MiniValuesArray.Length - 1) {
                switchIndex += 1;
            }
            else {
                switchIndex = 0;
            }
            model.Current_GraphName = model.GraphName_Array[switchIndex];
            model.CurrentMiniValues = model.MiniValuesArray[switchIndex];
        }

        /// <summary>
        /// When Clicked Select Previous Graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPrevious_Click(object sender, RoutedEventArgs e) {
            if (switchIndex == 0) {
                switchIndex = model.MiniValuesArray.Length - 1;
            }
            else {
                switchIndex -= 1;
            }
            model.Current_GraphName = model.GraphName_Array[switchIndex];
            model.CurrentMiniValues = model.MiniValuesArray[switchIndex];
        }

        /// <summary>
        /// Refresh Current Port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoButton_Click(object sender, RoutedEventArgs e) {
            model.Ports = SerialPort.GetPortNames();
        }
        #endregion

        #region Thread
        void Main() {
            while (true) {
                try {
                    if (mStopThread == false) {
                        if (mSerialPort.IsOpen == true) {
                            model.PortIsConnecting = true;
                            string getValue = mSerialPort.ReadLine();
                            string[] values = getValue.Split(';');

                            if (counter >= 3) {
                                model.Values.Add(Convert.ToInt32(values[3]));
                                model.MiniValues_A.Add(Convert.ToInt32(values[0]));
                                model.MiniValues_B.Add(Convert.ToInt32(values[1]));
                                model.MiniValues_C.Add(Convert.ToInt32(values[2]));

                                if (model.Values.Count > 30 || model.HiddenValues.Count > 30) {
                                    model.From += 1;
                                    model.To += 1;
                                }

                                if (model.CurrentMiniValues.Count > 20) {
                                    model.MiniFrom += 1;
                                    model.MiniTo += 1;
                                }

                                if (Convert.ToInt32(values[4]) == 1) {
                                    model.HiddenValues.Add(Convert.ToInt32(values[3]));
                                    model.HiddenList.Add(Convert.ToInt32(values[3]));
                                    model.PatientMax = Convert.ToInt32(model.HiddenValues.Max());
                                    model.PatientMin = model.HiddenList.Min();
                                    model.PatientRate = Convert.ToInt32(model.HiddenValues.Average());
                                    model.From += 1;
                                    model.To += 1;
                                }
                                else {
                                    model.HiddenValues.Add(double.NaN);
                                }

                            }

                            if (counter < 3) {
                                counter += 1;
                            }


                        }
                    }
                }
                catch {

                }
            }
        }
        #endregion

        private void Button_Save_Click(object sender, RoutedEventArgs e) {
            if (!(mSerialPort == null)) {
                mSerialPort.Close();

            }
            mStopThread = true;
            StartGraph.Visibility = Visibility.Visible;
        }
    }
}