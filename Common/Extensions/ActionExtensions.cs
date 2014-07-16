using System;

namespace Common.Extensions
{
	public static class ActionExtensions
	{
		public static void Execute(this Action action)
		{
			action();
		}

		public static void Execute(this Func<Action> action)
		{
			action().Execute();
		}

		public static void Execute(this Action<Action> action, Action callee)
		{
			action(callee);
		}
	}
}