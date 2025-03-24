using TaskList.Repository;

namespace TaskList.Contracts;


public interface ICommand
{
    void Execute(ITaskRepository tasksRepo);
}
