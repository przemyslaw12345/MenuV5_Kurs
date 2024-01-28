
using System.Reflection.Metadata;
using System.Xml.Linq;
using System.Xml.XPath;

internal class UserInterface : IUserInterface
{
	private readonly IRepository<Meal> _mealRepository;
	private readonly IRepository<Drink> _drinkRepository;

    public UserInterface(
		IRepository<Meal> mealRepository,
		IRepository<Drink> drinkRepository)
    {
		_mealRepository = mealRepository;
		_drinkRepository = drinkRepository;
	}
    public void Run()
	{
		bool isWroking = true;
		while (isWroking)
		{
			Console.Clear();
			MainMenuMessage();
			string optionSelected = UserStringInputMethod();
			isWroking = SelectingUserChoiceMethod(isWroking, optionSelected);
			Console.ReadKey();
		}
	}
	// Methods for main menu -------------------------------------------------------------------------
	private void MainMenuMessage()
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

	private bool SelectingUserChoiceMethod(bool isWroking, string optionSelected)
	{
		switch (optionSelected)
		{
			case "view":
				ViewingDbLoop();
				break;
			case "add":
				AddingToDbLoop();
				break;
			case "remove":
				DeletingFromDbLoop();
				break;
			case "update":
				UpdatingItemsInDbMenuLoop();
				break;
			case "exit":
				Console.WriteLine("Thank you for trying Soylent Green Cafe where our Customers are our specialty!");
				SaveToXMLFile();
				SaveToCSVFile();
				isWroking = false;
				break;
			default:
				Console.WriteLine("Incorrect input, please try again.");
				break;

		}
		return isWroking;
	}

	// Methods for Adding Items ----------------------------------------------------------------------

	public void AddingToDbLoop()
	{
		bool isWorking = true;
		while (isWorking)
		{
			AddingToDbMenuMessage();
			string optionSelected = UserStringInputMethod();
			UsingUserInputToAddToCorrectDb(optionSelected);
			isWorking = CheckIfUserWantsToContinue(isWorking);
		}
	}

	private void AddingToDbMenuMessage()
	{
		Console.Clear();
		Console.WriteLine(
			$"What would you like to add to the menu? {Environment.NewLine}" +
			$"A [Drink] {Environment.NewLine}" +
			$"A [Meal] {Environment.NewLine}" +
			$"[Nothing] {Environment.NewLine}" 
			);
	}

	private void UsingUserInputToAddToCorrectDb(string optionSelected)
	{
		bool isWorkingSubLoop = true;
		while (isWorkingSubLoop)
		{
			switch (optionSelected)
			{
				case "drink":
					AddingToDbMethod(optionSelected);
					isWorkingSubLoop = false;
					break;
				case "meal":
					AddingToDbMethod(optionSelected);
					isWorkingSubLoop = false;
					break;
				case "nothing":
                    Console.WriteLine("No new option will be added to the menu");
					isWorkingSubLoop = false;
					break;
                default:
					Console.WriteLine("You entered an invalid option, please try again");
					AddingToDbMenuMessage();
					optionSelected = UserStringInputMethod();
					isWorkingSubLoop = true;
					break;
			}
		}
		//return isWorking;
	}

	private void AddingToDbMethod(string optionToAddSelected)
	{
		Drink? newDrinkToAdd;

		Meal? newMealToAdd;

		string nameOfItem = GetNameMethod();

		float priceOfItem = GetFloatMethod();

		List<string> ingredientsList = GetIngrediantList();

		switch (optionToAddSelected)
		{
			case "drink":
				newDrinkToAdd = new Drink
				{
					ItemName = nameOfItem,
					ItemPrice = priceOfItem,
					Ingredients = ingredientsList
				};
				_drinkRepository.Add(newDrinkToAdd);
				_drinkRepository.Save();
				break;
			case "meal":
				newMealToAdd = new Meal
				{
					ItemName = nameOfItem,
					ItemPrice = priceOfItem,
					Ingredients = ingredientsList
				};
				_mealRepository.Add(newMealToAdd);
				_mealRepository.Save();
				break;
		}
	}

	private List<string> GetIngrediantList()
	{
		Console.WriteLine($"Add ingrediant list? [Yes/No] {Environment.NewLine}");
		string optionSelected = UserStringInputMethod();
		List<string> ingredientsTempList = new List<string>();
		switch (optionSelected)
		{
			case "yes":
				ingredientsTempList = FillingListWithIngrediants(ingredientsTempList);
				break;
			case "no":
				ingredientsTempList = new List<string>();
				break;
		}
		return ingredientsTempList;
	}
		
