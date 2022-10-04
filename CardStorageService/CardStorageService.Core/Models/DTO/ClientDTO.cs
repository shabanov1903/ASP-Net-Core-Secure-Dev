namespace CardStorageService.Core.Models.DTO
{
    public class ClientDTO : IError
    {
        public int? ClientId { get; set; }
        public string? Surname { get; set; }
        public string? FirstName { get; set; }
        public string? Patronymic { get; set; }

        public int? Code { get; set; }
        public string? Message { get; set; }
    }
}
