using TaskList.Contracts;
using TaskList.Repository;

namespace TaskList.CommandAndQuary.Quries;

public class TodayQuery : IQuery
{

    public string Run(ITaskRepository tasksRepo)
    {
        var rs = "";
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        foreach (var project in tasksRepo.GetAll().Where((keyValue) => keyValue.Tasks.Any(task => task.Deadline == today)).ToList())
        {
            rs += project.Name + Environment.NewLine;
            foreach (var task in project.Tasks.Where(task => task.Deadline == today))
            {
                rs += string.Format("    [{0}] {1}: {2}{3}", task.Done ? 'x' : ' ', task.Id, task.Description, task.Deadline != null ? $" {task.Deadline?.ToString("dd-MM-yyyy")}" : "") + Environment.NewLine;
            }
            rs += Environment.NewLine;
        }
        return rs;
    }


}
