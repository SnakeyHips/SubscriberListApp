using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DonorListApp.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace DonorListApp.Views
{

    public partial class EditSubscriberWindow : MetroWindow
    {
        Subscriber Subscriber;

        public EditSubscriberWindow(Subscriber selectedSubscriber)
        {
            InitializeComponent();
            Subscriber = selectedSubscriber;
            cboActivated.Items.Add("Yes");
            cboActivated.Items.Add("No");
            txtSubscriberName.Text = Subscriber.Name;
            txtSubscriberEmail.Text = Subscriber.Email;
            if (Subscriber.Activated.Equals("No"))
            {
                cboActivated.SelectedIndex = 1;
            }
            SubscriptionCheckboxes(Subscriber.Subscriptions);
        }

        private void SubscriptionCheckboxes(string subscriptions)
        {
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

        private string GetSubscriptions()
        {
            string subscriptions = "";
            grdEditSubscriber.FindChildren<CheckBox>().ToList().ForEach(x =>
            {
                if (x.IsChecked == true)
                {
                    subscriptions += x.Content + ",";
                }
            });
            return subscriptions;
        }

        private async void btnEditSubscriber_Click(object sender, RoutedEventArgs e)
        {
            //Make sure each input had an input before being made
            if (txtSubscriberEmail.Text == "")
            {
                await this.ShowMessageAsync("Missing Details", "Please enter the subscriber's email address");
            }
            else
            {
                Subscriber.Email = txtSubscriberEmail.Text;
                Subscriber.Activated = cboActivated.Text;
                Subscriber.Subscriptions = GetSubscriptions();
                Json.subscribers[Subscriber.Name] = Subscriber;
                Json.SaveSubscribers(Json.subscribers);
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
