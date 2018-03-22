namespace DonorListApp.Models
{
    public class Hospital
    {

        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Postcode { get; set; }

        public static int selectedHospital = 0;

        public Hospital(string name, string addressline1, string addressline2, string postcode)
        {
            this.Name = name;
            this.AddressLine1 = addressline1;
            this.AddressLine2 = addressline2;
            this.Postcode = postcode;
        }

    }
}
