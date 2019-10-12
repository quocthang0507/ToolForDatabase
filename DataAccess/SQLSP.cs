using System.Data.SqlClient;

namespace DataAccess
{
	/// <summary>
	/// Lớp SQL Stored Procedure
	/// cung cấp các phương thức liên quan đến thủ tục trong hệ quản trị cơ sở dữ liệu SQL Server 
	/// </summary>
	public class SQLSP
	{
		private SqlConnection Connection;

		public SQLSP()
		{

		}

		public SQLSP(string connectionString)
		{
			this.Connection = new SqlConnection(connectionString);
		}
	}
}
