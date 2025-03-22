using TaskList.Repository;

namespace TaskList.Commands;

public class AddTaskCommand : ICommand
{
    private readonly ITaskRepository tasksRepo;

    private readonly string projectName;
    private readonly string taskDescription;

    public AddTaskCommand(ITaskRepository tasksRepo, string projectName, string taskDescription)
    {
        this.tasksRepo = tasksRepo;
        this.projectName = projectName;
        this.taskDescription = taskDescription;
    }

    public void Execute()
    {
        tasksRepo.AddTask(projectName, new Task { Id = 0, Description = taskDescription, Done = false });
    }



}
