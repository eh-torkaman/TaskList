using TaskList.Repository;

namespace TaskList.Commands;

public class TodayCommand : ICommand
{
    private readonly IConsole console;
    private readonly ITaskRepository tasksRepo;

    public TodayCommand(ITaskRepository tasksRepo, IConsole console)
    {
        this.tasksRepo = tasksRepo;
        this.console = console;

    }

    public void Execute()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        foreach (var project in tasksRepo.GetAll().Where((keyValue) => keyValue.Value.Any(task => task.Deadline == today)).ToList())
        {
            console.WriteLine(project.Key);
            foreach (var task in project.Value.Where(task => task.Deadline == today))
            {
                console.WriteLine("    [{0}] {1}: {2}{3}", (task.Done ? 'x' : ' '), task.Id, task.Description, task.Deadline != null ? $" {task.Deadline?.ToString("dd-MM-yyyy")}" : "");
            }
            console.WriteLine();
        }
    }
}
