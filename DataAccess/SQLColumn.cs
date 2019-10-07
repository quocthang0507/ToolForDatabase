using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	/// <summary>
	/// Lớp SQL Column
	/// cung cấp các phương thức liên quan đến các thuộc tính trong bảng
	/// </summary>
	public class SQLColumn
	{
		private SqlConnection connection;
		private string table { get; set; }
		public List<KeyValuePair<string, string>> MyColumns { get; set; }

		public SQLColumn(SQLConnectionString SQLConnect, string table)
		{
			this.connection = new SqlConnection(SQLConnect.ConnectionString);
			this.table = table;
			MyColumns = new List<KeyValuePair<string, string>>();
		}

		/// <summary>
		/// Thêm một cột
		/// </summary>
		/// <param name="column">Cột cần thêm</param>
		public void AddColumn(KeyValuePair<string, string> column)
		{
			if (!MyColumns.Contains(column))
				MyColumns.Add(column);
		}

		/// <summary>
		/// Lấy các cột và kiểu dữ liệu tương ứng trong bảng
		/// </summary>
		public void GetColumns()
		{
			connection.Open();
			string[] restrictions = new string[4];
			restrictions[2] = table;
			DataTable columns = connection.GetSchema("Columns", restrictions);
			connection.Close();
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
