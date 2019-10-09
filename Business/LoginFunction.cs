using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
	public class LoginFunction
	{

		public LoginFunction()
		{

		}

		public List<string> LoadServer()
		{
			SQLServer server = new SQLServer();
			server.GetServers();
			return server.MyServers;
		}

		public bool TestConnection(string server)
		{
			SQLConnectionString connection = new SQLConnectionString(server);
			return connection.TestConnection();
		}

		public bool TestConnection(string server, string username, string password)
		{
			SQLConnectionString connection = new SQLConnectionString(server, "master", username, password);
			return connection.TestConnection();
		}
	}
}
