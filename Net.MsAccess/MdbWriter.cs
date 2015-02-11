using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Annotations;
using Net.Common.Contracts;
using Net.Common.Extensions;

namespace Net.MsAccess
{
	[PublicAPI]
	public class MdbWriter : MdbTable
	{
		public MdbWriter(string file)
			: base(file)
		{
		}

		/// <summary>
		/// Creates mdb database.
		/// </summary>
		/// <param name="path"></param>
		[PublicAPI]
		public static MdbWriter Create(string path)
		{
			Guard.CheckContainsText(path, "path");
			var extension = Path.GetExtension(path);
			Guard.CheckContainsText(extension, "extension");
			// ReSharper disable once PossibleNullReferenceException
			Guard.CheckTrue(extension.Equals(".mdb"), () => new ArgumentException("File must end with .mdb extensions"));

			if (File.Exists(path))
			{
				return new MdbWriter(path);
			}

			using (File.Create(path))
			{
				return new MdbWriter(path);
			}
		}

		/// <summary>
		/// 0 row will never use.
		/// В таблицах нумерация только от 1.
		/// </summary>
		[PublicAPI]
		public void Update<TParameter>(
			string tablename,
			IList<Tuple<string, TParameter>> parameters,
			int row)
		{
			Guard.CheckContainsText(tablename, "tablename");
			Guard.CheckNotEmpty(parameters, "parameters");
			Guard.CheckElementsNotNull(parameters, "parameters");
			Guard.CheckTrue(row > 0, () => new ArgumentException(@"Invalid row", "row"));

			InitUpdateCommand(tablename, parameters, row);

			using (var connection = CreateConnection())
			{
				using (Command)
				{
					connection.Open();
					Command.Connection = connection;
					Command.ExecuteNonQuery();
				}
			}
		}

		private OleDbCommand Command { get; set; }

		private static IEnumerable<OleDbParameter> CreateCommandParameters<TParameter>(
			IEnumerable<Tuple<string, TParameter>> parameters)
		{
			return parameters
				.Select(tuple =>
					new OleDbParameter(
						"@{0}".FormatString(tuple.Item1),
						tuple.Item2));
		}

		private static string CreateUpdateCommand<TParameter>(
			string tablename,
			IEnumerable<Tuple<string, TParameter>> parameters,
			int row)
		{
			var paramsText = parameters
				.Select(tuple => "[{0}] = @{1}".FormatString(tuple.Item1, NormalizeString(tuple.Item1)))
				.Aggregate((acc, next) => string.Join(", ", acc, next));

			return
				"update [{0}] set {1} where Index = {2}"
					.FormatString(tablename, paramsText, row);
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

		private void InitUpdateCommand<TParameter>(string tablename, IList<Tuple<string, TParameter>> parameters, int row)
		{
			var command = CreateUpdateCommand(tablename, parameters, row);
			var dbParameters = CreateCommandParameters(parameters);
			Command = new OleDbCommand(command);
			Command.Parameters.AddRange(dbParameters.ToArray());
		}
	}
}