using System.Windows.Controls;
using System.Windows;
using System.Linq;
using DonorListApp.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace DonorListApp.Views
{

    public partial class AddSubscriberWindow : MetroWindow
    {
        public AddSubscriberWindow()
        {
            InitializeComponent();
            cboActivated.Items.Add("Yes");
            cboActivated.Items.Add("No");
        }

        private async void btnAddSubscriber_Click(object sender, RoutedEventArgs e)
        {
            //Make sure each input had an input before being made
            if (txtSubscriberName.Text == "")
            {
                await this.ShowMessageAsync("Missing Details", "Please enter the subscriber's name");
            }
            else if (txtSubscriberEmail.Text == "")
            {
                await this.ShowMessageAsync("Missing Details", "Please enter subscriber's email address");
            }
            else
            {
                Subscriber temp = new Subscriber(
                    txtSubscriberName.Text,
                    txtSubscriberEmail.Text,
                    cboActivated.Text,
                    GetSubscriptions());
                try
                {
                    Json.subscribers.Add(temp.Name, temp);
                    Json.SaveSubscribers(Json.subscribers);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                catch
                {
                    await this.ShowMessageAsync("Duplicate Found", "A subscriber of the name " + temp.Name + " already exists!");
                }
            }

        }

        private string GetSubscriptions()
        {
            string subscriptions = "";
            grdAddSubscriber.FindChildren<CheckBox>().ToList().ForEach(x =>
            {
                if (x.IsChecked == true)
                {
                    subscriptions += x.Content + ",";
                }
            });
            return subscriptions;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
