namespace TaskList.Commands;

public class HelpCommand : ICommand
{
    private readonly IConsole console;

    public HelpCommand(IConsole console)
    {
        this.console = console;
    }



    public void Execute()
    {
        Help();
    }
    private void Help()
    {
        console.WriteLine("Commands:");
        console.WriteLine("  show");
        console.WriteLine("  add project <project name>");
        console.WriteLine("  add task <project name> <task description>");
        console.WriteLine("  check <task ID>");
        console.WriteLine("  uncheck <task ID>");
        console.WriteLine();
    }

}
