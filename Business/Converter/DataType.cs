﻿using System;
using System.Web.UI.WebControls;

namespace DataAccess
{
	/// <summary>
	/// Lớp chuyển đổi kiểu dữ liệu SqlDbType sang .NET Type
	/// </summary>
	public class DataType
	{
		/// <summary>
		/// Chuyển đổi kiểu dữ liệu SqlDbType sang .NET Type
		/// <para>Tham khảo: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings </para>
		/// </summary>
		/// <param name="SqlDbType">SQL Server Database Engine type</param>
		/// <returns>.NET Framework type</returns>
		public static string MapToOriginalType(string SqlDbType)
		{
			switch (SqlDbType)
			{
				case "bit":
					return "Boolean";
				case "tinyint":
					return "Byte";
				case "binary":
				case "varbinary(max)":
				case "image":
				case "rowversion":
				case "timestamp":
				case "varbinary":
					return "Byte[]";
				case "date":
				case "datetime":
				case "datetime2":
				case "smalldatetime":
					return "DateTime";
				case "datetimeoffset":
					return "DateTimeOffset";
				case "decimal":
				case "money":
				case "numeric":
				case "smallmoney":
					return "Decimal";
				case "float":
					return "Double";
				case "uniqueidentifier":
					return "Guid";
				case "smallint":
					return "Int16";
				case "int":
					return "Int32";
				case "bigint":
					return "Int64";
				case "real":
					return "Single";
				case "char":
				case "nchar":
				case "nvarchar":
				case "text":
				case "varchar":
				case "ntext":
					return "String";
				case "time":
					return "TimeSpan";
				case "xml":
					return "Xml";
				default:
					return "";
			}
		}

		/// <summary>
		/// Chuyển đổi kiểu dữ liệu SqlDbType sang .NET Type
		/// <para>Tham khảo: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings </para>
		/// </summary>
		/// <param name="SqlDbType">SQL Server Database Engine type</param>
		/// <returns>.NET Framework type</returns>
		public static string MapToNormalType(string SqlDbType)
		{
			switch (SqlDbType)
			{
				case "bit":
					return "bool";
				case "tinyint":
					return "byte";
				case "binary":
				case "varbinary(max)":
				case "image":
				case "rowversion":
				case "timestamp":
				case "varbinary":
					return "byte[]";
				case "date":
				case "datetime":
				case "datetime2":
				case "smalldatetime":
					return "DateTime";
				case "datetimeoffset":
					return "DateTimeOffset";
				case "decimal":
				case "money":
				case "numeric":
				case "smallmoney":
					return "decimal";
				case "float":
					return "double";
				case "uniqueidentifier":
					return "Guid";
				case "smallint":
					return "int";
				case "int":
					return "int";
				case "bigint":
					return "long";
				case "real":
					return "float";
				case "char":
				case "nchar":
				case "nvarchar":
				case "text":
				case "varchar":
				case "ntext":
					return "string";
				case "time":
					return "TimeSpan";
				case "xml":
					return "Xml";
				default:
					return "object";
			}
		}

		/// <summary>
		/// Chuyển đổi kiểu dữ liệu SqlDbType sang .NET Type
		/// <para>Tham khảo: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings </para>
		/// </summary>
		/// <param name="SqlDbType">SQL Server Database Engine type</param>
		/// <returns>.NET Framework type</returns>
		public static Type MapToRealType(string SqlDbType)
		{
			switch (SqlDbType)
			{
				case "bit":
					return typeof(bool);
				case "tinyint":
					return typeof(byte);
				case "binary":
				case "varbinary(max)":
				case "image":
				case "rowversion":
				case "timestamp":
				case "varbinary":
					return typeof(byte[]);
				case "date":
				case "datetime":
				case "datetime2":
				case "smalldatetime":
					return typeof(DateTime);
				case "datetimeoffset":
					return typeof(DateTimeOffset);
				case "decimal":
				case "money":
				case "numeric":
				case "smallmoney":
					return typeof(decimal);
				case "float":
					return typeof(double);
				case "uniqueidentifier":
					return typeof(Guid);
				case "smallint":
					return typeof(int);
				case "int":
					return typeof(int);
				case "bigint":
					return typeof(long);
				case "real":
					return typeof(float);
				case "char":
				case "nchar":
				case "nvarchar":
				case "text":
				case "varchar":
				case "ntext":
					return typeof(string);
				case "time":
					return typeof(TimeSpan);
				case "xml":
					return typeof(Xml);
				default:
					return typeof(object);
			}
		}

	}
}
