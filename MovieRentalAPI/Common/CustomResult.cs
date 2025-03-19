namespace MovieRentalAPI.Common
{
    public class CustomResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
