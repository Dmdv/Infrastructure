using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Net.Common.Contracts;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace Net.Aspects
{
	[ProvideAspectRole(StandardRoles.Validation)]
	[AspectRoleDependency(AspectDependencyAction.Commute, StandardRoles.Validation)]
	[Serializable]
	public sealed class CheckNotNullArgumentsAspect : OnMethodBoundaryAspect
	{
		public CheckNotNullArgumentsAspect(ParameterDescriptor[] inputParameters, ParameterDescriptor[] outParameters, bool checkResult)
		{
			_inputParameters = inputParameters;
			_outParameters = outParameters;
			_checkResult = checkResult;
		}

		[DebuggerStepThrough]
		public override void OnEntry(MethodExecutionArgs args)
		{
			ThrowIfAnyParameterIsNull(_inputParameters, args, parameterName => new ArgumentNullException(parameterName));
		}

		[DebuggerStepThrough]
		public override void OnSuccess(MethodExecutionArgs args)
		{
			ThrowIfAnyParameterIsNull(
				_outParameters,
				args,
				parameterName => new ContractViolationException("out parameter " + parameterName + " can't be null"));

			if (_checkResult && args.ReturnValue == null)
			{
				throw new ContractViolationException("return value must not be null for " + args.Method.ReflectedType + ":" + args.Method);
			}
		}

		[DebuggerStepThrough]
		private static void ThrowIfAnyParameterIsNull(
			IEnumerable<ParameterDescriptor> parameters,
			MethodExecutionArgs args,
			Func<string, Exception> createException)
		{
			var firstNullArgumentDescriptor = parameters.FirstOrDefault(descriptor => args.Arguments[descriptor.Index] == null);
			if (firstNullArgumentDescriptor != null)
			{
				throw createException(firstNullArgumentDescriptor.Name);
			}
		}

		private readonly ParameterDescriptor[] _inputParameters;
		private readonly ParameterDescriptor[] _outParameters;
		private readonly bool _checkResult;
	}
}
