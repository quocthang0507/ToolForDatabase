using DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
	/// <summary>
	/// Lớp cung cấp các thao tác cho form Main 
	/// </summary>
	public class MainFunction
	{
		private SqlConnection connection;

		public MainFunction(string connection)
		{
			this.connection = new SqlConnection(connection);
		}

		public List<string> GetTables()
		{
			SQLTable table = new SQLTable(connection.ConnectionString);
			table.GetTables();
			return table.MyTables;
		}

		public List<KeyValuePair<string,string>> GetColumns(string table)
		{
			SQLColumn column = new SQLColumn(connection.ConnectionString, table);
			column.GetColumns();
			return column.MyColumns;
		}
	}
}
