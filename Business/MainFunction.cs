using DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
		public List<string> GetTables()
		{
			SQLTable table = new SQLTable(connection.ConnectionString);
			table.GetTables();
			return table.MyTables;
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
		public string GenerateClass(string @namespace, string table)
		{
			ConvertClass convert = new ConvertClass(@namespace, table, GetColumns(table));
			return convert.ToString();
		}

		/// <summary>
		/// Lưu nội dung lớp ra file vào một thư mục có tên của cơ sở dữ liệu
		/// </summary>
		/// <param name="path">Đường dẫn thư mục</param>
		/// <param name="filename">Tên bảng sẽ thành tên file</param>
		/// <param name="content">Nội dung lớp</param>
		public void SaveTextToFile(string path, string filename, string content)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			File.WriteAllText(path + "\\" + filename, content);
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

	}
}
