﻿using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
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

		public ConvertClass(string table, List<KeyValuePair<string, string>> columns)
		{
			Table = table;
			Columns = columns;
		}

		public ConvertClass(string @namespace, string table, List<KeyValuePair<string, string>> columns)
		{
			Namespace = @namespace;
			Table = table;
			Columns = columns;
		}

		public string GenerateUsings
		{
			get
			{
				StringBuilder builder = new StringBuilder();
				builder.AppendLine("using System;");
				return builder.ToString();
			}
		}

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

		private string GenerateStatements(int length)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				builder.AppendLine(string.Format(Statement, Columns[i].Key));
			}
			return builder.ToString();
		}

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

		public string GenerateToString
		{
			get
			{
				return string.Format(ToStr, Columns[0].Key);
			}
		}

		public override string ToString()
		{
			return string.Format(Full, GenerateUsings, Namespace, Table, GenerateProperties, GenerateConstructors, GenerateToString);
		}
	}
}