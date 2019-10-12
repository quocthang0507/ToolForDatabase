using Business;
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

				SQLTable table = new SQLTable(SQLConnect.ConnectionString);
				table.GetTables();
				Console.WriteLine("\nTables in BalloonShop: \n" + table.ToString());

				SQLColumn column = new SQLColumn(SQLConnect.ConnectionString, "NhanVien");
				column.GetColumns();
				Console.WriteLine("\nColumns in Category: \n" + column.ToString());

				ConvertClass convert = new ConvertClass("NhanVien", column.MyColumns);
				Console.WriteLine(convert.GenerateConstructors());
			}
			else
			{
				Console.WriteLine("Connection: Error");
			}
			Console.ReadKey();
		}
	}
}
