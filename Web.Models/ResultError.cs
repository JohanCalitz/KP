namespace Web.Models
{
    public class ResultError
    {
        public ResultError(string? message)
        {
            _message = message;
        }

        internal string? _message;

        /// <summary>
        /// An error message to respond to the user with.
        /// </summary>
        public virtual string Message => _message ?? "An error occured!";
    }
}
