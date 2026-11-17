namespace CsvHandler;

public class HandlerExceptions : Exception
{
    public HandlerExceptions(string message) : base(message) { }
    public HandlerExceptions(string message, Exception inner_exception) : base(message, inner_exception) { }

    public class InvalidCSVException : HandlerExceptions
    {
        public InvalidCSVException(string message = "") : base($"Invalid CSV format: {message}") { }
    }

    public class InvalidArgumentException : HandlerExceptions
    {
        public InvalidArgumentException(string message = "") : base($"Invalid argument: {message}") { }
    }
}