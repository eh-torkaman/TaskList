using TaskList.Contracts;
using TaskList.Repository;

namespace TaskList.CommandAndQuary.Commands;

public class AddProjectCommand : ICommand
{
    private readonly string projectName;

    public AddProjectCommand(string projectName)
    {
        this.projectName = projectName;
    }

    public void Execute(ITaskRepository tasksRepo)
    {

        tasksRepo.AddProject(projectName);

    }


}
