using DataAccess;
using System;

namespace Testing
{
	class Program
	{
		static void Main()
		{
			SQLServer server = new SQLServer();
			server.FindServers();
			Console.WriteLine("Server name: " + server.ToString());

			SQLConnectionString connectionString = new SQLConnectionString(server.MyServers[0], "QLNV");
			Console.WriteLine("Connection: " + connectionString.TestConnection().ToString());

			SQLTable tables = new SQLTable(connectionString.ConnectionString);
			tables.FindTables();
			Console.WriteLine("\nExisting Tables:");
			Console.WriteLine(tables.ToString());

			Console.Write("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
