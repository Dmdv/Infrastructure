﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Net.Common.Extensions
{
	public static class ReflectionExtensions
	{
		/// <summary>
		/// Given a lambda expression that calls a method, returns the method info.
		/// </summary>
		/// <param name="expression">The expression.</param>
		public static MethodInfo GetMethodInfo(Expression<Action> expression)
		{
			return GetMethodInfo((LambdaExpression) expression);
		}

		/// <summary>
		/// Given a lambda expression that calls a method, returns the method info.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression">The expression.</param>
		public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
		{
			return GetMethodInfo((LambdaExpression) expression);
		}

		/// <summary>
		/// Given a lambda expression that calls a method, returns the method info.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="expression">The expression.</param>
		public static MethodInfo GetMethodInfo<T, TResult>(Expression<Func<T, TResult>> expression)
		{
			return GetMethodInfo((LambdaExpression) expression);
		}

		/// <summary>
		/// Given a lambda expression that calls a method, returns the method info.
		/// </summary>
		/// <param name="expression">The expression.</param>
		public static MethodInfo GetMethodInfo(LambdaExpression expression)
		{
			var outermostExpression = expression.Body as MethodCallExpression;

			if (outermostExpression == null)
			{
				throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
			}

			return outermostExpression.Method;
		}
	}
}