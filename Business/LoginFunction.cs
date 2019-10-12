﻿using DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO.IsolatedStorage;

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
		/// Load các server từ hệ thống và từ file (nếu có)
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
		/// Kiểm tra kết nối
		/// </summary>
		/// <param name="server">Tên server</param>
		/// <returns>Kết nối thành công</returns>
		public bool TestConnection(string server)
		{
			SQLConnect = new SQLConnectionString(server);
			return SQLConnect.TestConnection();
		}

		/// <summary>
		/// Kiểm tra kết nối
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
		/// Kiểm tra kết nối
		/// </summary>
		/// <param name="server">Tên server</param>
		/// <param name="username">Tên đăng nhập</param>
		/// <param name="password">Mật khẩu</param>
		/// <returns></returns>
		public bool TestConnection(string server, string username, string password)
		{
			SQLConnect = new SQLConnectionString(server, username, password);
			return SQLConnect.TestConnection();
		}

		/// <summary>
		/// Kiểm tra kết nối
		/// </summary>
		/// <param name="server">Tên server</param>
		/// <param name="database">Tên cơ sở dữ liệu</param>
		/// <param name="username">Tên đăng nhập</param>
		/// <param name="password">Mật khẩu</param>
		/// <returns></returns>
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
		/// Lấy danh sách tất cả các cơ sở dữ liệu có trong server
		/// </summary>
		/// <returns>Danh sách tên cơ sở dữ liệu</returns>
		public List<string> GetDatabases()
		{
			SQLDatabase database = new SQLDatabase(SQLConnect.ConnectionString);
			database.GetDatabases();
			return database.MyDatabases;
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
			string data = string.Empty;
			if (File.Exists(loginFile))
			{
				using (StreamReader reader = new StreamReader(loginFile))
				{
					string value = reader.ReadToEnd();
					return Crypto.Decrypt(value, pass);
				}
			}
			else
				return data;
		}

		/// <summary>
		/// Lấy chuỗi kết nối hiện tại
		/// </summary>
		/// <returns></returns>
		public string GetSQLConnectionString()
		{
			return SQLConnect.ConnectionString;
		}
	}
}