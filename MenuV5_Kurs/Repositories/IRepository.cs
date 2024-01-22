
public interface IRepository<T> :
	IReadRepository<T>,
	IWriteRepository<T>
	where T : class, ICafeMenu
{
	public event EventHandler<T> RemovedItem;
	public event EventHandler<T> AddedItem;
}
