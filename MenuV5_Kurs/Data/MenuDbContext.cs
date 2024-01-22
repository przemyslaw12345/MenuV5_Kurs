
using Microsoft.EntityFrameworkCore;

internal class MenuDbContext : DbContext
{
	public DbSet<Drink> Drinks { get; set; }
	public DbSet<Meal> Meals { get; set; }

	public MenuDbContext(DbContextOptions<MenuDbContext> options) : base(options)
	{

	}
}