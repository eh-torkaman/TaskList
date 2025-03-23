using TaskList;
using TaskList.Repository;

if (args.Length > 0)
{
    TaskList.TaskList.Main(args);
}
else
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers(); // Register controllers


    //It depends on how we want to treat TaskList: is it our orchestrator? Do we intend to implement undo,
    //store state, etc.? And do we want it to be our interface to the underlying data?
    builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
    builder.Services.AddSingleton<IConsole, RealConsole>();
    builder.Services.AddSingleton<TaskList.TaskList>();


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers(); // Map controllers

    app.Run();
}