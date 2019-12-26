using Business.Other;
using DataAccess;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using TreeView;

namespace Business
{
	/// <summary>
	/// Lớp cung cấp các thao tác cho form Main 
	/// </summary>
	public class MainFunction
	{
		private SqlConnection connection;

		public MainFunction(string serverName)
		{
			SQLConnectionString temp = new SQLConnectionString(serverName);
			this.connection = new SqlConnection(temp.ConnectionString);
		}

		public MainFunction(string serverName, string loginName, string password)
		{
			SQLConnectionString temp = new SQLConnectionString(serverName, loginName, password);
			this.connection = new SqlConnection(temp.ConnectionString);
		}

		public MainFunction(string serverName, string database, string loginName, string password)
		{
			SQLConnectionString temp = new SQLConnectionString(serverName, database, loginName, password);
			this.connection = new SqlConnection(temp.ConnectionString);
		}

		/// <summary>
		/// Lấy tên các bảng có trong cơ sở dữ liệu
		/// </summary>
		/// <returns>Danh sách các bảng</returns>
		public List<TreeViewModel> GetDetailTable(string database)
		{
			SQLTable sqlTable = new SQLTable(connection.ConnectionString);
			sqlTable.GetTables();
			var tables = sqlTable.MyTables;
			List<TreeViewModel> treeView = new List<TreeViewModel>();
			TreeViewModel root = new TreeViewModel(database);
			treeView.Add(root);
			foreach (var table in tables) // Table
			{
				SQLColumn sqlColumn = new SQLColumn(connection.ConnectionString, table);
				sqlColumn.GetColumns();
				var columns = sqlColumn.MyColumns;
				TreeViewModel tvTable = new TreeViewModel(table);
				root.Children.Add(tvTable);
				foreach (var column in columns) //Column
				{
					tvTable.Children.Add(new TreeViewModel(column.Key));
				}
			}
			root.Initialize();
			return treeView;
		}

		/// <summary>
		/// Lấy tên và kiểu dữ liệu của các thuộc tính trong bảng
		/// </summary>
		/// <param name="table">Tên bảng</param>
		/// <returns>Danh sách tên và kiểu dữ liệu</returns>
		public List<KeyValuePair<string, string>> GetColumns(string table)
		{
			SQLColumn column = new SQLColumn(connection.ConnectionString, table);
			column.GetColumns();
			return column.MyColumns;
		}

		/// <summary>
		/// Tạo ra lớp tương ứng với bảng
		/// </summary>
		/// <param name="namespace">Tên namespace (tùy chỉnh)</param>
		/// <param name="table">Tên bảng sẽ thành tên lớp</param>
		/// <returns></returns>
		public string GenerateClass(List<TreeViewModel> treeView, string @namespace, string table)
		{
			var referencee = GetColumns(table); //Danh sách cột ban đầu 
			var converted = ConvertSingleListToPairList(referencee, GetSelectedColumnsInTables(treeView, table)); //Danh sách cột được chọn
			ClassConverter convert = new ClassConverter(@namespace, table, referencee, converted);
			return convert.ToString();
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
		/// Trả về các bảng được chọn
		/// </summary>
		/// <returns></returns>
		public List<string> GetSelectedTables(List<TreeViewModel> treeView)
		{
			List<string> list = new List<string>();
			var root = treeView[0]; //Root (database)
			foreach (var table in root.Children) //Children (tables in database)
			{
				if (table.IsChecked == true || table.IsChecked == null)
					list.Add(table.Name);
			}
			return list;
		}

		/// <summary>
		/// Trả về các cột được chọn trong bảng
		/// </summary>
		/// <param name="treeView"></param>
		/// <param name="table"></param>
		/// <returns></returns>
		public List<string> GetSelectedColumnsInTables(List<TreeViewModel> treeView, string table)
		{
			List<string> list = new List<string>();
			var root = treeView[0]; //Root (database)
			foreach (var item in root.Children)
			{
				if (item.Name == table)
				{
					foreach (var column in item.Children)
					{
						if (column.IsChecked == true)
							list.Add(column.Name);
					}
				}
			}
			return list;
		}

		/// <summary>
		/// Chuyển đổi danh sách các cột được chọn ở dạng string sang danh sách các cột dạng KeyValuePair
		/// </summary>
		/// <param name="referencee">Danh sách được tham chiếu: Danh sách đầy đủ</param>
		/// <param name="reference">Danh sách cần tham chiếu: Danh sách được chọn</param>
		/// <returns></returns>
		public List<KeyValuePair<string, string>> ConvertSingleListToPairList(List<KeyValuePair<string, string>> referencee, List<string> reference)
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			foreach (var item in reference)
			{
				var result = referencee.Where(x => x.Key == item).FirstOrDefault();
				list.Add(result);
			}
			return list;
		}
	}
}
