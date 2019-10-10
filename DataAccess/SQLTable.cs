using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccess
{
	/// <summary>
	/// Lớp SQL Table
	/// cung cấp các phương thức liên quan đến bảng trong hệ quản trị cơ sở dữ liệu SQL Server
	/// </summary>
	public class SQLTable
	{
		private SqlConnection connection;
		public List<string> MyTables { get; set; }

		/// <summary>
		/// Khởi tạo lớp với chuỗi kết nối
		/// </summary>
		/// <param name="connection"></param>
		public SQLTable(string connection)
		{
			this.connection = new SqlConnection(connection);
			this.MyTables = new List<string>();
		}

		/// <summary>
		/// Thêm một bảng
		/// </summary>
		/// <param name="table">Bảng cần thêm</param>
		public void AddTable(string table)
		{
			if (!MyTables.Contains(table))
				MyTables.Add(table);
		}

		/// <summary>
		/// Lấy tất cả các bảng có trong cơ sở dữ liệu
		/// </summary>
		public void GetTables()
		{
			connection.Open();
			DataTable tables = connection.GetSchema("Tables");
			connection.Close();
			foreach (DataRow item in tables.Rows)
			{
				AddTable(item[2].ToString());
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
