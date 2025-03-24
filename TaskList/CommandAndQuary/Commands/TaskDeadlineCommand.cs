using TaskList.Contracts;
using TaskList.Repository;
namespace TaskList.CommandAndQuary.Commands;

public class TaskDeadlineCommand : ICommand
{

    private readonly int taskId;
    private readonly DateOnly deadline;


    public TaskDeadlineCommand(int taskId, DateOnly deadline)
    {
        this.taskId = taskId;
        this.deadline = deadline;
    }

    public void Execute(ITaskRepository tasksRepo)
    {
        tasksRepo.SetTaskDeadline(taskId, deadline);
    }

}
