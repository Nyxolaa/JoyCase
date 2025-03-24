namespace JoyCase.Service
{
    public class Response<T>
    {
        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }

        public string FriendlyMessage { get; set; }

        public T Data { get; set; }

        public static Response<T> Success(T data, string friendlyMessage = "")
        {
            return new Response<T>
            {
                Data = data,
                FriendlyMessage = friendlyMessage,
                Succeeded = true
            };
        }

        public static Response<T> Failure(IEnumerable<string> errors)
        {
            return new Response<T>
            {
                Errors = errors.ToArray(),
                Succeeded = false
            };
        }
    }
}
