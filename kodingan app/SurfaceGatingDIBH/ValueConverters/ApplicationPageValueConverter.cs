using System;
using System.Diagnostics;
using System.Globalization;


namespace SurfaceGatingDIBH {
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page
    /// </summary>
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter> {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            switch ((ApplicationPage)value) {
                // Find the approriate page
                case ApplicationPage.Tracker:
                    return new TrackerPage();
                case ApplicationPage.Login:
                    return new LoginPage();
                case ApplicationPage.Create:
                    return new CreatePage();
                case ApplicationPage.Settings:
                    return new SettingsPage();
                case ApplicationPage.Patient:
                    return new PatientPage();
                default:
                    Debugger.Break();
                    return null;
            }

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
