using TaskList.CommandAndQuary.Commands;
using TaskList.Contracts;
using TaskList.Exceptions;

namespace TaskList.CommandAndQuaryFactory;

public class CommandFactory
{
    public ICommand CreateCommand(string commandLine)
    {
        var commandRest = commandLine.Split(" ", 2);
        var command = commandRest[0];

        switch (command)
        {
            case "add":
                var subcommandRest = commandRest[1].Split(" ", 2);
                if (subcommandRest[0] == "project")
                {
                    return new AddProjectCommand(subcommandRest[1]);
                }
                else if (subcommandRest[0] == "task")
                {
                    var projectTask = subcommandRest[1].Split(" ", 2);
                    return new AddTaskCommand(projectTask[0], projectTask[1]);
                }
                break;
            case "check":
                return new CheckCommand(int.Parse(commandRest[1]), true);
            case "uncheck":
                return new CheckCommand(int.Parse(commandRest[1]), false);

            case "deadline":
                var taskId = int.Parse(commandRest[1].Split(" ", 2)[0]);
                var deadline = DateOnly.ParseExact(commandRest[1].Split(" ", 2)[1], "dd-MM-yyyy");
                return new TaskDeadlineCommand(taskId, deadline);


        }
        throw new UnknownCommandException(string.Format("I don't know what the command \"{0}\" is.", command));
    }
}
