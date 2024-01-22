using Microsoft.EntityFrameworkCore;

internal class MenuSqlRepository<T> :
	IRepository<T> where T : class, ICafeMenu
{
	private readonly DbSet<T> _dbSet;
	private readonly MenuDbContext _dbContext;

	public MenuSqlRepository(MenuDbContext dbContext)
	{
		_dbContext = dbContext;
		_dbSet = _dbContext.Set<T>();
	}

	public event EventHandler<T> RemovedItem;
	public event EventHandler<T> AddedItem;

	public void Add(T item)
	{
		_dbSet.Add(item);
		AddedItem.Invoke(this, item);
	}

	public void Edit(T item)
	{
		_dbSet.Update(item);
	}

	public IEnumerable<T> GetAll()
	{
		return _dbSet.ToList();
	}

	public T GetSpecific(int id)
	{
		return _dbSet.Find(id);
	}

	public void RemoveItem(T item)
	{
		_dbSet.Remove(item);
		RemovedItem.Invoke(this, item);
	}

	public void Save()
	{
		_dbContext.SaveChanges();
	}
}

