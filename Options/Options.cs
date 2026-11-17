namespace CsvHandler.Options;

public class Delimiter
{
    private Delimiter(string value) { Value = value; }
    public string Value { get; private set; }

    public static Delimiter Comma { get { return new Delimiter(","); } }
    public static Delimiter Tab { get { return new Delimiter("\t"); } }
    public static Delimiter Semicolon { get { return new Delimiter(";"); } }
    public static Delimiter Pipe { get { return new Delimiter("|"); } }
}

public class NewLine
{
    private NewLine(string value) { Value = value; }
    public string Value { get; private set; }

    public static NewLine CRLF { get { return new NewLine("\r\n"); } }
    public static NewLine LF { get { return new NewLine("\n"); } }
    public static NewLine CR { get { return new NewLine("\r"); } }
}

public enum WriteMode
{
    Overwrite,
    Append
}