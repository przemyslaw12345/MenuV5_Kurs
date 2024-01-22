
using System.Text.Json;
using System.Xml.Linq;

internal class App : IApp
{
	private readonly MenuDbContext _menuDbContext;
	private readonly ICreatingToDatabase _iCreatingToDatabase;
	private readonly IReadingFromDatabase _iReadingFromDatabase;
	private readonly IUpdatingDatabase _iUpdatingDatabase;
	private readonly IDeletingFromDatabase _iDeletingFromDatabase;
	private readonly IRepository<Meal> _mealRepository;
	private readonly IRepository<Drink> _drinkRepository;
	public App(
		MenuDbContext menuDbContext,
		ICreatingToDatabase iCreatingToDatabase,
		IReadingFromDatabase iReadingFromDatabase,
		IUpdatingDatabase iUpdatingDatabase,
		IDeletingFromDatabase iDeletingFromDatabase,
		IRepository<Meal> mealRepository, 
		IRepository<Drink> drinkRepository
		)
	{
		_iCreatingToDatabase = iCreatingToDatabase;
		_iReadingFromDatabase = iReadingFromDatabase;
		_iUpdatingDatabase = iUpdatingDatabase;
		_iDeletingFromDatabase = iDeletingFromDatabase;
		_mealRepository = mealRepository;
		_drinkRepository = drinkRepository;
		_menuDbContext = menuDbContext;
		_menuDbContext.Database.EnsureCreated();
	}
	public void Run()
	{
		_mealRepository.AddedItem += AddEventItemToList;
		_mealRepository.RemovedItem += RemoveEventItemToList;

		_drinkRepository.AddedItem += AddEventItemToList;
		_drinkRepository.RemovedItem += RemoveEventItemToList;

		//Console.WriteLine("Running App");
		bool isWroking = true;
		while (isWroking)
		{
			Console.Clear();
			MenuGreetingMethod();
			string optionSelected = UserStringInputMethod();
			isWroking = SelectingUserChoiceMethod(isWroking, optionSelected);
			Console.ReadKey();
		}
	}
	//--------------------------------------------------------------------------------------------------------------------------------------------------------------
	void AddEventItemToList(object? sender, CafeMenu e)
	{
		const string addedItemEvent = "AddedItemEvent.json";
		List<string> addedItemEventList = new List<string>();
		string jsonAddedItemEventList;
		if (File.Exists(addedItemEvent))
		{
			jsonAddedItemEventList = File.ReadAllText(addedItemEvent);
			addedItemEventList = JsonSerializer.Deserialize<List<string>>(jsonAddedItemEventList);
		}
		string itemAdded = $"Date Added: {DateTime.Now}, Menu Item: {e.ItemName}, Menu Price: {e.ItemPrice}, From: {sender.GetType().Name}";
		addedItemEventList.Add(itemAdded);
		jsonAddedItemEventList = JsonSerializer.Serialize(addedItemEventList);
		File.WriteAllText(addedItemEvent, jsonAddedItemEventList);
	}

	void RemoveEventItemToList(object? sender, CafeMenu e)
	{
		const string removedItemEvent = "RemovedItemEvent.json";
		List<string> removedItemEventList = new List<string>();
		string jsonRemovedItemEventList;
		if (File.Exists(removedItemEvent))
		{
			jsonRemovedItemEventList = File.ReadAllText(removedItemEvent);
			removedItemEventList = JsonSerializer.Deserialize<List<string>>(jsonRemovedItemEventList);
		}
		string itemRemoved = $"Date Removed: {DateTime.Now}, Menu Item: {e.ItemName}, Menu Price: {e.ItemPrice}, From: {sender.GetType().Name}";
		removedItemEventList.Add(itemRemoved);
		jsonRemovedItemEventList = JsonSerializer.Serialize(removedItemEventList);
		File.WriteAllText(removedItemEvent, jsonRemovedItemEventList);
	}

	//--------------------------------------------------------------------------------------------------------------------------------------------------------------

	private void MenuGreetingMethod()
	{
		Console.WriteLine(
			$"Welcome to Soylent Green Cafe where our Customer is our specialty!! {Environment.NewLine}" +
			$"I will be your host Jenna {Environment.NewLine}" +
			$"{Environment.NewLine}" +
			$"How may I assist you from the following options? {Environment.NewLine}" +
			$"{Environment.NewLine}" +
			$"[View] the Menu {Environment.NewLine}" +
			$"[Add] new item to the menu {Environment.NewLine}" +
			$"[Update] an item on the menu {Environment.NewLine}" +
			$"[Remove] an item from the menu {Environment.NewLine}" +
			$"[Exit] Soylent Green Cafe {Environment.NewLine}" +
			$"{Environment.NewLine}" +
			$"Input preferred choice within []?"
			);
	}

	private string UserStringInputMethod() => Console.ReadLine().ToLower();

	private bool SelectingUserChoiceMethod(bool isWroking, string optionSelected)
	{
		switch (optionSelected)
		{
			case "view":
				//Console.WriteLine("In View");
				_iReadingFromDatabase.Run();
				break;
			case "add":
				//Console.WriteLine("In Add");
				_iCreatingToDatabase.Run();

				break;
			case "remove":
				//Console.WriteLine("In Remove");
				_iDeletingFromDatabase.Run();
				break;
			case "update":
				//Console.WriteLine("In Update");
				_iUpdatingDatabase.Run();
				break;
			case "exit":
				Console.WriteLine("Thank you for trying Soylent Green Cafe where our Customers are our specialty!");
				SaveToXMLFile();
				isWroking = false;
				break;
			default:
				Console.WriteLine("Incorrect input, please try again.");
				break;

		}
		return isWroking;
	}

	private void SaveToXMLFile()
	{
		var drinks = _drinkRepository.GetAll();
		var meals = _mealRepository.GetAll();
		var xDocument = new XDocument();
		var xElement_1 = new XElement("Menu", 
			drinks.Select(x =>
					new XElement("Drinks",
				new XAttribute("Name", x.ItemName),
				new XAttribute("FE_Combined", x.ItemPrice),
				new XAttribute("Manufacturer", x.Ingredients))),
			meals.Select(x =>
					new XElement("Meals",
				new XAttribute("Name", x.ItemName),
				new XAttribute("FE_Combined", x.ItemPrice),
				new XAttribute("Manufacturer", x.Ingredients))));

		xDocument.Add(xElement_1);
		
		xDocument.Save("menu.xml");
	}
}

