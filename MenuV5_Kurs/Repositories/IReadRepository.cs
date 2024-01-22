public interface IReadRepository<out T>
	where T : class, ICafeMenu
{
	public T GetSpecific(int id);

	public IEnumerable<T> GetAll();
}

