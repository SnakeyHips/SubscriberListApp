using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DonorListApp.Models
{
    public class Json
    {

        public Json()
        {
        }

        //App's storage of the json data
        public static Dictionary<string, Hospital> hospitals { get; set; }
        public static Dictionary<string, Subscriber> subscribers { get; set; }

        public static int selectedTab = 0;

        //Location of json files
        public static string hospitalPath = "Resources/Hospitals.json";
        public static string subscriberPath = "Resources/Subscribers.json";

        //Retrieves Dictionary of Hospitals from json file
        public static Dictionary<string, Hospital> GetHospitals()
        {
            using (StreamReader file = File.OpenText(hospitalPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (Dictionary<string, Hospital>)serializer.Deserialize(file, typeof(Dictionary<string, Hospital>));
            }
        }

        //Updates json file with Dictionary input
        public static void SaveHospitals(Dictionary<string, Hospital> hospitals)
        {
            using (StreamWriter file = File.CreateText(hospitalPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, hospitals);
            }
        }

        //Retrieves Dictionary of Subscribers from json file
        public static Dictionary<string, Subscriber> GetSubscribers()
        {
            using (StreamReader file = File.OpenText(subscriberPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (Dictionary<string, Subscriber>)serializer.Deserialize(file, typeof(Dictionary<string, Subscriber>));
            }
        }

        //Updates json file with Dictionary input
        public static void SaveSubscribers(Dictionary<string, Subscriber> subscribers)
        {
            using (StreamWriter file = File.CreateText(subscriberPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, subscribers);
            }
        }
    }

}

