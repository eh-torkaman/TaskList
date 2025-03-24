namespace TaskList.Exceptions;

public class TaskOperationException : Exception
{
    public TaskOperationException() : base()
    {

    }
    public TaskOperationException(string msg) : base(msg)
    {

    }
}
