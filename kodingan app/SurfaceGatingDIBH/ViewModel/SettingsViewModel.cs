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
    public class SettingsViewModel : BaseViewModel {


        public string DatabaseURL { get; set; }
        public string DatabaseAuth { get; set; } 

        
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsViewModel() {
            DatabaseURL = FirebaseConfiguration.BasePathURL;
            DatabaseAuth = FirebaseConfiguration.AuthSecretCode;
            FirebaseConfiguration.UpdateConfig();
        }


    }


    #endregion
}

