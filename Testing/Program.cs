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

			SQLConnectionString connectionString = new SQLConnectionString(server.MyServers[0]);
			Console.WriteLine(connectionString.TestConnection().ToString());

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
