internal class CreatingToDatabase : ICreatingToDatabase
{
	private readonly MenuDbContext _menuDbContext;
	private readonly IRepository<Meal> _mealRepository;
	private readonly IRepository<Drink> _drinkRepository;
	public CreatingToDatabase(
		MenuDbContext menuDbContext, 
		IRepository<Meal> mealRepository, 
		IRepository<Drink> drinkRepository)
	{
		_menuDbContext = menuDbContext;
		_mealRepository = mealRepository;
		_drinkRepository = drinkRepository;
	}

	public void Run()
	{
		bool isWorking = true;
		while (isWorking)
		{
			CreatingToDatabaseMainMenuMethod();
			string optionSelected = UserStringInputMethod();
			isWorking = SelectingWhatToAddToTheMenuMethod(optionSelected, isWorking);
		}
	}

	private void CreatingToDatabaseMainMenuMethod()
	{
		Console.Clear();
		Console.WriteLine(
			$"What would you like to add to the menu? {Environment.NewLine}" +
			$"A [Drink] {Environment.NewLine}" +
			$"A [Meal] {Environment.NewLine}" +
			$"Or would you like to [Exit] to the Main Menu {Environment.NewLine}"
			);
	}

	private string UserStringInputMethod() => Console.ReadLine().ToLower();

	private bool SelectingWhatToAddToTheMenuMethod(string optionSelected, bool isWorking)
	{
		bool isWorkingSubLoop = true;
		while (isWorkingSubLoop)
		{
			switch (optionSelected)
			{
				case "drink":
					AddMethod(optionSelected);
					isWorkingSubLoop = false;
					isWorking = CheckIfToContinueAddingToMenuMethod(isWorking);
					break;
				case "meal":
					AddMethod(optionSelected);
					isWorkingSubLoop = false;
					isWorking = CheckIfToContinueAddingToMenuMethod(isWorking);
					break;
				case "exit":
					isWorkingSubLoop = false;
					isWorking = false;
					break;
				default:
					Console.WriteLine("You entered an invalid option, please try again");
					CreatingToDatabaseMainMenuMethod();
					optionSelected = UserStringInputMethod();
					isWorkingSubLoop = true;
					Console.ReadKey();
					break;
			}
		}
		return isWorking;
	}
	private void AddMethod(string optionToAddSelected)
	{
		switch (optionToAddSelected)
		{
			case "drink":
				Console.WriteLine($"What is the drinks name? {Environment.NewLine}");
				break;
			case "meal":
				Console.WriteLine($"What is the meals name? {Environment.NewLine}");
				break;
		}

		string nameOfItem = UserStringInputMethod();

		float priceOfItem = GetPriceMethod(optionToAddSelected);

		Console.WriteLine($"Are there any ingredients you would like to display? [Yes/No] {Environment.NewLine}");
		string optionSelected = UserStringInputMethod();
		List<string> ingredientsTempList = new List<string>();
		switch (optionSelected)
		{
			case "yes":
				ingredientsTempList = IngredientsOfItems();
				break;
			case "no":
				ingredientsTempList = new List<string>();
				break;
		}
		Drink? newDrinkToAdd;
		Meal? newMealToAdd;
		switch (optionToAddSelected)
		{
			case "drink":
				newDrinkToAdd = new Drink
				{
					ItemName = nameOfItem,
					ItemPrice = priceOfItem,
					Ingredients = ingredientsTempList
				};
				_drinkRepository.Add(newDrinkToAdd);
				_drinkRepository.Save();
				break;
			case "meal":
				newMealToAdd = new Meal
				{
					ItemName = nameOfItem,
					ItemPrice = priceOfItem,
					Ingredients = ingredientsTempList
				};
				_mealRepository.Add(newMealToAdd);
				_mealRepository.Save();
				break;
		}
	}

	private float GetPriceMethod(string optionToAddSelected)
	{
		bool isWorkingSubLoop = true;
		float price = 0;
		while (isWorkingSubLoop)
		{
			switch (optionToAddSelected)
			{
				case "drink":
					Console.WriteLine($"What is the drinks price? {Environment.NewLine}");
					break;
				case "meal":
					Console.WriteLine($"What is the meals price? {Environment.NewLine}");
					break;
			}

			if (float.TryParse(UserStringInputMethod(), out price))
			{
				return price;
				isWorkingSubLoop = false;
			}
			else
			{
				Console.WriteLine("Please input price correctly i.e. [0.99]");
			}
		}
		return price;
	}

	private List<string> IngredientsOfItems()
	{
		List<string> ingrediantsTempList = new List<string>();
		string ingrediant;
		bool subLoop = true;
		int counter = 0;
		while (subLoop)
		{
			switch (counter)
			{
				case 0:
					Console.WriteLine("Please add first ingrediant.");
					counter++;
					break;
				case 1:
					Console.WriteLine("Please add next ingrediant.");
					break;
			}
			ingrediant = UserStringInputMethod();
			ingrediantsTempList.Add(ingrediant);
			Console.WriteLine("Add another ingrediant? [Yes/No]");
			string optionSelected = UserStringInputMethod();
			switch (optionSelected)
			{
				case "yes":
					subLoop = true;
					break;
				case "no":
					subLoop = false;
					Console.WriteLine("Thank you for adding all the ingrediants, have a nice day!");
					break;
			}
		}
		return ingrediantsTempList;
	}

	private bool CheckIfToContinueAddingToMenuMethod(bool isWorking)
	{
		bool isWorkingSubLoop = true;
		Console.WriteLine("Would you like to add another item to the menu? Yes/No");
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

