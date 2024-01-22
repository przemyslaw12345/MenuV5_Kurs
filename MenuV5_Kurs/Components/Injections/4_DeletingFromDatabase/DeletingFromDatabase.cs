internal class DeletingFromDatabase : IDeletingFromDatabase
{
	private readonly MenuDbContext _menuDbContext;
	private readonly IReadingFromDatabase _iReadingFromDatabase;
	private readonly IRepository<Meal> _mealRepository;
	private readonly IRepository<Drink> _drinkRepository;
	public DeletingFromDatabase(
		MenuDbContext menuDbContext,
		IReadingFromDatabase iReadingFromDatabase,
		IRepository<Meal> mealRepository,
		IRepository<Drink> drinkRepository
		)
	{
		_menuDbContext = menuDbContext;
		_iReadingFromDatabase = iReadingFromDatabase;
		_mealRepository = mealRepository;
		_drinkRepository = drinkRepository;
	}
	public void Run()
	{
		bool isWorking = true;
		while (isWorking)
		{
			DeletingFromDatabaseMainMenuMethod();
			string optionSelected = UserStringInputMethod();
			isWorking = SelectingFromWhatTableToDeleteMethod(optionSelected, isWorking);
		}
	}

	private void DeletingFromDatabaseMainMenuMethod()
	{
		Console.Clear();
		Console.WriteLine(
			$"What would you like to remove from the menu? {Environment.NewLine}" +
			$"A [Drink] {Environment.NewLine}" +
			$"A [Meal] {Environment.NewLine}" +
			$"Or would you like to [Exit] to the Main Menu {Environment.NewLine}"
			);
	}
	private string UserStringInputMethod() => Console.ReadLine().ToLower();

	private bool SelectingFromWhatTableToDeleteMethod(string optionSelected, bool isWorking)
	{
		bool isWorkingSubLoop = true;
		while (isWorkingSubLoop)
		{
			switch (optionSelected)
			{
				case "drink":
					GeneralRemoveMethod(optionSelected);
					isWorkingSubLoop = false;
					isWorking = CheckIfToContinueDeletingFromMenuMethod(isWorking);
					break;
				case "meal":
					GeneralRemoveMethod(optionSelected);
					isWorkingSubLoop = false;
					isWorking = CheckIfToContinueDeletingFromMenuMethod(isWorking);
					break;
				case "exit":
					isWorkingSubLoop = false;
					isWorking = false;
					break;
				default:
					Console.WriteLine("You entered an invalid option, please try again");
					DeletingFromDatabaseMainMenuMethod();
					optionSelected = UserStringInputMethod();
					isWorkingSubLoop = true;
					Console.ReadKey();
					break;
			}
		}
		return isWorking;
	}

	public void GeneralRemoveMethod(string optionToRemoveSelected)
	{
		Console.Clear();
		CafeMenu cafeMenu = null;
		int itemNumber = 0;
		bool isWorkingSubLoop = true;
		switch (optionToRemoveSelected)
		{
			case "drink":
				_iReadingFromDatabase.ViewDrinkMenu();
				itemNumber = GetItemNumberMethod(optionToRemoveSelected);
				cafeMenu = _drinkRepository.GetSpecific(itemNumber);
				_drinkRepository.Save();
				break;
			case "meal":
				_iReadingFromDatabase.ViewMealMenu();
				itemNumber = GetItemNumberMethod(optionToRemoveSelected);
				cafeMenu = _mealRepository.GetSpecific(itemNumber);
				_mealRepository.Save();
				break;
		}

		while (isWorkingSubLoop)
		{
			isWorkingSubLoop = RemovingItemsFromDatabaseMethod(optionToRemoveSelected, cafeMenu, itemNumber, isWorkingSubLoop);
		}
	}

	private bool RemovingItemsFromDatabaseMethod(string optionToRemoveSelected, CafeMenu cafeMenu, int itemNumber, bool isWorkingSubLoop)
		
	{
		VerificationOfItemRemovalMethod(cafeMenu);
		string optionSelected = UserStringInputMethod();
		switch (optionSelected)
		{
			case "yes":
				switch (optionToRemoveSelected)
				{
					case "drink":
						_drinkRepository.RemoveItem(_drinkRepository.GetSpecific(itemNumber));
						break;
					case "meal":
						_mealRepository.RemoveItem(_mealRepository.GetSpecific(itemNumber));
						break;
				}
				isWorkingSubLoop = false;
				break;
			case "no":
				Console.WriteLine("Understood, will not delete the selected option. Have a good day.");
				isWorkingSubLoop = false;
				break;
			default:
				Console.WriteLine("You entered an invalid option, please try again");
				isWorkingSubLoop = true;
				break;
		}

		return isWorkingSubLoop;
	}

	public void VerificationOfItemRemovalMethod(CafeMenu cafeMenu)
	{
		if (cafeMenu.Ingredients == null)
		{
			Console.WriteLine($"Are you sure you wish to delete: {Environment.NewLine}" +
			$"{cafeMenu.Id}. {cafeMenu.ItemName} ----- {cafeMenu.ItemPrice}USD {Environment.NewLine}");
		}
		else
		{
			Console.WriteLine($"Are you sure you wish to delete: {Environment.NewLine}" +
			$"{cafeMenu.Id}. {cafeMenu.ItemName} ----- {cafeMenu.ItemPrice}USD {Environment.NewLine}" +
			$"{String.Join(", ", cafeMenu.Ingredients)} {Environment.NewLine}");
		}
		Console.WriteLine("[Yes/No]");
	}

	public int GetItemNumberMethod(string optionToRemoveSelected)
	{
		bool isWorkingSubLoop = true;
		int itemNumber = 0;
		while (isWorkingSubLoop)
		{
			switch (optionToRemoveSelected)
			{
				case "drink":
					Console.WriteLine("Which drink # would you like to retrieve from the menu");
					break;
				case "meal":
					Console.WriteLine("Which meal # would you like to retrieve from the menu");
					break;
			}

			if (int.TryParse(UserStringInputMethod(), out itemNumber))
			{
				return itemNumber;
				isWorkingSubLoop = false;
			}
			else
			{
				Console.WriteLine("Please input item number correctly i.e. [1]");
			}
		}
		return itemNumber;
	}

	private bool CheckIfToContinueDeletingFromMenuMethod(bool isWorking)
	{
		bool isWorkingSubLoop = true;
		Console.WriteLine("Would you like to remove another item from the menu? Yes/No");
		while (isWorkingSubLoop)
		{
			string willContinue = UserStringInputMethod();
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