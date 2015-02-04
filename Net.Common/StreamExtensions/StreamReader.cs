using System;
using System.IO;
using Common.Annotations;
using Net.Common.Contracts;

namespace Net.Common.StreamExtensions
{
	public static class StreamReader
	{
		public static void ProcessWhileRead(
			[NotNull] this TextReader reader,
			[NotNull] Action<TextReader, string> processor,
			[CanBeNull] Predicate<string> exitCondition = null)
		{
			Guard.CheckNotNull(reader, "reader");
			Guard.CheckNotNull(processor, "processor");

			string currentLine;

			while ((currentLine = reader.ReadLine()) != null)
			{
				if (exitCondition != null && exitCondition(currentLine))
				{
					break;
				}

				if (string.IsNullOrWhiteSpace(currentLine) || string.IsNullOrEmpty(currentLine))
				{
					continue;
				}

				processor(reader, currentLine);
			}
		}
	}
}