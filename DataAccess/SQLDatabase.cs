﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	/// <summary>
	/// Lớp SQL Database
	/// cung cấp các phương thức liên quan đến cơ sở dữ liệu trong hệ quản trị cơ sở dữ liệu SQL Server
	/// </summary>
	public class SQLDatabase
	{
		private SqlConnection Connection;
		public List<string> MyDatabases { get; set; }

		/// <summary>
		/// Khởi tạo lớp với chuỗi kết nối
		/// </summary>
		/// <param name="connection"></param>
		public SQLDatabase(string connection)
		{
			this.Connection = new SqlConnection(connection);
			MyDatabases = new List<string>();
		}

		/// <summary>
		/// Thêm một cơ sở dữ liệu
		/// </summary>
		/// <param name="database">Cơ sở dữ liệu cần thêm</param>
		public void AddDatabase(string database)
		{
			if (!MyDatabases.Contains(database))
				MyDatabases.Add(database);
		}

		/// <summary>
		/// Lấy tất cả các cơ sở dữ liệu có trong server
		/// </summary>
		public void GetDatabases()
		{
			Connection.Open();
			DataTable databases = Connection.GetSchema("Databases");
			Connection.Close();
			foreach (DataRow item in databases.Rows)
			{
				AddDatabase(item.Field<string>("database_name"));
			}
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			foreach (var item in MyDatabases)
			{
				builder.AppendLine(item);
			}
			return builder.ToString();
		}
	}
}