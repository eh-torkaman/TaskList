using Microsoft.AspNetCore.Mvc;
using TaskList.Commands;
using TaskList.Dtos;

namespace TaskList;

[ApiController]
[Route("projects")]
public class TaskController : ControllerBase
{
    public TaskController(TaskList taskList)
    {
        TaskList = taskList;
    }

    [HttpGet()]
    public ActionResult<IDictionary<string, IList<Task>>> GetAllTasks()
    {
        return Ok(TaskList.Repo.GetAll());
    }


    [HttpGet("view_by_deadline")]
    public ActionResult<IList<GroupedTaskByDeadlineDto>> GetViewByDeadline()
    {
        return Ok(TaskList.Repo.GetGroupedTaskListByDeadLine());
    }

    [HttpPost]
    public ActionResult CreateProject([FromBody] string project_id)
    {
        var command = new AddProjectCommand(TaskList.Repo, project_id);
        command.Execute();
        return Ok();
    }
    [HttpPost("{project_id}/tasks")]
    public ActionResult CreateTask([FromBody] string taskDescription, [FromRoute] string project_id)
    {
        var command = new AddTaskCommand(TaskList.Repo, project_id, taskDescription);
        command.Execute();
        return Ok();
    }

    [HttpPut("{project_id}/tasks/{task_id}")]
    public ActionResult SetTaskDeadline([FromQuery] DateOnly deadline, [FromRoute] string project_id, [FromRoute] int task_id)
    {
        var command = new TaskDeadlineCommand(TaskList.Repo, task_id, deadline);
        command.Execute();
        return Ok();
    }


    private TaskList TaskList { get; }

}
