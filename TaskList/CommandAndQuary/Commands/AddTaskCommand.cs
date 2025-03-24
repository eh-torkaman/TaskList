using TaskList.Contracts;
using TaskList.Repository;

namespace TaskList.CommandAndQuary.Commands;

public class AddTaskCommand : ICommand
{
    private readonly string projectName;
    private readonly string taskDescription;

    public AddTaskCommand(string projectName, string taskDescription)
    {
        this.projectName = projectName;
        this.taskDescription = taskDescription;
    }

    public void Execute(ITaskRepository tasksRepo)
    {
        tasksRepo.AddTask(projectName, new Task { Id = 0, Description = taskDescription, Done = false });
    }



}
