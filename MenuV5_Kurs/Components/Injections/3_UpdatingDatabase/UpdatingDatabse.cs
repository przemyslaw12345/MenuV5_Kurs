internal class UpdatingDatabse : IUpdatingDatabase
{
	private readonly MenuDbContext _menuDbContext;
	private readonly IReadingFromDatabase _iReadingFromDatabase;
	private readonly IDeletingFromDatabase _iDeletingFromDatabase;
	private readonly IRepository<Meal> _mealRepository;
	private readonly IRepository<Drink> _drinkRepository;
	public UpdatingDatabse(
		MenuDbContext menuDbContext,
		IReadingFromDatabase iReadingFromDatabase,
		IDeletingFromDatabase iDeletingFromDatabase,
		IRepository<Meal> mealRepository,
		IRepository<Drink> drinkRepository
		)
	{
		_menuDbContext = menuDbContext;
		_iReadingFromDatabase = iReadingFromDatabase;
		_iDeletingFromDatabase = iDeletingFromDatabase;
		_mealRepository = mealRepository;
		_drinkRepository = drinkRepository;
	}

	public void Run()
	{
		bool isWorking = true;
		while (isWorking)
		{
			UpdatingDatabaseMainMenuMethod();
			string optionSelected = UserStringInputMethod();
			isWorking = SelectingWhatToUpdateInTheMenuMethod(optionSelected, isWorking);
		}
	}

	private void UpdatingDatabaseMainMenuMethod()
	{
		Console.Clear();
		Console.WriteLine(
			$"What would you like to update in the menu? {Environment.NewLine}" +
			$"A [Drink] {Environment.NewLine}" +
			$"A [Meal] {Environment.NewLine}" +
			$"Or would you like to [Exit] to the Main Menu {Environment.NewLine}"
			);
	}

	private string UserStringInputMethod() => Console.ReadLine().ToLower();

	private bool SelectingWhatToUpdateInTheMenuMethod(string optionSelected, bool isWorking)
	{
		Console.Clear();
		bool isWorkingSubLoop = true;
		CafeMenu cafeMenu = null;
		int itemNumber = 0;
		while (isWorkingSubLoop)
		{
			switch (optionSelected)
			{
				case "drink":
					_iReadingFromDatabase.ViewDrinkMenu();
					itemNumber = _iDeletingFromDatabase.GetItemNumberMethod(optionSelected);
					cafeMenu = _drinkRepository.GetSpecific(itemNumber);
					cafeMenu = UpdateProcessMainMenuMethod(cafeMenu);
					_drinkRepository.Edit((Drink)cafeMenu);
					isWorkingSubLoop = false;
					isWorking = CheckIfToContinueUpdatingItemInTheMenuMethod(isWorking);
					_drinkRepository.Save();
					break;
				case "meal":
					_iReadingFromDatabase.ViewMealMenu();
					itemNumber = _iDeletingFromDatabase.GetItemNumberMethod(optionSelected);
					cafeMenu = _mealRepository.GetSpecific(itemNumber);
					cafeMenu = UpdateProcessMainMenuMethod(cafeMenu);
					_mealRepository.Edit((Meal)cafeMenu);
					isWorkingSubLoop = false;
					isWorking = CheckIfToContinueUpdatingItemInTheMenuMethod(isWorking);
					_mealRepository.Save();
					break;
				case "exit":
					isWorkingSubLoop = false;
					isWorking = false;
					break;
				default:
					Console.WriteLine("You entered an invalid option, please try again");
					UpdatingDatabaseMainMenuMethod();
					optionSelected = UserStringInputMethod();
					isWorkingSubLoop = true;
					Console.ReadKey();
					break;
			}
		}
		return isWorking;
	}

	private CafeMenu UpdateProcessMainMenuMethod(CafeMenu? cafeMenu)
	{
		bool isWorking = true;
		while (isWorking)
		{
			Console.Clear();
			ChoicesAvailableToUpdateMethod();
			string optionSelcted = UserStringInputMethod();
			cafeMenu = SelectingWhatToUpdateInTheMenuMethod(optionSelcted, cafeMenu);
			if (optionSelcted == "exit")
			{
				isWorking = false;
			}
		}

		return cafeMenu;

	}

	private CafeMenu SelectingWhatToUpdateInTheMenuMethod(string optionSelcted, CafeMenu? cafeMenu)
	{
		bool isWorking = true;
		switch (optionSelcted)
		{
			case "name":
				cafeMenu.ItemName = UpdateNameMethod();
				Console.WriteLine($"Name has been updated to: {cafeMenu.ItemName}");
				break;
			case "price":
				cafeMenu.ItemPrice = UpdatePriceMethod();
				Console.WriteLine($"Price has been updated to: {cafeMenu.ItemPrice}");
				break;
			case "ingrediants":
				cafeMenu.Ingredients = UpdateIngrediantsMethod(cafeMenu);
				Console.WriteLine($"Ingrediants have been updated to: {Environment.NewLine}" +
					$"{String.Join(",", cafeMenu.Ingredients)}");
				break;
			default:
				Console.WriteLine("You entered an invalid option, press any key to try again");
				Console.ReadLine();
				break;
		}
		return cafeMenu;
	}
	private static void ChoicesAvailableToUpdateMethod()
	{
		Console.WriteLine(
						$"What would you like to update? {Environment.NewLine}" +
						$"The [Name] {Environment.NewLine}" +
						$"The [Price] {Environment.NewLine}" +
						$"The [Ingrediants] {Environment.NewLine}" +
						$"Or would you like to [Exit] {Environment.NewLine}"
						);
	}

	private List<string> UpdateIngrediantsMethod(CafeMenu cafeMenu)
	{
		bool isWorking = true;
		while (isWorking)
		{
			Console.WriteLine($"{cafeMenu.ItemName} {Environment.NewLine}");
			for (int i = 0; i < cafeMenu.Ingredients.Count; i++)
			{
				Console.WriteLine($"{i + 1} | {cafeMenu.Ingredients[i]}");
			}
			Console.WriteLine("Which item would you like to update?");
			int userSelection = int.Parse(UserStringInputMethod());

			Console.WriteLine("What is the name of the updated ingrediant?");
			string newIngrediantName = UserStringInputMethod();
			cafeMenu.Ingredients[userSelection - 1] = newIngrediantName;
			isWorking = CheckIfToContinueUpdatingItemInTheMenuMethod(isWorking);
		}
		return cafeMenu.Ingredients;
	}

	float UpdatePriceMethod()
	{
		float price = 0;
		bool isWorkingSubLoop = true;
		while (isWorkingSubLoop)
		{
			Console.WriteLine("What is the new price of the item?");
			if (float.TryParse(UserStringInputMethod(), out price))
			{
				isWorkingSubLoop = false;
				return price;
			}
			else
			{
				Console.WriteLine("Please input price correctly i.e. [0.99]");
			}
		}
		return price;
	}

	string UpdateNameMethod()
	{
		Console.WriteLine("What is the new name of the item?");
		string optionSelected = UserStringInputMethod();
		return optionSelected;
	}

	public static void VerificationOfItemUpdateMethod(CafeMenu cafeMenu)
	{
		if (cafeMenu.Ingredients == null)
		{
			Console.WriteLine($"Are you sure you wish to update:" +
			$"{cafeMenu.Id}. {cafeMenu.ItemName} ----- {cafeMenu.ItemPrice}USD {Environment.NewLine}");
		}
		else
		{
			Console.WriteLine($"Are you sure you wish to update:" +
			$"{cafeMenu.Id}. {cafeMenu.ItemName} ----- {cafeMenu.ItemPrice}USD {Environment.NewLine}" +
			$"{String.Join(", ", cafeMenu.Ingredients)} {Environment.NewLine}");
		}
		Console.WriteLine("[Yes/No]");
	}

	private bool CheckIfToContinueUpdatingItemInTheMenuMethod(bool isWorking)
	{
		bool isWorkingSubLoop = true;
		Console.WriteLine("Would you like to update another item in the menu? Yes/No");
		while (isWorkingSubLoop)
		{
			string willContinue = Console.ReadLine().ToLower();
			switch (willContinue)
			{
				case "yes":
					isWorking = true;
					isWorkingSubLoop = false;
					break;
				case "no":
					isWorking = false;
					isWorkingSubLoop = false;
					break;
				default:
					isWorkingSubLoop = true;
					Console.WriteLine("Please write yes or no");
					break;
			}
		}
		return isWorking;
	}
}