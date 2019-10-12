using Business;
using DataAccess;
using System;

namespace Testing
{
	class Program
	{
		static void Main()
		{
			SQLServer server = new SQLServer();
			server.GetServers();

			SQLConnectionString SQLConnect = new SQLConnectionString(@"DESKTOP-G3SCN6I\SQLEXPRESS", "QLNV");
			if (SQLConnect.TestConnection())
			{
				SQLTable table = new SQLTable(SQLConnect.ConnectionString);
				table.GetTables();

				SQLColumn column = new SQLColumn(SQLConnect.ConnectionString, "NhanVien");
				column.GetColumns();

				Console.Write("Nhap ten namespace: ");
				string @namespace = Console.ReadLine();

				ConvertClass convert = new ConvertClass(@namespace, "NhanVien", column.MyColumns);
				Console.WriteLine(convert.ToString());
			}
			else
			{
				Console.WriteLine("Connection: Error");
			}
			Console.ReadKey();
		}
	}
}
