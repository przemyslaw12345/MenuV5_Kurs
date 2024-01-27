using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

string connectionString = "Data Source=DESKTOP-GA3KCB6;Initial Catalog=MenuV5;Integrated Security=True;Trust Server Certificate=True";

var service = new ServiceCollection();
service.AddDbContext<MenuDbContext>(
	options => options.UseSqlServer(connectionString));
service.AddScoped<IApp, App>();
service.AddScoped<IUserInterface, UserInterface>();
service.AddScoped<IEventHandlerInterface, EventHandlerClass>();
service.AddScoped<IRepository<Meal>, MenuSqlRepository<Meal>>();
service.AddScoped<IRepository<Drink>, MenuSqlRepository<Drink>>();

var serviceProvider = service.BuildServiceProvider();
var app = serviceProvider.GetRequiredService<IApp>();
app.Run();
Console.ReadLine();