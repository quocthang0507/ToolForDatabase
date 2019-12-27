using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Business
{
	/// <summary>
	/// Lớp cung cấp các thao tác cho form Adding Values
	/// </summary>
	public class InsertFunction
	{
		private SqlConnection connection;
		private string insertCommand = "INSERT INTO {0} VALUES {1}";

		public InsertFunction(string serverName)
		{
			SQLConnectionString temp = new SQLConnectionString(serverName);
			this.connection = new SqlConnection(temp.ConnectionString);
		}

		public InsertFunction(string serverName, string loginName, string password)
		{
			SQLConnectionString temp = new SQLConnectionString(serverName, loginName, password);
			this.connection = new SqlConnection(temp.ConnectionString);
		}

		public InsertFunction(string serverName, string database, string loginName, string password)
		{
			SQLConnectionString temp = new SQLConnectionString(serverName, database, loginName, password);
			this.connection = new SqlConnection(temp.ConnectionString);
		}

		/// <summary>
		/// Trả về danh sách các cơ sở dữ liệu
		/// </summary>
		/// <returns></returns>
		public List<string> GetDatabases()
		{
			SQLDatabase database = new SQLDatabase(connection.ConnectionString);
			database.GetDatabases();
			return database.MyDatabases;
		}

		/// <summary>
		/// Lấy tên các bảng có trong cơ sở dữ liệu
		/// </summary>
		/// <returns>Danh sách các bảng</returns>
		public List<string> GetTables(string database)
		{
			SQLTable sqlTable = new SQLTable(connection.ConnectionString);
			sqlTable.GetTables();
			return sqlTable.MyTables;
		}

		/// <summary>
		/// Lấy tên và kiểu dữ liệu của các thuộc tính trong bảng
		/// </summary>
		/// <param name="table">Tên bảng</param>
		/// <returns>Danh sách tên và kiểu dữ liệu</returns>
		public List<KeyValuePair<string, string>> GetColumns(string table)
		{
			SQLColumn sqlColumn = new SQLColumn(connection.ConnectionString, table);
			sqlColumn.GetColumns();
			return sqlColumn.MyColumns;
		}

		/// <summary>
		/// Định nghĩa bảng DataView dựa vào bảng trong SQL
		/// </summary>
		/// <param name="columns"></param>
		/// <returns></returns>
		public DataView DefineTable(List<KeyValuePair<string, string>> columns)
		{
			DataTable table = new DataTable();
			foreach (var column in columns)
			{
				table.Columns.Add(new DataColumn(column.Key, DataType.MapToRealType(column.Value)));
			}
			return table.DefaultView;
		}

		/// <summary>
		/// Chèn nhiều dữ liệu vào bảng
		/// </summary>
		/// <param name="table"></param>
		/// <param name="dataView"></param>
		/// <returns></returns>
		public bool InsertValuesToTable(string table, DataView dataView)
		{
			connection.Open();
			var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
			var cmd = new SqlCommand();
			cmd.Transaction = transaction;
			try
			{
				string param = ListValuesToString(GetValuesFromDataView(dataView));
				cmd.CommandText = string.Format(insertCommand, table, param);
				cmd.Connection = connection;
				cmd.ExecuteNonQuery();
				transaction.Commit(); //All values will have been inserted
				connection.Close();
				return true;
			}
			catch (Exception e)
			{
				transaction.Rollback(); //No values will have been inserted
				connection.Close();
				return false;
			}
		}

		/// <summary>
		/// Lấy dữ liệu từ DataView
		/// </summary>
		/// <param name="dataView"></param>
		/// <returns></returns>
		private List<string> GetValuesFromDataView(DataView dataView)
		{
			List<String> list = new List<string>();
			DataTable dataTable = dataView.ToTable();
			foreach (DataRow row in dataTable.Rows)
			{
				string values = "(";
				for (int i = 0; i < dataTable.Columns.Count; i++)
				{
					var cell = row[i];
					values += "N'" + cell + "'";    //Unicode Text for all
					if (i < dataTable.Columns.Count - 1)
						values += ", ";
					else
						values += ")";
				}
				list.Add(values);
			}
			return list;
		}

		/// <summary>
		/// Đổi danh sách sang chuỗi theo cú pháp T-SQL
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		private string ListValuesToString(List<string> list)
		{
			string str = "";
			for (int i = 0; i < list.Count; i++)
			{
				str += list[i];
				if (i < list.Count - 1)
					str += ", ";
			}
			return str;
		}
	}
}
