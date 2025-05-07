namespace TodoManagementApplication.Application.Core
{
    public class BaseModel<T> where T : class
    {
        public T? Data { get; set; }
        public IEnumerable<T>? DataList { get; set; }
        public string? Message { get; set; }
        public bool? Success { get; set; }
        public string? Token { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
        public Exception? Exception { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public int? totalPages { get; set; }
    }
}
