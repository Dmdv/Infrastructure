using System;
using Net.Common.Contracts;
using Net.Common.Extensions;

namespace Net.Common.Patterns
{
	/// <summary>
	/// Implements <see cref="IHandler{TValue, TResult}"/> interface by delegating calls to its methods to the delegates passed to the constructor.
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	public class DelegatingHandler<TValue, TResult> : IHandler<TValue, TResult>
	{
		public DelegatingHandler(
			Func<TValue, bool> canHandle,
			Func<TValue, TResult> handle)
		{
			_canHandle = canHandle;
			_handle = handle;
		}

		public TResult Handle(TValue value)
		{
			if (!CanHandle(value))
			{
				throw new ContractViolationException(
					"Handler '{0}' can't handle value '{1}'."
						.FormatString(GetType().FullName, value));
			}

			return _handle(value);
		}

		public bool CanHandle(TValue value)
		{
			return _canHandle(value);
		}

		private readonly Func<TValue, bool> _canHandle;
		private readonly Func<TValue, TResult> _handle;
	}
}
