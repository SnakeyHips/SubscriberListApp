using System.Windows;
using DonorListApp.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace DonorListApp.Views
{ 
    public partial class AddHospitalWindow : MetroWindow
    {
        public AddHospitalWindow()
        {
            InitializeComponent();
        }

        private async void btnAddHospital_Click(object sender, RoutedEventArgs e)
        {
            //Make sure each input had an input before being made
            if (txtHospitalName.Text == "")
            {
                await this.ShowMessageAsync("Missing Details", "Please enter the hospital's name");
            }
            else if (txtHospitalAddressLine1.Text == "")
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
                Hospital temp = new Hospital(
                    txtHospitalName.Text,
                    txtHospitalAddressLine1.Text,
                    txtHospitalAddressLine2.Text,
                    txtHospitalPostcode.Text);
                try
                {
                    Json.hospitals.Add(temp.Name, temp);
                    Json.SaveHospitals(Json.hospitals);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                catch
                {
                    await this.ShowMessageAsync("Duplicate Found", "A hospital of the name " + temp.Name + " already exists!");
                }
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
