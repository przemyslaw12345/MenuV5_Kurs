internal interface IDeletingFromDatabase
{
	void VerificationOfItemRemovalMethod(CafeMenu cafeMenu);
	int GetItemNumberMethod(string optionToRemoveSelected);
	void Run();
}