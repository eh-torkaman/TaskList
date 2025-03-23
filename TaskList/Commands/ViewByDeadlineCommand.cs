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

        var result = tasksRepo.GetGroupedTaskListByDeadLine();

        foreach (var group in result)
        {
            console.WriteLine($"{group.Deadline}:");

            foreach (var project in group.GroupdProjectTasks)
            {
                console.WriteLine($"   {project.Project}:");

                foreach (var task in project.Task)
                {
                    console.WriteLine($"      {task.Id}: {task.Description}");
                }
            }
        }

    }

}
