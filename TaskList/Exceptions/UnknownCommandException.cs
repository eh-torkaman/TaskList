namespace TaskList.Exceptions;

public class UnknownCommandException : Exception
{
    public UnknownCommandException() : base()
    {

    }
    public UnknownCommandException(string msg) : base(msg)
    {

    }
}
