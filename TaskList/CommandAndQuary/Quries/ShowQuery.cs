using TaskList.Contracts;
using TaskList.Repository;

namespace TaskList.CommandAndQuary.Quries;

public class ShowQuery : IQuery
{
    public string Run(ITaskRepository tasksRepo)
    {
        var rs = "";
        foreach (var project in tasksRepo.GetAll())
        {
            rs += project.Name + Environment.NewLine;
            foreach (var task in project.Tasks)
            {
                rs += string.Format("    [{0}] {1}: {2}{3}", task.Done ? 'x' : ' ', task.Id, task.Description, task.Deadline != null ? $" {task.Deadline?.ToString("dd-MM-yyyy")}" : "") + Environment.NewLine;
            }
            rs += Environment.NewLine;
        }
        return rs;
    }

}
