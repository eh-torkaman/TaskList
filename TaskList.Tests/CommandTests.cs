using TaskList.Commands;
using TaskList.Repository;
namespace Tasks;


[TestFixture]
public sealed class CommandTests
{

    private ITaskRepository repo;

    [SetUp]
    public void SetUp()
    {
        //or a mock..
        this.repo = new TaskRepository();
    }

    [TearDown]
    public void TearDown()
    {
    }

    [Test]
    public void AddProjectCommand()
    {
        var command = new AddProjectCommand(repo, "Training");
        command.Execute();
        Assert.True(repo.ContainsProject("Training"));
    }

    [Test]
    public void AddDuplicatedProject_ThorwException()
    {
        ICommand command = new AddProjectCommand(repo, "Training");
        command.Execute();
        Assert.True(repo.ContainsProject("Training"));
        Assert.Throws<TaskOperationException>(() => command.Execute());
    }

    [Test]
    public void AddTaskCommandToNoexistingProject()
    {
        ICommand command = new AddTaskCommand(repo, "Training", "Task1");
        Assert.Throws<TaskOperationException>(() => command.Execute());
    }

    [Test]
    public void AddTaskCommand()
    {
        ICommand command = new AddProjectCommand(repo, "Training");
        command.Execute();
        command = new AddTaskCommand(repo, "Training", "Task1");
        command.Execute();

        Assert.True(repo.GetTasksByProject("Training").Any(x => x.Description == "Task1"));
    }


    [Test]
    public void AddTaskDeadlineCommand()
    {
        ICommand command = new AddProjectCommand(repo, "Training");
        command.Execute();
        command = new AddTaskCommand(repo, "Training", "Task1");
        command.Execute();

        command = new TaskDeadlineCommand(repo, 1, new DateOnly());
        command.Execute();
        Assert.True(repo.GetTasksByProject("Training").Any(x => x.Description == "Task1" && x.Deadline == new DateOnly()));
    }


    [Test]
    public void AddCheckCommand()
    {
        ICommand command = new AddProjectCommand(repo, "Training");
        command.Execute();

        command = new AddTaskCommand(repo, "Training", "Task1");
        command.Execute();

        command = new CheckCommand(repo, 1, true);
        command.Execute();

        Assert.True(repo.GetTasksByProject("Training").Any(x => x.Description == "Task1" && x.Done));

        command = new CheckCommand(repo, 1, false);
        command.Execute();

        Assert.True(repo.GetTasksByProject("Training").Any(x => x.Description == "Task1" && !x.Done));
    }
}