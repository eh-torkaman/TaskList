using TaskList.CommandAndQuaryFactory;
using TaskList.Contracts;
using TaskList.Exceptions;
using TaskList.Repository;

namespace TaskList
{
    public sealed class TaskList
    {

        // todo implement undo ...
        //private IList<ICommand> _commandHistory= new List<ICommand>();
        // save state....
        private const string QUIT = "quit";
        public static readonly string startupText = "Welcome to TaskList! Type 'help' for available commands.";

        private readonly IConsole console;
        private readonly ITaskRepository taskRepo;
        private readonly CommandFactory commandFactory = new CommandFactory();
        private readonly QueryFactory queryFactory = new QueryFactory();

        public static void Main(string[] args)
        {
            new TaskList(new RealConsole(), new TaskRepository()).Run();
        }

        public TaskList(IConsole console, ITaskRepository taskRepo)
        {
            this.console = console;
            this.taskRepo = taskRepo;
        }

        public void Run()
        {
            console.WriteLine(startupText);
            while (true)
            {
                console.Write("> ");
                var command = console.ReadLine();
                if (command == QUIT)
                {
                    break;
                }
                if (!string.IsNullOrWhiteSpace(command))
                {
                    Execute(command);
                }
            }
        }

        public void Execute(string commandLine)
        {
            try
            {
                var command = commandFactory.CreateCommand(commandLine);
                ExecuteCommand(command);
            }
            catch (UnknownCommandException ex)
            {
                var query = queryFactory.CreateQuery(commandLine);
                ExecuteQuery(query);

            }
            catch (Exception ex)
            {
                console.WriteLine("Error!=> " + ex.Message);
            }

        }


        public void ExecuteQuery(IQuery query)
        {
            console.Write(query.Run(taskRepo));
        }

        public void ExecuteCommand(ICommand command)
        {
            try
            {
                command.Execute(taskRepo);
                //push command to _commandHistory
            }
            catch (TaskOperationException ex)
            {
                console.WriteLine(ex.Message);
            }
        }


        //public void UndoLastCommand()
        //{
        //    //var lastCommand = _commandHistory.Last();
        //    //lastCommand.Undo(taskRepo);
        //    //_commandHistory.Remove(lastCommand);
        //}
        public ITaskRepository Repo => this.taskRepo;


    }
}
