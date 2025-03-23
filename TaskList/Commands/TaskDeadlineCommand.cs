
using TaskList.Repository;
namespace TaskList.Commands;

public class TaskDeadlineCommand : ICommand
{
    private readonly ITaskRepository tasksRepo;

    private readonly int taskId;
    private readonly DateOnly deadline;


    public TaskDeadlineCommand(ITaskRepository tasksRepo, int taskId, DateOnly deadline)
    {
        this.tasksRepo = tasksRepo;
        this.taskId = taskId;
        this.deadline = deadline;
    }

    public void Execute()
    {
        tasksRepo.SetTaskDeadline(taskId, deadline);
    }

}
