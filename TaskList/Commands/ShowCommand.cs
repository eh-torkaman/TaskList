
using TaskList.Repository;

namespace TaskList.Commands;

public class ShowCommand : ICommand
{
    private readonly IConsole console;
    private readonly ITaskRepository tasksRepo;

    public ShowCommand(ITaskRepository tasksRepo, IConsole console)
    {
        this.tasksRepo = tasksRepo;
        this.console = console;

    }

    public void Execute()
    {
        foreach (var project in tasksRepo.GetAll())
        {
            console.WriteLine(project.Key);
            foreach (var task in project.Value)
            {
                console.WriteLine("    [{0}] {1}: {2}{3}", (task.Done ? 'x' : ' '), task.Id, task.Description, task.Deadline != null ? $" {task.Deadline?.ToString("dd-MM-yyyy")}" : "");
            }
            console.WriteLine();
        }
    }

}
