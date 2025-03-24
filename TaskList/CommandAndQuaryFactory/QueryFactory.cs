using TaskList.CommandAndQuary.Quries;
using TaskList.Contracts;
using TaskList.Exceptions;

namespace TaskList.CommandAndQuaryFactory;

public class QueryFactory
{
    public IQuery CreateQuery(string commandLine)
    {
        var commandRest = commandLine.Split(" ", 2);
        var command = commandRest[0];

        switch (command)
        {
            case "show":
                return new ShowQuery();
            case "today":
                return new TodayQuery();
            case "view-by-deadline":
                return new ViewByDeadlineQuery();
            case "help":
                return new HelpQuery();


        }
        throw new UnknownQueryException(string.Format("I don't know what the Query \"{0}\" is.", command));
    }
}
