using System.Windows;
using DonorListApp.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace DonorListApp.Views
{
    public partial class EditHospitalWindow : MetroWindow
    {
        Hospital Hospital;

        public EditHospitalWindow(Hospital selectedHospital)
        {
            InitializeComponent();
            Hospital = selectedHospital;
            txtHospitalName.Text = Hospital.Name;
            txtHospitalAddressLine1.Text = Hospital.AddressLine1;
            txtHospitalAddressLine2.Text = Hospital.AddressLine2;
            txtHospitalPostcode.Text = Hospital.Postcode;
        }

        private async void btnEditHospital_Click(object sender, RoutedEventArgs e)
        {
            //Make sure each input had an input before being made
            if (txtHospitalAddressLine1.Text == "")
            {
                await this.ShowMessageAsync("Missing Details", "Please enter the hospital's address line 1");
            }
            else if (txtHospitalAddressLine2.Text == "")
            {
                await this.ShowMessageAsync("Missing Details", "Please enter the hospital's address line 2");
            }
            else if (txtHospitalPostcode.Text == "")
            {
                await this.ShowMessageAsync("Missing Details", "Please enter the hospital's postcode");
            }
            else
            {
                Hospital.AddressLine1 = txtHospitalAddressLine1.Text;
                Hospital.AddressLine2 = txtHospitalAddressLine2.Text;
                Hospital.Postcode = txtHospitalPostcode.Text;
                Json.hospitals[Hospital.Name] = Hospital;
                Json.SaveHospitals(Json.hospitals);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
