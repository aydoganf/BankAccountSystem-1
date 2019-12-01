namespace AccountApp.Models.Operation.Company
{
    public class CompanyAddressChange
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }

        public CompanyAddressChange()
        {
        }

        public CompanyAddressChange(string companyName, string address)
        {
            CompanyName = companyName;
            Address = address;
        }
    }
}