	// Methods for Viewing Items ---------------------------------------------------------------------

	public void ViewingDbLoop()
	{
		bool isWorking = true;
		string optionSelected;
		while (isWorking)
		{
			ViewingDbMenuMessage();
			optionSelected = UserStringInputMethod();
			isWorking = UsingUserInputToOrderOrExitMenu(optionSelected, isWorking);
		}
	}

	private void ViewingDbMenuMessage()
	{
		Console.Clear();
		Console.WriteLine(
			$"Please enjoy our menu. {Environment.NewLine}" +
			$"If you would like to organize the menu please type [Order] {Environment.NewLine}" +
			$"Else to return to main menu please type [Exit] {Environment.NewLine}"
			);
		Console.WriteLine("Drink Menu");
		ViewMenu(_drinkRepository);

		Console.WriteLine("------------------------------------------------------------------------");

		Console.WriteLine("Meal Menu");
		ViewMenu(_mealRepository);

	}

	public void ViewMenu<T>(IRepository<T> _itemRepository)
		where T : class, ICafeMenu
	{
		foreach (var item in _itemRepository.GetAll())
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

	}

	private bool UsingUserInputToOrderOrExitMenu(string optionSelected, bool isWorking)
	{
		switch (optionSelected)
		{
			case "order":
				OrderCafeMenuLoop();
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

	// Order Viewing Methods -------------------------------------------------------------------------

	private void OrderCafeMenuLoop()
	{
		bool isWorking = true;
		string optionSelected;
		while (isWorking)
		{
			OrderCafeMenuMessage();
			optionSelected = UserStringInputMethod();
			isWorking = UsingUserInputToSelectHowToOrderMenu(optionSelected, isWorking);
		}
	}

	private void OrderCafeMenuMessage()
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

	private bool UsingUserInputToSelectHowToOrderMenu(string optionSelected, bool isWorking)
	{
		switch (optionSelected)
		{
			case "1":
				ViewMenu(_drinkRepository);
				Console.ReadLine();
				break;
			case "2":
				ViewMenu(_mealRepository);
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
		IEnumerable<CafeMenu>? menu = GetOrderedMenu(optionSelected);
		Console.Clear();

		WriteOutOrderedMenu(menu);
		Console.ReadLine();
	}

	private static void WriteOutOrderedMenu(IEnumerable<CafeMenu>? menu)
	{
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
	}

	private IEnumerable<CafeMenu>? GetOrderedMenu(string optionSelected)
	{
		IEnumerable<CafeMenu>? menu = null;
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
		return menu;
	}

	// Methods for Deleting Items --------------------------------------------------------------------

	public void DeletingFromDbLoop()
	{
		bool isWorking = true;
		while (isWorking)
		{
			DeletingFromDbMenuMessage();
			string optionSelected = UserStringInputMethod();
			UsingUserInputToDeleteSelectedItem(optionSelected);
			isWorking = CheckIfUserWantsToContinue(isWorking);
		}
	}

	private void DeletingFromDbMenuMessage()
	{
		Console.Clear();
		Console.WriteLine(
			$"What would you like to remove from the menu? {Environment.NewLine}" +
			$"A [Drink] {Environment.NewLine}" +
			$"A [Meal] {Environment.NewLine}" +
			$"[Nothing] {Environment.NewLine}"
			);
	}

	private void UsingUserInputToDeleteSelectedItem(string optionSelected)
	{
		CafeMenu cafeMenu = null;
		bool isWorkingSubLoop = true;
		while (isWorkingSubLoop)
		{
			Console.Clear();
			switch (optionSelected)
			{
				case "drink":
					cafeMenu = RetrieveOptionForDeletion(optionSelected);
					DeleteFromDb(optionSelected, cafeMenu);
					isWorkingSubLoop = false;
					break;
				case "meal":
					cafeMenu = RetrieveOptionForDeletion(optionSelected);
					DeleteFromDb(optionSelected, cafeMenu);
					isWorkingSubLoop = false;
					break;
				case "nothing":
                    Console.WriteLine("Nothing will be removed from the menu");
                    isWorkingSubLoop = false;
					break;
				default:
					Console.WriteLine("You entered an invalid option, please try again");
					DeletingFromDbMenuMessage();
					optionSelected = UserStringInputMethod();
					isWorkingSubLoop = true;
					Console.ReadKey();
					break;
			}
			
		}
		
	}

	private CafeMenu RetrieveOptionForDeletion(string optionToRemoveSelected)
	{
		CafeMenu cafeMenu = null;
		int itemNumber = 0;
		switch (optionToRemoveSelected)
		{
			case "drink":
				ViewMenu(_drinkRepository);
				itemNumber = GetIntMethod();
				cafeMenu = _drinkRepository.GetSpecific(itemNumber);	
				break;
			case "meal":
				ViewMenu(_mealRepository);
				itemNumber = GetIntMethod();
				cafeMenu = _mealRepository.GetSpecific(itemNumber);
				break;
		}
		return cafeMenu;
	}

	private void DeleteFromDb(string optionToRemoveSelected, CafeMenu cafeMenu)

	{
		bool isWorking = true;
		int itemNumber = cafeMenu.Id;
		
		while (isWorking)
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
							_drinkRepository.Save();
							break;
						case "meal":
							_mealRepository.RemoveItem(_mealRepository.GetSpecific(itemNumber));
							_mealRepository.Save();
							break;
					}
					isWorking = false;
					break;
				case "no":
					Console.WriteLine("Understood, will not delete the selected option. Have a good day.");
					isWorking = false;
					break;
				default:
					Console.WriteLine("You entered an invalid option, please try again");
					break;
			}
		}
		
	}

