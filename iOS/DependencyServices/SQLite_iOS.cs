using System;
using System.IO;
using sub100DemoApp.iOS;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace sub100DemoApp.iOS
{
	public class SQLite_iOS : ISQLite
	{
		public SQLiteConnection GetConn()
		{
			var sqliteFilename = "sub100DemoApp.db3";
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string libraryPath = Path.Combine(documentsPath, "..", "Library");
			var path = Path.Combine(libraryPath, sqliteFilename);
			var conn = new SQLiteConnection(path);

			return conn;
		}
	}
}