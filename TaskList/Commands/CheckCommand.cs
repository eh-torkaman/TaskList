using TaskList.Repository;

namespace TaskList.Commands;

public class CheckCommand : ICommand
{
    private readonly ITaskRepository tasksRepo;
    private readonly int taskId;
    private readonly bool done;

    public CheckCommand(ITaskRepository tasksRepo, int taskId, bool done)
    {
        this.tasksRepo = tasksRepo;
        this.taskId = taskId;
        this.done = done;
    }

    public void Execute()
    {

        tasksRepo.SetTaskDone(taskId, done);


    }

}
