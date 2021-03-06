﻿using System.Collections.Generic;
using System.Linq;
using Net.Common.Contracts;

namespace Net.Common.Patterns
{
	/// <summary>
	/// Implements <see cref="IHandler{TValue,TResult}"/> interface by aggregating a collection of handlers.
	/// First handler, which is able to handle wins and handles the <typeparamref name="TValue"/>.
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	public class FirstHandlingAggregatingHandler<TValue, TResult> : DelegatingHandler<TValue, TResult>
	{
		public FirstHandlingAggregatingHandler(IEnumerable<IHandler<TValue, TResult>> aggregatedHandlers)
			: base(
				value => aggregatedHandlers.Any(handler => handler.CanHandle(value)),
				value => aggregatedHandlers.First(handler => handler.CanHandle(value)).Handle(value))
		{
			Guard.CheckTrue(
				aggregatedHandlers.All(value => value != null),
				() => new ContractViolationException("aggregatedHandlers contains at least one null value."));
		}

		public FirstHandlingAggregatingHandler(
			params IHandler<TValue, TResult>[] aggregatedHandlers)
			: this((IEnumerable<IHandler<TValue, TResult>>)aggregatedHandlers)
		{
		}
	}
}
