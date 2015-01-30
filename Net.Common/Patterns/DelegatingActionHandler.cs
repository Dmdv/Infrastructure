using System;
using Net.Common.Contracts;
using Net.Common.Extensions;

namespace Net.Common.Patterns
{
	/// <summary>
	/// Implements <see cref="IHandler{TValue}"/> interface by delegating calls to its methods to the delegates passed to the constructor.
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	public class DelegatingHandler<TValue> : IHandler<TValue>
	{
		public DelegatingHandler(
			Func<TValue, bool> canHandle,
			Action<TValue> handle)
		{
			_canHandle = canHandle;
			_handle = handle;
		}

		public void Handle(TValue value)
		{
			if (!CanHandle(value))
			{
				throw new ContractViolationException(
					"Handler '{0}' can't handle value '{1}'."
						.FormatString(GetType().FullName, value));
			}

			_handle(value);
		}

		public bool CanHandle(TValue value)
		{
			return _canHandle(value);
		}

		private readonly Func<TValue, bool> _canHandle;
		private readonly Action<TValue> _handle;
	}
}
