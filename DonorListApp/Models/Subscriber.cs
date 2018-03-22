namespace DonorListApp.Models
{
    public class Subscriber
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public string Activated { get; set; }
        public string Subscriptions { get; set; }

        public static int selectedSubscriber = 0;

        public Subscriber(string name, string email, string activated, string subscriptions)
        {
            this.Name = name;
            this.Email = email;
            this.Activated = activated;
            this.Subscriptions = subscriptions;
        }

    }
}
