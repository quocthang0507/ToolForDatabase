using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccess
{
	/// <summary>
	/// Lớp SQL Column
	/// cung cấp các phương thức liên quan đến các thuộc tính trong bảng
	/// </summary>
	public class SQLColumn
	{
		private SqlConnection Connection;
		private string Table { get; set; }

		/// <summary>
		/// Danh sách các cột / thuộc tính của bảng
		/// </summary>
		public List<KeyValuePair<string, string>> MyColumns { get; set; }

		/// <summary>
		/// Khởi tạo với chuỗi kết nối và tên bảng
		/// </summary>
		/// <param name="connection">Chuỗi kết nối</param>
		/// <param name="table">Tên bảng</param>
		public SQLColumn(string connection, string table)
		{
			this.Connection = new SqlConnection(connection);
			this.Table = table;
			MyColumns = new List<KeyValuePair<string, string>>();
		}

		/// <summary>
		/// Thêm một cột vào danh sách cột nếu chưa tồn tại
		/// </summary>
		/// <param name="column">Cột cần thêm</param>
		private void AddColumn(KeyValuePair<string, string> column)
		{
			if (!MyColumns.Contains(column))
				MyColumns.Add(column);
		}

		/// <summary>
		/// Lấy các cột và kiểu dữ liệu tương ứng trong bảng
		/// </summary>
		public void GetColumns()
		{
			Connection.Open();
			string[] restrictions = new string[4];
			restrictions[2] = Table;
			DataTable columns = Connection.GetSchema("Columns", restrictions);
			Connection.Close();
			foreach (DataRow item in columns.Rows)
			{
				AddColumn(new KeyValuePair<string, string>(item[3].ToString(), item[7].ToString()));
			}
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			foreach (var item in MyColumns)
			{
				builder.AppendLine(item.Key + "\t" + item.Value);
			}
			return builder.ToString();
		}

	}
}
