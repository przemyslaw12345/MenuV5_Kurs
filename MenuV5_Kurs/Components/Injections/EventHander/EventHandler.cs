
using System.Text.Json;

internal class EventHandlerClass : IEventHandlerInterface
{
	public void AddEventItemToList(object? sender, CafeMenu e)
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



	public void RemoveEventItemToList(object? sender, CafeMenu e)
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

	
}

