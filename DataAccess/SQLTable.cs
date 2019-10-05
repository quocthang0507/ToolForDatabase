using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccess
{
	/// <summary>
	/// Lớp SQL Table
	/// cung cấp các phương thức liên quan đến thủ tục trong hệ quản trị cơ sở dữ liệu SQL Server 
	/// </summary>
	public class SQLTable
	{
		private SqlConnection connection;
		public List<string> MyTables { get; set; }

		public SQLTable()
		{
			this.MyTables = new List<string>();
		}

		public SQLTable(string connectionString)
		{
			this.connection = new SqlConnection(connectionString);
			this.MyTables = new List<string>();
		}

		public void FindTables()
		{
			connection.Open();
			DataTable table = connection.GetSchema("Tables");
			connection.Close();
			foreach (DataRow item in table.Rows)
			{
				MyTables.Add(item[2].ToString());
			}
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();
			foreach (var item in MyTables)
			{
				result.AppendLine(item);
			}
			return result.ToString();
		}
	}
}
