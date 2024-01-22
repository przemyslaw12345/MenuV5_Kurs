using System.Linq;
using System.Text.Json;
internal class ReadingFromDatabase : IReadingFromDatabase
{
	//private readonly MenuDbContext _menuDbContext;
	private readonly IRepository<Meal> _mealRepository;
	private readonly IRepository<Drink> _drinkRepository;
	public ReadingFromDatabase(MenuDbContext menuDbContext, 
		IRepository<Meal> mealRepository, 
		IRepository<Drink> drinkRepository)
	{
		//_menuDbContext = menuDbContext;
		_mealRepository = mealRepository;
		_drinkRepository = drinkRepository;
	}
	public void Run()
	{
		bool isWorking = true;
		while (isWorking)
		{
			ReadingFromDatabaseMainMenuMethod();
			string optionSelected = UserStringInputMethod();
			isWorking = SelectingToStayOnReadingMenuOrToOrderMenu(optionSelected, isWorking);
		}
	}
	
	private void ReadingFromDatabaseMainMenuMethod()
	{
		Console.Clear();
		Console.WriteLine(
			$"Please enjoy our menu. {Environment.NewLine}" +
			$"If you would like to organize the menu please type [Order] {Environment.NewLine}" +
			$"Else to return to main menu please type [Exit] {Environment.NewLine}"
			);
		Console.WriteLine("Drink Menu");
		ViewDrinkMenu();

		Console.WriteLine("------------------------------------------------------------------------");

		Console.WriteLine("Meal Menu");
		ViewMealMenu();

	}

	public void ViewDrinkMenu()
	{
		foreach (var drink in _drinkRepository.GetAll())
		{

			if (drink.Ingredients == null)
			{
				Console.WriteLine($"{drink.Id}. {drink.ItemName} ----- {drink.ItemPrice}USD");
			}
			else
			{
				Console.WriteLine($"{drink.Id}. {drink.ItemName} ----- {drink.ItemPrice}USD {Environment.NewLine}" +
				$"{String.Join(", ", drink.Ingredients)}");
			}
		}

	}

	public void ViewMealMenu()
	{
		foreach (var meal in _mealRepository.GetAll())
		{
			if (meal.Ingredients == null)
			{
				Console.WriteLine($"{meal.Id}. {meal.ItemName} ----- {meal.ItemPrice}USD");
			}
			else
			{
				Console.WriteLine($"{meal.Id}. {meal.ItemName} ----- {meal.ItemPrice}USD {Environment.NewLine}" +
				$"{String.Join(", ", meal.Ingredients)}");
			}

		}
	}

	private string UserStringInputMethod() => Console.ReadLine().ToLower();

	private bool SelectingToStayOnReadingMenuOrToOrderMenu(string optionSelected, bool isWorking)
	{
		switch (optionSelected)
		{
			case "order":
				OrderCafeMenuMethod();
				break;
			case "exit":
				isWorking = false;
				break;
			default:
				Console.WriteLine("You entered an invalid option, please try again");
				break;
		}
		return isWorking;
	}

	private void OrderCafeMenuMethod()
	{
		bool isWorking = true;
		while (isWorking)
		{
			OrderCafeMenuMenuMethod();
			string optionSelected = UserStringInputMethod();
			isWorking = SelectingOptionToOrderBy(optionSelected, isWorking);
		}
	}



	private void OrderCafeMenuMenuMethod()
	{
		Console.Clear();
		Console.WriteLine(
			$"Welcome, how would you like to order the menu? {Environment.NewLine}" +
			$"[1] View only the Drinks Menu {Environment.NewLine}" +
			$"[2] View only the Meal Menu {Environment.NewLine}" +
			$"[3] Order the Drinks Menu by price {Environment.NewLine}" +
			$"[4] Order the Meal Menu by price {Environment.NewLine}" +
			$"[5] Order the Whole Menu by price {Environment.NewLine}" +
			$"[6] Exit {Environment.NewLine}" +
			$"{Environment.NewLine}" +
			$"Input preferred choice within []?");
	}

	private bool SelectingOptionToOrderBy(string optionSelected, bool isWorking)
	{
		switch (optionSelected)
		{
			case "1":
				ViewDrinkMenu();
				Console.ReadLine();
				break;
			case "2":
				ViewMealMenu();
				Console.ReadLine();
				break;
			case "3":
				ViewMenuOrderedByPrice(optionSelected);
				break;
			case "4":
				ViewMenuOrderedByPrice(optionSelected);
				break;
			case "5":
				ViewMenuOrderedByPrice(optionSelected);
				break;
			case "6":
				isWorking = false;
				break;
			default:
				Console.WriteLine("You entered an invalid option, please try again");
				break;
		}
		return isWorking;
	}

	private void ViewMenuOrderedByPrice(string optionSelected)
	{
		IEnumerable<CafeMenu>? menu = null;
		Console.Clear();
		switch (optionSelected)
		{
			case "3":
				menu = _drinkRepository.GetAll().OrderBy(x => x.ItemPrice);
				break;
			case "4":
				menu = _mealRepository.GetAll().OrderBy(x => x.ItemPrice);
				break;
			case "5":
				List<CafeMenu> cafeMenu = [.. _drinkRepository.GetAll(), .. _mealRepository.GetAll()];
				menu = cafeMenu.OrderBy(x => x.ItemPrice);
				//menu = _menuDbContext.Meals.;
				break;
		}

		foreach (var item in menu)
		{
			if (item.Ingredients == null)
			{
				Console.WriteLine($"{item.Id}. {item.ItemName} ----- {item.ItemPrice}USD");
			}
			else
			{
				Console.WriteLine($"{item.Id}. {item.ItemName} ----- {item.ItemPrice}USD {Environment.NewLine}" +
				$"{String.Join(", ", item.Ingredients)}");
			}
		}
		Console.ReadLine();
	}
}

