using System;
using System.Text;
using Common.Contracts;

// ReSharper disable CodeCleanup
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace Common.Extensions
{
	public static class ExceptionExtensions
	{
		public static string GetExceptionMessages(this Exception exception)
		{
			Guard.CheckNotNull(exception, "exception");

			var messageBuilder = new StringBuilder(exception.Message);

			while (exception.InnerException != null)
			{
				messageBuilder.Append("--->");
				messageBuilder.Append(exception.InnerException.GetExceptionMessages());
				exception = exception.InnerException;
			}

			return messageBuilder.ToString();
		}

		//public static void RethrowForCriticalException(this Exception exception)
		//{
		//	Guard.CheckNotNull(exception, "exception");

		//	if (ExceptionHelper.IsCriticalException(exception))
		//	{
		//		throw exception;
		//	}
		//}
	}
}