namespace TaskList.Exceptions;

public class UnknownQueryException : Exception
{
    public UnknownQueryException() : base()
    {

    }
    public UnknownQueryException(string msg) : base(msg)
    {

    }
}