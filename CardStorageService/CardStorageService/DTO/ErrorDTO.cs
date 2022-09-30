namespace CardStorageService.DTO
{
    public class ErrorDTO
    {
        public int Code { get; set; } = 500;
        public string Message { get; set; } = "Внутренняя ошибка при работе с данными. Пожалуйста, проверьте параметры запроса";
    }
}
