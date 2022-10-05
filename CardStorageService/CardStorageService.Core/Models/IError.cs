namespace CardStorageService.Core.Models
{
    public interface IError
    {
        public int? Code { get; set; }
        public string? Message { get; set; }
    }
}
