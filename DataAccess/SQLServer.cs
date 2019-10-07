using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccess
{
	/// <summary>
	/// Lớp SQL Server
	/// cung cấp các phương thức trên hệ quản trị cơ sở dữ liệu SQL Server 
	/// </summary>
	public class SQLServer
	{
		public List<string> MyServers { get; set; }

		public SQLServer()
		{
			MyServers = new List<string>();
		}

		/// <summary>
		/// Thêm một server
		/// </summary>
		/// <param name="server">Server cần thêm</param>
		public void AddServer(string server)
		{
			if (!MyServers.Contains(server))
				MyServers.Add(server);
		}

		/// <summary>
		/// Lấy các SQL Server trên máy (không đầy đủ)
		/// </summary>
		public void GetServers()
		{
			string ComputerName = Environment.MachineName;
			RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
			using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
			{
				RegistryKey InstanceName = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
				if (InstanceName != null)
				{
					foreach (var instanceName in InstanceName.GetValueNames())
					{
						AddServer(ComputerName + "\\" + instanceName);
					}
				}
			}
		}

		/// <summary>
		/// Đọc tên SQL Server từ file
		/// </summary>
		/// <param name="path">Đường dẫn đến file</param>
		public void ReadFromFile(string path)
		{
			using (var reader = new StreamReader(path))
			{
				while (!reader.EndOfStream)
				{
					string name = reader.ReadLine();
					AddServer(name);
				}
			}
		}

		/// <summary>
		/// Ghi tên SQL Server ra file
		/// </summary>
		/// <param name="path">Đường dẫn đến file</param>
		public void WriteToFile(string path)
		{
			StringBuilder data = new StringBuilder();
			foreach (var item in this.MyServers)
			{
				data.AppendLine(item);
			}
			File.WriteAllText(path, data.ToString()); //Ghi đè nếu không tồn tại!
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();
			foreach (var item in MyServers)
			{
				result.AppendLine(item);
			}
			return result.ToString();
		}
	}
}
