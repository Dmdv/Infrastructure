using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Annotations;
using Net.Common.Contracts;

namespace Net.MsAccess
{
	[PublicAPI]
	public class TableWriter : MdbTable
	{
		public TableWriter(string file)
			: base(file)
		{
		}

		/// <summary>
		/// 0 row никогда не используем.
		/// В таблицах нумерация только от 1.
		/// </summary>
		[PublicAPI]
		public void Write(string tablename, IList<Tuple<string, double>> parameters, int row)
		{
			Guard.CheckContainsText(tablename, "tablename");
			Guard.CheckNotEmpty(parameters, "parameters");
			Guard.CheckElementsNotNull(parameters, "parameters");
			Guard.CheckTrue(row > 0, () => new ArgumentException(@"Invalid row", "row"));

			InitCommand(tablename, parameters, row);

			using (var connection = CreateConnection())
			{
				using (Command)
				{
					// Command.CommandText = "update [table] set [column] = @var where Index = 1";
					connection.Open();
					Command.Connection = connection;
					Command.ExecuteNonQuery();
				}
			}
		}

		private OleDbCommand Command { get; set; }

		private static IEnumerable<OleDbParameter> CreateCommandParameters(IEnumerable<Tuple<string, double>> parameters)
		{
			return parameters
				.Select(tuple =>
					new OleDbParameter(
						string.Format("@{0}", tuple.Item1),
						tuple.Item2));
		}

		private static string CreateCommandText(string tablename, IEnumerable<Tuple<string, double>> parameters, int row)
		{
			var text = string.Format("update [{0}] set", tablename);

			var paramsText = parameters
				.Select(tuple => string.Format("[{0}] = @{1}", tuple.Item1, NormalizeString(tuple.Item1)))
				.Aggregate((acc, next) => string.Join(", ", acc, next));

			return string.Format("{0} {1} where Index = {2}", text, paramsText, row);
		}

		private static string NormalizeString(string str)
		{
			var rgx = new Regex("[^a-zA-Z0-9а-яА-Я]");
			return rgx.Replace(str, string.Empty).ToLowerInvariant();

			//return 
			//    str
			//    .Replace(" ", string.Empty)
			//    .Replace("(", string.Empty)
			//    .Replace(")", string.Empty)
			//    .Replace(",", string.Empty)
			//    .Replace(".", string.Empty)
			//    .Replace("-", string.Empty)
			//    .Replace("_", string.Empty)
			//    .Replace("%", string.Empty)
			//        .Trim()
			//        .ToLowerInvariant();
		}

		private void InitCommand(string tablename, IList<Tuple<string, double>> parameters, int row)
		{
			var cmdText = CreateCommandText(tablename, parameters, row);
			var dbParameters = CreateCommandParameters(parameters);
			Command = new OleDbCommand(cmdText);
			Command.Parameters.AddRange(dbParameters.ToArray());
		}
	}
}