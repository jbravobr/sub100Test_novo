using SQLite;

namespace sub100DemoApp
{
	public interface ISQLite
	{
		SQLiteConnection GetConn();
	}
}