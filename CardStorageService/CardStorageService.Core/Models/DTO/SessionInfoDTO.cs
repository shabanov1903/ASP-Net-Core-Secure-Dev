namespace CardStorageService.Core.Models.DTO
{
    public class SessionInfo : IError
    {
        public int SessionId { get; set; }
        public string SessionToken { get; set; }
        public AccountInfo Account { get; set; }

        public int? Code { get; set; }
        public string? Message { get; set; }
    }
}
