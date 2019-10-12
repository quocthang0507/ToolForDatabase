using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
	public class ConvertClass
	{
		private string Namespace = "DataAccess";
		private string Table;
		private List<KeyValuePair<string, string>> Columns;
		private string Field = "\t\tpublic {0} {1} {{ get; set; }}";
		private string Constructor = "\t\tpublic {0}({1})\n\t\t{{\n{2}\t\t}}\n";
		private string Parameter = "{0} {1}";
		private string Statement = "\t\t\tthis.{0} = {0};";

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

		public string GenerateUsings()
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("using System");
			return builder.ToString();
		}

		public string GenerateProperties()
		{
			StringBuilder builder = new StringBuilder();
			foreach (var attribute in Columns)
			{
				builder.AppendLine(string.Format(Field, attribute.Key, DataType.Mapping(attribute.Value)));
			}
			return builder.ToString();
		}

		public string GenerateParameters(int length)
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

		public string GenerateStatements(int length)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				builder.AppendLine(string.Format(Statement, Columns[i].Key));
			}
			return builder.ToString();
		}

		public string GenerateConstructors()
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
}
