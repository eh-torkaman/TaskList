namespace TaskList.Dtos;
#nullable disable
public class GroupedTaskByDeadlineDto
{
    public string Deadline { get; set; }
    public IList<GroupedTaskByProjectDto> GroupdProjectTasks { get; set; }

}

public class GroupedTaskByProjectDto
{
    public string Project { get; set; }
    public IList<Task> Task { get; set; }
}
