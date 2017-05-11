using SQLite;

namespace sub100DemoApp
{
	public class EntityBase
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
	}
}