namespace CardStorageService.Core.Models.DTO
{
    public class CardDTO : IError
    {
        public Guid? CardId { get; set; }
        public int ClientId { get; set; }
        public string CardNo { get; set; }
        public string? Name { get; set; }
        public string? CVV2 { get; set; }
        public DateTime ExpDate { get; set; }
        
        public int? Code { get; set; }
        public string? Message { get; set; }
    }
}
