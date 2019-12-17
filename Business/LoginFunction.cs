using DataAccess;
using System.Collections.Generic;
using System.IO;

namespace Business
{
	/// <summary>
	/// Lớp cung cấp các thao tác cho form Login 
	/// </summary>
	public class LoginFunction
	{
		private SQLServer Server = new SQLServer();
		private SQLConnectionString SQLConnect;
		private readonly string pass = "lqt";
		private readonly string serverFile = "server.dat";
		private readonly string loginFile = "data.dat";

		public LoginFunction()
		{

		}

		/// <summary>
		/// Lấy tên các server từ hệ thống và từ file (nếu có)
		/// </summary>
		/// <returns>Danh sách các server</returns>
		public List<string> GetServers()
		{
			Server.GetServers();
			if (File.Exists(serverFile))
				Server.ReadFromFile(serverFile);
			return Server.MyServers;
		}

		/// <summary>
		/// Kiểm tra kết nối đến SQL Server
		/// </summary>
		/// <param name="server">Tên server</param>
		/// <returns>Kết nối thành công</returns>
		public bool TestConnection(string server)
		{
			SQLConnect = new SQLConnectionString(server);
			return SQLConnect.TestConnection();
		}

		/// <summary>
		/// Kiểm tra kết nối đến SQL Server
		/// </summary>
		/// <param name="server">Tên server</param>
		/// <param name="database">Tên cơ sở dữ liệu</param>
		/// <returns>Kết nối thành công</returns>
		public bool TestConnection(string server, string database)
		{
			SQLConnect = new SQLConnectionString(server, database);
			return SQLConnect.TestConnection();
		}

		/// <summary>
		/// Kiểm tra kết nối đến SQL Server
		/// </summary>
		/// <param name="server">Tên server</param>
		/// <param name="username">Tên đăng nhập</param>
		/// <param name="password">Mật khẩu</param>
		/// <returns>Kết nối thành công</returns>
		public bool TestConnection(string server, string username, string password)
		{
			SQLConnect = new SQLConnectionString(server, username, password);
			return SQLConnect.TestConnection();
		}

		/// <summary>
		/// Kiểm tra kết nối đến SQL Server
		/// </summary>
		/// <param name="server">Tên server</param>
		/// <param name="database">Tên cơ sở dữ liệu</param>
		/// <param name="username">Tên đăng nhập</param>
		/// <param name="password">Mật khẩu</param>
		/// <returns>Kết nối thành công</returns>
		public bool TestConnection(string server, string database, string username, string password)
		{
			SQLConnect = new SQLConnectionString(server, database, username, password);
			return SQLConnect.TestConnection();
		}

		/// <summary>
		/// Lưu trữ tên server để sau này dùng lại
		/// </summary>
		/// <param name="others">Danh sách server khác</param>
		public void SaveServers(List<string> others)
		{
			Server.WriteToFile(serverFile, others);
		}

		/// <summary>
		/// Lưu thông tin đăng nhập vào file đã được mã hóa
		/// </summary>
		/// <param name="username">Tên đăng nhập</param>
		/// <param name="password">Mật khẩu</param>
		public void SaveLoginInfo(string username, string password)
		{
			string encrypted = Crypto.Encrypt(username + " " + password, pass);
			File.WriteAllText(loginFile, encrypted);
		}

		/// <summary>
		/// Lấy thông tin đăng nhập đã được mã hóa từ file
		/// </summary>
		/// <returns>Chuỗi thông tin đăng nhập</returns>
		public string GetLoginInfo()
		{
			if (File.Exists(loginFile))
			{
				using (StreamReader reader = new StreamReader(loginFile))
				{
					string value = reader.ReadToEnd();
					try
					{
						return Crypto.Decrypt(value, pass);
					}
					catch (System.Exception)
					{
						return string.Empty;
					}
				}
			}
			else
				return string.Empty;
		}

	}
}
