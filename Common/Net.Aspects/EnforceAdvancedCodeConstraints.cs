using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace Net.Aspects
{
	[MulticastAttributeUsage(
		MulticastTargets.Method | MulticastTargets.InstanceConstructor,
		TargetMemberAttributes = MulticastAttributes.Public | MulticastAttributes.Instance | MulticastAttributes.Static)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = true)]
	public class EnforceAdvancedCodeConstraints : MethodLevelAspect, IAspectProvider
	{
		public IEnumerable<AspectInstance> ProvideAspects(object targetElement)
		{
			var methodBase = (MethodBase)targetElement;

			if (methodBase.ReflectedType.IsInterface 
				|| IsPrivateOrProtectedMethod(methodBase)
				|| NullIsOkAttributeFound(methodBase))
			{
				yield break;
			}

			var methodParameters = methodBase.GetParameters();

			if (methodParameters.Any(ShouldCheckForNull) || ShouldCheckForNullReturnValue(methodBase))
			{
				yield return new AspectInstance(
					targetElement,
					new CheckNotNullArgumentsAspect(
						SelectParameterDescriptors(methodParameters.Where(parameter => parameter.IsIn), ShouldCheckForNull),
						SelectParameterDescriptors(methodParameters.Where(parameter => parameter.IsOut), ShouldCheckForNull),
						ShouldCheckForNullReturnValue(methodBase)));
			}
		}

		private static bool NullIsOkAttributeFound(MethodBase methodBase)
		{
			return methodBase.ReflectedType.IsDefined(typeof(NullIsOkAttribute), true) || methodBase.IsDefined(typeof(NullIsOkAttribute), true);
		}

		private static bool IsPrivateOrProtectedMethod(MethodBase methodBase)
		{
			return methodBase.IsPrivate || methodBase.IsFamily || methodBase.IsFamilyOrAssembly;
		}

		private static ParameterDescriptor[] SelectParameterDescriptors(
			IEnumerable<ParameterInfo> parameters,
			Func<ParameterInfo, bool> selectParameters)
		{
			return parameters
				.Select((parameterInfo, index) => new { parameterInfo, index })
				.Where(pair => selectParameters(pair.parameterInfo))
				.Select(pair => new ParameterDescriptor { Index = pair.index, Name = pair.parameterInfo.Name })
				.ToArray();
		}

		private static bool ShouldCheckForNullReturnValue(MethodBase methodBase)
		{
			var methodInfo = methodBase as MethodInfo;
			return methodInfo != null && ShouldCheckForNull(methodInfo.ReturnParameter);
		}

		private static bool ShouldCheckForNull(ParameterInfo parameterInfo)
		{
			if (parameterInfo.ParameterType.IsValueType || IsNullable(parameterInfo.ParameterType))
			{
				return false;
			}

			return !parameterInfo.IsDefined(typeof(NullIsOkAttribute), false);
		}

		private static bool IsNullable(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}
	}
}
