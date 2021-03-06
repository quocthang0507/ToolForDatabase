﻿using Business.Other;
using DataAccess;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Business
{
	/// <summary>
	/// Lớp cung cấp các phương thức tạo dựng lớp C#
	/// </summary>
	public class ClassConverter
	{
		private string full = "{0}\nnamespace {1}\n{{\n\tpublic class {2}\n\t{{\n{3}\n{4}{5}\n\t}}\n}}";
		private string @namespace = "DataAccess";
		private string table;
		private List<KeyValuePair<string, string>> columns; //Danh sách toàn bộ các cột
		private List<KeyValuePair<string, string>> selectedColumns; //Danh sách các cột chọn làm phương thức tạo lập
		private string attribute = "\t\tpublic {0} {1} {{ get; set; }}";
		private string contructor = "\t\tpublic {0}({1})\n\t\t{{\n{2}\t\t}}\n";
		private string parameter = "{0} {1}";
		private string statement = "\t\t\tthis.{0} = {0};";
		private string toStr = "\t\tpublic override string ToString()\n\t\t{{\n\t\t\treturn {0};\n\t\t}}";


		/// <summary>
		/// Khởi tạo lớp
		/// </summary>
		/// <param name="table">Tên bảng</param>
		/// <param name="columns">Tên thuộc tính</param>
		public ClassConverter(string @namespace, string table)
		{
			this.@namespace = @namespace;
			this.table = table;
		}

		/// <summary>
		/// Khởi tạo lớp
		/// </summary>
		/// <param name="namespace"></param>
		/// <param name="table">Tên bảng</param>
		/// <param name="columns">Tên thuộc tính</param>
		/// <param name="selectedColumns"></param>
		public ClassConverter(string @namespace, string table, List<KeyValuePair<string, string>> columns, List<KeyValuePair<string, string>> selectedColumns)
		{
			this.@namespace = @namespace;
			this.table = table;
			this.columns = columns;
			this.selectedColumns = selectedColumns;
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
				foreach (var attribute in columns)
				{
					builder.AppendLine(string.Format(this.attribute, DataType.MapToNormalType(attribute.Value), attribute.Key));
				}
				return builder.ToString();
			}
		}

		/// <summary>
		/// Tạo ra các một loạt các tham số cho phương thức tạo lập tùy theo số lượng cần
		/// </summary>
		/// <param name="length">Số lượng tham số</param>
		/// <returns></returns>
		private string GenerateParameters()
		{
			string param = "";
			for (int i = 0; i < selectedColumns.Count; i++)
			{
				param += string.Format(parameter, DataType.MapToNormalType(selectedColumns[i].Value), selectedColumns[i].Key);
				if (i < selectedColumns.Count - 1)
					param += ", ";
			}
			return param;
		}

		/// <summary>
		/// Tạo ra một loạt các lệnh khởi tạo thuộc tính của lớp tùy theo số lượng cần
		/// </summary>
		/// <param name="length">Số lượng câu lệnh</param>
		/// <returns></returns>
		private string GenerateStatements()
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < selectedColumns.Count; i++)
			{
				builder.AppendLine(string.Format(statement, selectedColumns[i].Key));
			}
			return builder.ToString();
		}

		/// <summary>
		/// Tạo ra 2 phương thức khởi tạo lớp (phương thức tạo lập mặc định và phương thức tạo lập tùy chỉnh)
		/// </summary>
		public string GenerateConstructors
		{
			get
			{
				StringBuilder builder = new StringBuilder();
				builder.AppendLine(string.Format(contructor, table, "", "")); //Phương thức trống
																			   //Phương thức tạo lập với những cột đã được chọn
				if (selectedColumns.Count > 0)
				{
					string param = GenerateParameters();
					string statement = GenerateStatements();
					builder.AppendLine(string.Format(contructor, table, param, statement));
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
				string tostr = "";
				foreach (var col in selectedColumns)
				{
					tostr += col.Key + ".ToString()";
					if (selectedColumns.IndexOf(col) < selectedColumns.Count - 1)
						tostr += " + \"\\t\" + ";
				}
				return string.Format(toStr, tostr);
			}
		}

		/// <summary>
		/// Xuất toàn bộ nội dung lớp ra chuỗi
		/// </summary>
		/// <returns>Nội dung lớp</returns>
		public override string ToString()
		{
			return string.Format(full, GenerateUsings, @namespace, table, GenerateProperties, GenerateConstructors, GenerateToString);
		}

	}
}