	public void VerificationOfItemRemovalMethod(CafeMenu cafeMenu)
	{
		Console.Clear();
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

	// Methods for Updating Items --------------------------------------------------------------------

	public void UpdatingItemsInDbMenuLoop()
	{
		bool isWorking = true;
		while (isWorking)
		{
			UpdatingDbMenuMessage();
			string optionSelected = UserStringInputMethod();
			SelectingWhatToUpdateInTheMenuMethod(optionSelected);
			isWorking = CheckIfUserWantsToContinue(isWorking);
		}
	}

	private void UpdatingDbMenuMessage()
	{
		Console.Clear();
		Console.WriteLine(
			$"What would you like to update in the menu? {Environment.NewLine}" +
			$"A [Drink] {Environment.NewLine}" +
			$"A [Meal] {Environment.NewLine}" +
			$"[Nothing] {Environment.NewLine}"
			);
	}

	private void SelectingWhatToUpdateInTheMenuMethod(string optionSelected )
	{
		bool isWorkingSubLoop = true;
		CafeMenu cafeMenu = null;
		int itemNumber = 0;
		while (isWorkingSubLoop)
		{
			Console.Clear();
			Console.WriteLine("Please select which item number to update.");
			switch (optionSelected)
			{
				case "drink":
					ViewMenu(_drinkRepository);
					itemNumber = GetIntMethod();
					cafeMenu = SelectingWhatToUpdateInItemLoop(_drinkRepository.GetSpecific(itemNumber));
					_drinkRepository.Edit((Drink)cafeMenu);
					isWorkingSubLoop = false;
					_drinkRepository.Save();
					break;
				case "meal":
					ViewMenu(_mealRepository);
					itemNumber = GetIntMethod();
					cafeMenu = SelectingWhatToUpdateInItemLoop(_mealRepository.GetSpecific(itemNumber));
					_mealRepository.Edit((Meal)cafeMenu);
					isWorkingSubLoop = false;
					_mealRepository.Save();
					break;
				case "nothing":
					isWorkingSubLoop = false;
					break;
				default:
					Console.WriteLine("You entered an invalid option, please try again");
					UpdatingDbMenuMessage();
					optionSelected = UserStringInputMethod();
					isWorkingSubLoop = true;
					Console.ReadKey();
					break;
			}
		}
	}

	private CafeMenu SelectingWhatToUpdateInItemLoop(CafeMenu cafeMenu)
	{
		bool isWorking = true;
		while (isWorking)
		{
			Console.Clear();
			ItemProperetiesAvailableToUpdateMessage();
			string optionSelcted = UserStringInputMethod();
			cafeMenu = SelectingWhatToUpdateInTheMenuMethod(optionSelcted, cafeMenu);
			if (optionSelcted == "exit")
			{
				isWorking = false;
			}
		}

		return cafeMenu;

	}

	private static void ItemProperetiesAvailableToUpdateMessage()
	{
		Console.WriteLine(
						$"What would you like to update? {Environment.NewLine}" +
						$"The [Name] {Environment.NewLine}" +
						$"The [Price] {Environment.NewLine}" +
						$"The [Ingrediants] {Environment.NewLine}" +
						$"[Add] new items to ingrediant list {Environment.NewLine}" +
						$"Or would you like to [Exit] {Environment.NewLine}"
						);
	}

	private CafeMenu SelectingWhatToUpdateInTheMenuMethod(string optionSelcted, CafeMenu? cafeMenu)
	{
		switch (optionSelcted)
		{
			case "name":
				cafeMenu.ItemName = GetNameMethod();
				Console.WriteLine($"Name has been updated to: {cafeMenu.ItemName}");
				break;
			case "price":
				cafeMenu.ItemPrice = GetFloatMethod();
				Console.WriteLine($"Price has been updated to: {cafeMenu.ItemPrice}");
				break;
			case "ingrediants":
				cafeMenu.Ingredients = UpdateIngrediantsMethod(cafeMenu);
				Console.WriteLine($"Ingrediants have been updated to: {Environment.NewLine}" +
					$"{String.Join(",", cafeMenu.Ingredients)}");
				break;
			case "add":
				cafeMenu.Ingredients = FillingListWithIngrediants(cafeMenu.Ingredients);
				Console.WriteLine($"New ingrediants have been added to the list: {Environment.NewLine}" +
					$"{String.Join(",", cafeMenu.Ingredients)}");
				break;
			case "exit":
				break;
			default:
				Console.WriteLine("You entered an invalid option, press any key to try again");
				Console.ReadLine();
				break;
		}
		return cafeMenu;
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
			int userSelection = GetIntMethod();

			Console.WriteLine("What is the name of the updated ingrediant?");
			string newIngrediantName = UserStringInputMethod();
			cafeMenu.Ingredients[userSelection - 1] = newIngrediantName;
			isWorking = CheckIfUserWantsToContinue(isWorking);
		}
		return cafeMenu.Ingredients;
	}

