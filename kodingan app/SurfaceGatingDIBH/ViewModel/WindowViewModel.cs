using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace SurfaceGatingDIBH {
    /// <summary>
    /// The view model for the custom flat window
    /// </summary>
    class WindowViewModel : BaseViewModel {
        #region Private Member
        /// <summary>
        ///  The window this view model controls
        /// </summary>   
        private Window mWindow;

        /// <summary>
        ///  The margin around window to allow for drop shadow
        /// </summary>
        private int mOuterMarginSize = 10;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private int mWindowRadius = 10;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;
        #endregion

        #region Public Properties
        /// <summary>
        /// The smallest width window can go to
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 400;

        /// <summary>
        /// The smallest height window can go to 
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 400;

        public bool Borderless { get { return mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked; } }

        /// <summary>
        /// The size of the resize border around window
        /// </summary>
        public int ResizeBorder { get { return Borderless ? 0 : 6; } }

        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }

        /// <summary>
        /// The margin around window allow for a drop shadow
        /// </summary>
        public int OuterMarginSize {
            get {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mOuterMarginSize;
            }
            set {
                mOuterMarginSize = value;
            }
        }
        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public int WindowRadius {
            get {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mWindowRadius;
            }
            set {
                mWindowRadius = value;
            }
        }
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

        /// <summary>
        /// The height of the title bar
        /// </summary>
        public int TitleHeight { get; set; } = 42;
        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + ResizeBorder); } }

        /// <summary>
        /// The padding of the content
        /// </summary>
        public Thickness InnerContentPadding { get; set; } = new Thickness(0);

        /// <summary>
        /// The current page
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.Login;

        public ICommand SearchCommand { get; set; }
        public bool SearchIsRunning { get; set; }

        #endregion

        #region Commands
        /// <summary>
        /// The command to minimize the window
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close the window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        ///  The command to show system menu of the window
        /// </summary>
        public ICommand MenuCommand { get; set; }
        #endregion


        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public WindowViewModel(Window window) {
            mWindow = window;

            // Listen out for the window resizing
            mWindow.StateChanged += (sender, e) => {
                // Fire off events of all properties that are affected by a resize 
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginSizeThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };

            // Create Commands
            SearchCommand = new RelayParameterizedCommand(async (parameter) => await Search(parameter));
            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => CloseWindow());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

            // Fix window resize issue
            var resizer = new WindowResizer(mWindow);

        }
        #endregion

        #region Private Helpers
        /// <summary>
        /// Gets the current mouse positions on the screen
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition() {
            // Position of the mouse relative to the window
            var position = Mouse.GetPosition(mWindow);
            // Add the window position so its a "ToScreen"
            return new Point(position.X + mWindow.Left, position.X + mWindow.Top);
        }

        private void CloseWindow() {
            Environment.Exit(Environment.ExitCode);
            mWindow.Close();
        }
        #endregion

        public async Task Search(object parameter) {
            await RunCommand(() => this.SearchIsRunning, async () => {
                try {
                    DataTable newData = new DataTable();

                    FirebaseConfiguration.client = new FireSharp.FirebaseClient(FirebaseConfiguration.config);
                    FirebaseResponse response = await FirebaseConfiguration.client.GetAsync("Patient/");
                    Patient result = response.ResultAs<Patient>();
                    var json = response.Body;
                    Dictionary<string, Patient> elist = JsonConvert.DeserializeObject<Dictionary<string, Patient>>(json);

                    newData.Columns.Add("ID");
                    newData.Columns.Add("Name");
                    newData.Columns.Add("Gender");
                    newData.Columns.Add("Age");
                    newData.Columns.Add("MaxGraph");
                    newData.Columns.Add("MinGraph");
                    newData.Columns.Add("RateGraph");

                    if(!(elist == null)) {
                        foreach (KeyValuePair<string, Patient> entry in elist) {
                            DataRow row = newData.NewRow();
                            row["ID"] = entry.Value.Id;
                            row["Name"] = entry.Value.Name;
                            row["Gender"] = entry.Value.Gender;
                            row["Age"] = entry.Value.Age;
                            row["MaxGraph"] = entry.Value.MaxGraph;
                            row["MinGraph"] = entry.Value.MinGraph;
                            row["RateGraph"] = entry.Value.RateGraph;
                            newData.Rows.Add(row);
                        }

                        DatabasePatient.PatientData = newData;

                        MainWindow.ChangePage("patient");
                    } else {
                        MessageBox.Show("Database Is Empty");
                    }
                   
                }
                catch (Exception ex) {
                    MessageBox.Show("A handled exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }
    }
}
