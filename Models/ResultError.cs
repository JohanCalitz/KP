namespace API.Models
{
    public class ResultError
    {
        public ResultError(string? message)
        {
            _message = message;
        }

        internal string? _message;

        public virtual string Message => _message ?? "An error occured!";
    }
}
