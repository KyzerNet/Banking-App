namespace Response
{
    public class ResponseApi<T>
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public T Data { get; set; }
    }
}
