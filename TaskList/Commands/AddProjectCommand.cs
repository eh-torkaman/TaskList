using TaskList.Repository;

namespace TaskList.Commands;

public class AddProjectCommand : ICommand
{
    private readonly ITaskRepository tasksRepo;
    private readonly string projectName;

    public AddProjectCommand(ITaskRepository tasksRepo, string projectName)
    {
        this.tasksRepo = tasksRepo;
        this.projectName = projectName;
    }

    public void Execute()
    {

        tasksRepo.AddProject(projectName);

    }


}
