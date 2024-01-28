
using CsvHelper;
using System.Globalization;
using System.Xml.Linq;

internal class SaveToXmlAndCsv : ISaveToXmlAndCsv
{
	private readonly IRepository<Meal> _mealRepository;
	private readonly IRepository<Drink> _drinkRepository;
	public SaveToXmlAndCsv(
		IRepository<Meal> mealRepository,
		IRepository<Drink> drinkRepository)
    {
		_mealRepository = mealRepository;
		_drinkRepository = drinkRepository;
	}
    public void SaveToXMLFile()
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
	public void SaveToCSVFile()
	{
		List<CafeMenu> csvMenu = [.. _drinkRepository.GetAll(), .. _mealRepository.GetAll()];

		using (var writer = new StreamWriter(@"Menu.csv"))

		using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
		{
			csv.WriteRecords(csvMenu);
		}
	}
}

