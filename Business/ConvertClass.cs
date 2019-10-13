using DataAccess;
using System.Collections.Generic;
using System.Text;

namespace Business
{
	/// <summary>
	/// Lớp cung cấp các phương thức tạo dựng lớp C#
	/// </summary>
	public class ConvertClass
	{
		private string Full = "{0}\nnamespace {1}\n{{\n\tpublic class {2}\n\t{{\n{3}\n{4}{5}\n\t}}\n}}";
		private string Namespace = "DataAccess";
		private string Table;
		private List<KeyValuePair<string, string>> Columns;
		private string Field = "\t\tpublic {0} {1} {{ get; set; }}";
		private string Constructor = "\t\tpublic {0}({1})\n\t\t{{\n{2}\t\t}}\n";
		private string Parameter = "{0} {1}";
		private string Statement = "\t\t\tthis.{0} = {0};";
		private string ToStr = "\t\tpublic override string ToString()\n\t\t{{\n\t\t\treturn {0}.ToString();\n\t\t}}";

		/// <summary>
		/// Khởi tạo lớp
		/// </summary>
		/// <param name="table">Tên bảng</param>
		/// <param name="columns">Tên thuộc tính</param>
		public ConvertClass(string table, List<KeyValuePair<string, string>> columns)
		{
			Table = table;
			Columns = columns;
		}

		/// <summary>
		/// Khởi tạo lớp
		/// </summary>
		/// <param name="namespace">Tên namespace (tùy chỉnh)</param>
		/// <param name="table">Tên bảng</param>
		/// <param name="columns">Tên thuộc tính</param>
		public ConvertClass(string @namespace, string table, List<KeyValuePair<string, string>> columns)
		{
			Namespace = @namespace;
			Table = table;
			Columns = columns;
		}

		/// <summary>
		/// Tạo các chỉ thị Using
		/// </summary>
		public string GenerateUsings
		{
			get
			{
				StringBuilder builder = new StringBuilder();
				builder.AppendLine("using System;");
				return builder.ToString();
			}
		}

		/// <summary>
		/// Tạo các trường (thuộc tính) của lớp
		/// </summary>
		public string GenerateProperties
		{
			get
			{
				StringBuilder builder = new StringBuilder();
				foreach (var attribute in Columns)
				{
					builder.AppendLine(string.Format(Field, DataType.Mapping(attribute.Value), attribute.Key));
				}
				return builder.ToString();
			}
		}

		/// <summary>
		/// Tạo ra các một loạt các tham số cho phương thức tạo lập tùy theo số lượng cần
		/// </summary>
		/// <param name="length">Số lượng tham số</param>
		/// <returns></returns>
		private string GenerateParameters(int length)
		{
			string param = "";
			for (int i = 0; i < length; i++)
			{
				param += string.Format(Parameter, DataType.Mapping(Columns[i].Value), Columns[i].Key);
				if (i < length - 1)
					param += ", ";
			}
			return param;
		}

		/// <summary>
		/// Tạo ra một loạt các lệnh khởi tạo thuộc tính của lớp tùy theo số lượng cần
		/// </summary>
		/// <param name="length">Số lượng câu lệnh</param>
		/// <returns></returns>
		private string GenerateStatements(int length)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				builder.AppendLine(string.Format(Statement, Columns[i].Key));
			}
			return builder.ToString();
		}

		/// <summary>
		/// Tạo ra một loạt các phương thức khởi tạo của lớp
		/// </summary>
		public string GenerateConstructors
		{
			get
			{
				StringBuilder builder = new StringBuilder();
				builder.AppendLine(string.Format(Constructor, Table, "", ""));
				for (int i = 1; i <= Columns.Count; i++)
				{
					string param = GenerateParameters(i);
					string statement = GenerateStatements(i);
					builder.AppendLine(string.Format(Constructor, Table, param, statement));
				}
				return builder.ToString();
			}
		}

		/// <summary>
		/// Tạo ra phương thức ToString
		/// </summary>
		public string GenerateToString
		{
			get
			{
				return string.Format(ToStr, Columns[0].Key);
			}
		}

		/// <summary>
		/// Xuất toàn bộ nội dung lớp ra chuỗi
		/// </summary>
		/// <returns>Nội dung lớp</returns>
		public override string ToString()
		{
			return string.Format(Full, GenerateUsings, Namespace, Table, GenerateProperties, GenerateConstructors, GenerateToString);
		}
	}
}
