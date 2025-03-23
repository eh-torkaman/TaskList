using TaskList.Repository;

namespace TaskList.Commands;

public class CommandFactory
{
    private readonly ITaskRepository tasksRepo;
    private readonly IConsole console;

    public CommandFactory(ITaskRepository tasksRepo, IConsole console)
    {
        this.tasksRepo = tasksRepo;
        this.console = console;
    }

    public ICommand CreateCommand(string commandLine)
    {
        var commandRest = commandLine.Split(" ", 2);
        var command = commandRest[0];

        switch (command)
        {
            case "show":
                return new ShowCommand(tasksRepo, console);
            case "today":
                return new TodayCommand(tasksRepo, console);
            case "add":
                var subcommandRest = commandRest[1].Split(" ", 2);
                if (subcommandRest[0] == "project")
                {
                    return new AddProjectCommand(tasksRepo, subcommandRest[1]);
                }
                else if (subcommandRest[0] == "task")
                {
                    var projectTask = subcommandRest[1].Split(" ", 2);
                    return new AddTaskCommand(tasksRepo, projectTask[0], projectTask[1]);
                }
                break;
            case "check":
                return new CheckCommand(tasksRepo, int.Parse(commandRest[1]), true);
            case "uncheck":
                return new CheckCommand(tasksRepo, int.Parse(commandRest[1]), false);

            case "deadline":
                var taskId = int.Parse(commandRest[1].Split(" ", 2)[0]);
                var deadline = DateOnly.Parse(commandRest[1].Split(" ", 2)[1]);
                return new TaskDeadlineCommand(tasksRepo, taskId, deadline);


            case "help":
                return new HelpCommand(console);


        }
        throw new TaskOperationException(string.Format("I don't know what the command \"{0}\" is.", command));
    }
}
