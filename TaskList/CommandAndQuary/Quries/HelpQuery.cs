using TaskList.Contracts;
using TaskList.Repository;

namespace TaskList.CommandAndQuary.Quries;

public class HelpQuery : IQuery
{
    public string Run(ITaskRepository tasksRepo)
    {
        var rs = "";
        rs += "Commands:" + Environment.NewLine;
        rs += "  show" + Environment.NewLine;
        rs += "  add project <project name>" + Environment.NewLine;
        rs += "  add task <project name> <task description>" + Environment.NewLine;
        rs += "  check <task ID>" + Environment.NewLine;
        rs += "  uncheck <task ID>" + Environment.NewLine;
        rs += "  deadline <ID> <date>" + Environment.NewLine;
        rs += "  today" + Environment.NewLine;
        rs += "  view-by-deadline" + Environment.NewLine;
        rs += Environment.NewLine;
        return rs;
    }



}
