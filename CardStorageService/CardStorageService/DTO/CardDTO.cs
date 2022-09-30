namespace CardStorageService.DTO
{
    public class CardDTO
    {
        public Guid? CardId { get; set; }
        public int ClientId { get; set; }
        public string CardNo { get; set; }
        public string? Name { get; set; }
        public string? CVV2 { get; set; }
        public DateTime ExpDate { get; set; }
    }
}
