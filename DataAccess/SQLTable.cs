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
		private SqlConnection Connection;

		/// <summary>
		/// Danh sách các bảng có trong cơ sở dữ liệu
		/// </summary>
		public List<string> MyTables { get; set; }

		/// <summary>
		/// Khởi tạo lớp với chuỗi kết nối
		/// </summary>
		/// <param name="connection">Chuỗi kết nối</param>
		public SQLTable(string connection)
		{
			this.Connection = new SqlConnection(connection);
			this.MyTables = new List<string>();
		}

		/// <summary>
		/// Thêm một bảng
		/// </summary>
		/// <param name="table">Bảng cần thêm</param>
		private void AddTable(string table)
		{
			if (!MyTables.Contains(table))
				MyTables.Add(table);
		}

		/// <summary>
		/// Lấy tất cả các bảng có trong cơ sở dữ liệu
		/// </summary>
		public void GetTables()
		{
			Connection.Open();
			DataTable tables = Connection.GetSchema("Tables");
			Connection.Close();
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
