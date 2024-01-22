public interface ICafeMenu
{
	public int Id { get; set; }
	public string ItemName { get; set; }
	public float ItemPrice { get; set; }
	public List<string> Ingredients { get; set; }
}

