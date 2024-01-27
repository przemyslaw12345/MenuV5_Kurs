
using System.Text.Json;
using System.Xml.Linq;

internal class App : IApp
{
	private readonly MenuDbContext _menuDbContext;
	private readonly IUserInterface _userInterface;
	private readonly IEventHandlerInterface _eventHandlerInterface;
	private readonly IRepository<Meal> _mealRepository;
	private readonly IRepository<Drink> _drinkRepository;

	public App(
		MenuDbContext menuDbContext,
		IRepository<Meal> mealRepository, 
		IRepository<Drink> drinkRepository,
		IUserInterface userInterface,
		IEventHandlerInterface eventHandlerInterface
		)
	{
		
		_mealRepository = mealRepository;
		_drinkRepository = drinkRepository;
		_userInterface = userInterface;
		_eventHandlerInterface = eventHandlerInterface;
		_menuDbContext = menuDbContext;
		_menuDbContext.Database.EnsureCreated();
	}
	public void Run()
	{
		_mealRepository.AddedItem += _eventHandlerInterface.AddEventItemToList;
		_mealRepository.RemovedItem += _eventHandlerInterface.RemoveEventItemToList;

		_drinkRepository.AddedItem += _eventHandlerInterface.AddEventItemToList;
		_drinkRepository.RemovedItem += _eventHandlerInterface.RemoveEventItemToList;

		_userInterface.Run();
	}
}

