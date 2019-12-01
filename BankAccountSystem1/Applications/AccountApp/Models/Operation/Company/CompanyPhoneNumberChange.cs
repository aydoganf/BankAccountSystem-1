namespace AccountApp.Models.Operation.Company
{
    public class CompanyPhoneNumberChange
    {
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }

        public CompanyPhoneNumberChange()
        {
        }

        public CompanyPhoneNumberChange(string companyName, string phoneNumber)
        {
            CompanyName = companyName;
            PhoneNumber = phoneNumber;
        }
    }
}