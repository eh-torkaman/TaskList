using TaskList.Contracts;
using TaskList.Repository;

namespace TaskList.CommandAndQuary.Commands;

public class CheckCommand : ICommand
{
    private readonly int taskId;
    private readonly bool done;

    public CheckCommand(int taskId, bool done)
    {

        this.taskId = taskId;
        this.done = done;
    }

    public void Execute(ITaskRepository tasksRepo)
    {

        tasksRepo.SetTaskDone(taskId, done);


    }

}
