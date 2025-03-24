using TaskList.Contracts;
using TaskList.Repository;
namespace TaskList.CommandAndQuary.Quries;

public class ViewByDeadlineQuery : IQuery
{
    public string Run(ITaskRepository tasksRepo)
    {
        var rs = "";
        var result = tasksRepo.GetGroupedTaskListByDeadLine();

        foreach (var group in result)
        {
            rs += $"{group.Deadline}:" + Environment.NewLine;

            foreach (var project in group.GroupdProjectTasks)
            {
                rs += $"   {project.Project}:" + Environment.NewLine;

                foreach (var task in project.Task)
                {
                    rs += $"      {task.Id}: {task.Description}" + Environment.NewLine;
                }
            }
        }
        return rs;
    }
}
