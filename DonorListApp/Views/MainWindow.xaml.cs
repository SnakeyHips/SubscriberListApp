using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using DonorListApp.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using iTextSharp.text.pdf;
using MahApps.Metro;

namespace DonorListApp.Views
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Json.hospitals = Json.GetHospitals();
            lstHospitals.ItemsSource = Json.hospitals;
            lstHospitals.SelectedIndex = Hospital.selectedHospital;

            Json.subscribers = Json.GetSubscribers();
            lstSubscribers.ItemsSource = Json.subscribers;
            lstSubscribers.SelectedIndex = Subscriber.selectedSubscriber;

            if(Json.selectedTab == 1)
            {
                tabSubscribers.IsSelected = true;
            }
        }

        private void SubscriptionCheckboxes(string subscriptions)
        {
            grdSubscriber.FindChildren<CheckBox>().ToList().ForEach(x => { x.IsChecked = false; });

            if (subscriptions != null)
            {
                subscriptions.Split(',').ToList().ForEach(x =>
                {
                    try
                    {
                        CheckBox checkbox = (CheckBox)FindName("cbx" + x);
                        checkbox.IsChecked = true;
                    }
                    catch
                    {
                        //do nothing
                    }
                });
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            if (tabHospitals.IsSelected)
            {
                Json.selectedTab = 0;
                ChangeAccent(Json.selectedTab);
            }
            else if (tabSubscribers.IsSelected)
            {
                Json.selectedTab = 1;
                ChangeAccent(Json.selectedTab);
            }
        }

        //Method used to change app's accent depending on which tab selected
        private void ChangeAccent(int tab)
        {
            switch (tab)
            {
                case 0:
                    ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.Accents.First(x => x.Name == "Crimson"),
                        ThemeManager.AppThemes.First(x => x.Name == "BaseLight"));
                    break;
                case 1:
                    ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.Accents.First(x => x.Name == "Mauve"),
                        ThemeManager.AppThemes.First(x => x.Name == "BaseLight"));
                    break;
            }
        }

        private void lstHospitals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstHospitals.SelectedItem != null)
            {
                KeyValuePair<string, Hospital> selectedHospital = (KeyValuePair<string, Hospital>)lstHospitals.SelectedItem;
                lblHospitalName.Content = selectedHospital.Value.Name;
                lblHospitalAddressLine1.Content = selectedHospital.Value.AddressLine1;
                lblHospitalAddressLine2.Content = selectedHospital.Value.AddressLine2;
                lblHospitalPostcode.Content = selectedHospital.Value.Postcode;
                Hospital.selectedHospital = lstHospitals.SelectedIndex;
            }
        }

        private void lstSubscribers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSubscribers.SelectedItem != null)
            {
                KeyValuePair<string, Subscriber> selectedSubscriber = (KeyValuePair<string, Subscriber>)lstSubscribers.SelectedItem;
                lblSubscriberName.Content = selectedSubscriber.Value.Name;
                lblSubscriberEmail.Content = selectedSubscriber.Value.Email;
                lblSubsciberActivated.Content = selectedSubscriber.Value.Activated.ToString();
                SubscriptionCheckboxes(selectedSubscriber.Value.Subscriptions);
                Subscriber.selectedSubscriber = lstSubscribers.SelectedIndex;
            }
        }

        private void btnAddHospital_Click(object sender, RoutedEventArgs e)
        {
            AddHospitalWindow addHospitalWindow = new AddHospitalWindow();
            addHospitalWindow.Show();
            this.Close();
        }

        private void btnEditHospital_Click(object sender, RoutedEventArgs e)
        {
            if (lstHospitals.SelectedItem != null)
            {
                KeyValuePair<string, Hospital> selectedHospital = (KeyValuePair<string, Hospital>)lstHospitals.SelectedItem;
                EditHospitalWindow editHospitalWindow = new EditHospitalWindow(selectedHospital.Value);
                editHospitalWindow.Show();
                this.Close();
            }
        }

        private async void btnDeleteHospital_Click(object sender, RoutedEventArgs e)
        {
            if (lstHospitals.SelectedItem != null)
            {
                MessageDialogResult choice = await this.ShowMessageAsync("Delete",
                    "Are you sure you want to delete this hospital?",
                    MessageDialogStyle.AffirmativeAndNegative);
                if (choice == MessageDialogResult.Affirmative)
                {
                    KeyValuePair<string, Hospital> selectedHospital = (KeyValuePair<string, Hospital>)lstHospitals.SelectedItem;
                    Json.hospitals.Remove(selectedHospital.Key);
                    Json.SaveHospitals(Json.hospitals);
                    CollectionViewSource.GetDefaultView(Json.hospitals).Refresh();
                    lstHospitals.SelectedIndex = 0;
                }
            }
        }

        private void btnAddSubscriber_Click(object sender, RoutedEventArgs e)
        {
            AddSubscriberWindow addSubscriberWindow = new AddSubscriberWindow();
            addSubscriberWindow.Show();
            this.Close();
        }

        private void btnEditSubscriber_Click(object sender, RoutedEventArgs e)
        {
            if (lstSubscribers.SelectedItem != null)
            {
                KeyValuePair<string, Subscriber> selectedSubscriber = (KeyValuePair<string, Subscriber>)lstSubscribers.SelectedItem;
                EditSubscriberWindow editSubscriberWindow = new EditSubscriberWindow(selectedSubscriber.Value);
                editSubscriberWindow.Show();
                this.Close();
            }
        }

        private async void btnDeleteSubscriber_Click(object sender, RoutedEventArgs e)
        {
            if (lstSubscribers.SelectedItem != null)
            {
                MessageDialogResult choice = await this.ShowMessageAsync("Delete",
                    "Are you sure you want to delete this subscriber?",
                    MessageDialogStyle.AffirmativeAndNegative);
                if (choice == MessageDialogResult.Affirmative)
                {
                    KeyValuePair<string, Subscriber> selectedSubscriber = (KeyValuePair<string, Subscriber>)lstSubscribers.SelectedItem;
                    Json.subscribers.Remove(selectedSubscriber.Key);
                    Json.SaveSubscribers(Json.subscribers);
                    CollectionViewSource.GetDefaultView(Json.subscribers).Refresh();
                    lstSubscribers.SelectedIndex = 0;
                }
            }
        }

        private async void CreateReport(string path)
        {
            try
            {
                PdfReader reader = new PdfReader("../../Resources/testform.pdf");
                PdfStamper stamp = new PdfStamper(reader, new FileStream(path, FileMode.Create));

                //Put together all fields in pdf file
                AcroFields form = stamp.AcroFields;

                //First string is field name, second is input
                form.SetField("prof name", "Test Prof Name");
                form.SetField("your name", "Test My Name");
                stamp.Close();

                await this.ShowMessageAsync("Report Success", "Report created successfully!");
            }
            catch
            {
                //Most common issue for report not producing is that previous file is already open
                await this.ShowMessageAsync("Report Failure", "Report failed to create!");
            }
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            //Allow user to choose save location for report
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = "Choose Report Save Location";
            saveDialog.FileName = "Report";
            saveDialog.Filter = "PDF document (*.pdf)|*.pdf";
            bool? result = saveDialog.ShowDialog();

            if (result == true)
            {
                CreateReport(saveDialog.FileName);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
