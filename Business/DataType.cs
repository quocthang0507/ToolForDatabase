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
		public static string Mapping(string SqlDbType)
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
					return "String";
				case "time":
					return "TimeSpan";
				case "xml":
					return "Xml";
				default:
					return "";
			}
		}
	}
}
