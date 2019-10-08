using DataAccess;
using System;

namespace Testing
{
	class Program
	{
		static void Main()
		{
			SQLServer server = new SQLServer();
			server.GetServers();
			Console.WriteLine("Server name: " + server.ToString());

			SQLConnectionString SQLConnect = new SQLConnectionString(@"DESKTOP-G3SCN6I\SQLEXPRESS", "QLNV");
			if (SQLConnect.TestConnection())
			{
				Console.WriteLine("Connection: OK");

				//SQLDatabase database = new SQLDatabase(SQLConnect);
				//database.GetDatabases();
				//Console.WriteLine("\nDatabases: \n" + database.ToString());

				SQLTable table = new SQLTable(SQLConnect);
				table.GetTables();
				Console.WriteLine("\nTables in BalloonShop: \n" + table.ToString());

				SQLColumn column = new SQLColumn(SQLConnect, "NhanVien");
				column.GetColumns();
				Console.WriteLine("\nColumns in Category: \n" + column.ToString());
			}
			else
			{
				Console.WriteLine("Connection: Error");
			}
			Console.ReadKey();
		}
	}
}