	// Multi Use Methods -----------------------------------------------------------------------------

	private string GetNameMethod()
	{
		Console.WriteLine($"What is the items name? {Environment.NewLine}");

		return UserStringInputMethod();
	}

	private float GetFloatMethod()
	{
		bool isWorkingSubLoop = true;
		float price = 0;
		while (isWorkingSubLoop)
		{

			Console.WriteLine($"What is the items price? {Environment.NewLine}");

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

	private int GetIntMethod()
	{
		bool isWorkingSubLoop = true;
		int itemNumber = 0;
		while (isWorkingSubLoop)
		{
			Console.WriteLine("Input option number");

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

	private bool CheckIfUserWantsToContinue(bool isWorking)
	{
		bool isWorkingSubLoop = true;
		Console.WriteLine("Would you like to continue or return to previous menu? [Continue/Exit]");
		while (isWorkingSubLoop)
		{
			string willContinue = Console.ReadLine().ToLower();
			switch (willContinue)
			{
				case "continue":
					isWorking = true;
					isWorkingSubLoop = false;
					break;
				case "exit":
					isWorking = false;
					isWorkingSubLoop = false;
					break;
				default:
					isWorkingSubLoop = true;
					Console.WriteLine("Please write continue or exit");
					break;
			}
		}
		return isWorking;
	}

	private List<string> FillingListWithIngrediants(List<string> ingredientsTempList)
	{
		string ingrediant;
		string optionSelected;
		bool isWorking = true;
		while (isWorking)
		{
			Console.WriteLine("Please add ingrediant.");

			ingrediant = UserStringInputMethod();

			ingredientsTempList.Add(ingrediant);

			Console.WriteLine("Add another ingrediant? [Yes/No]");
			optionSelected = UserStringInputMethod();

			switch (optionSelected)
			{
				case "yes":
					isWorking = true;
					break;
				case "no":
					isWorking = false;
					Console.WriteLine("Thank you for adding all the ingrediants, have a nice day!");
					break;
			}
		}

		return ingredientsTempList;
	}

	private string UserStringInputMethod() => Console.ReadLine().ToLower();

	// Saving to File --------------------------------------------------------------------------------

	void SaveToXMLFile()
	{
		var xMLDocument = new XDocument();
		var xMLMenu = new XElement("Menu", 
			_drinkRepository.GetAll().Select(x =>
			new XElement("Drinks",
				new XAttribute("Name", x.ItemName),
				new XAttribute("Price", x.ItemPrice),
				new XAttribute("Ingrediants", String.Join(",", x.Ingredients)))),
			_mealRepository.GetAll().Select(y =>
			new XElement("Meals",
				new XAttribute("Name", y.ItemName),
				new XAttribute("Price", y.ItemPrice),
				new XAttribute("Ingrediants", String.Join(",", y.Ingredients))))
			);

		xMLDocument.Add(xMLMenu);
		xMLDocument.Save("XML_Menu.xml");

	}
	void SaveToCSVFile()
	{

	}
}

