using TaskList.Repository;

namespace TaskList.Contracts;

public interface IQuery
{
    string Run(ITaskRepository tasksRepo);
}
