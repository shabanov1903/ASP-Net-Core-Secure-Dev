namespace CardStorageService.Core.Models
{
    public class AccountInfo
    {
        public int AccountId { get; set; }
        public string EMail { get; set; }
        public bool Locked { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
    }
}
