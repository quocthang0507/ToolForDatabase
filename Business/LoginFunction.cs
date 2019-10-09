using DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
	/// <summary>
	/// Lớp cung cấp các thao tác cho form Login 
	/// </summary>
	public class LoginFunction
	{
		private SQLServer server = new SQLServer();

		public LoginFunction()
		{

		}

		/// <summary>
		/// Load các server từ hệ thống và từ file (nếu có)
		/// </summary>
		/// <returns>Danh sách các server</returns>
		public List<string> LoadServer()
		{
			server.GetServers();
			LoadServerFromFile();
			return server.MyServers;
		}

		/// <summary>
		/// Kiểm tra kết nối
		/// </summary>
		/// <param name="server">Tên server</param>
		/// <returns>Kết nối thành công</returns>
		public bool TestConnection(string server)
		{
			SQLConnectionString connection = new SQLConnectionString(server);
			return connection.TestConnection();
		}

		/// <summary>
		/// Kiểm tra kết nối
		/// </summary>
		/// <param name="server">Tên server</param>
		/// <param name="username">Tên đăng nhập</param>
		/// <param name="password">Mật khẩu</param>
		/// <returns></returns>
		public bool TestConnection(string server, string username, string password)
		{
			SQLConnectionString connection = new SQLConnectionString(server, "master", username, password);
			return connection.TestConnection();
		}

		/// <summary>
		/// Lưu trữ tên server để sau này dùng lại
		/// </summary>
		/// <param name="others">Danh sách server khác</param>
		public void SaveServer(List<string> others)
		{
			server.WriteToFile("data.txt", others);
		}

		/// <summary>
		/// Load danh sách server vào file
		/// </summary>
		public void LoadServerFromFile()
		{
			if (File.Exists("data.txt"))
				server.ReadFromFile("data.txt");
		}
	}
}
