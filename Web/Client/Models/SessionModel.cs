using System;

namespace NetBoard.Models
{
    [Serializable]
    public class SessionModel
    {
        public string AccessToken { get; set; }
        public string AzureADID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string LocalUserName { get; set; }
        public string SelectionType { get; set; }
        public string UserIsNew { get; set; }
        public bool isNull { get; set; }
        public string FirstInitial
        {
            get { return FirstName != null ? FirstName.Substring(0, 1) : "";  }
        }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public bool UserIsLoggedIn
        {
            get { return !string.IsNullOrWhiteSpace(AzureADID); }
        }
    }
}
