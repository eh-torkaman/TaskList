
using TaskList.Repository;
namespace TaskList.Commands;

public class ViewByDeadlineCommand : ICommand
{
    private readonly ITaskRepository tasksRepo;
    private readonly IConsole console;

    public ViewByDeadlineCommand(ITaskRepository tasksRepo, IConsole console)
    {
        this.tasksRepo = tasksRepo;
        this.console = console;

    }

    public void Execute()
    {
        // Group by deadline first, then by project

        var groupedTasks = tasksRepo.GetAll()
            .SelectMany(kvp => kvp.Value.Select(task => new { Project = kvp.Key, Task = task }))
            .GroupBy(item => item.Task.Deadline)
            .OrderBy(group => group.Key ?? DateOnly.MaxValue); // Null (no deadline) goes last

        foreach (var dateGroup in groupedTasks)
        {
            console.WriteLine($"{dateGroup.Key?.ToString("dd-MM-yyyy") ?? "No deadline"}:");

            foreach (var projectGroup in dateGroup.GroupBy(item => item.Project))
            {
                console.WriteLine($"   {projectGroup.Key}:");

                foreach (var task in projectGroup)
                {
                    console.WriteLine($"      {task.Task.Id}: {task.Task.Description}");
                }
            }
        }
    }

}
