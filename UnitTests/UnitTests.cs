using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Common.Extensions;

namespace UnitTests
{
	[TestClass]
	public class UnitTests
	{
		[TestMethod]
		public void GetMethodInfo_should_return_method_info()
		{
			var methodInfo = ReflectionExtensions.GetMethodInfo<SampleClass>(c => c.AMethod());
			methodInfo.Name.Should().Be("AMethod");
		}

		[TestMethod]
		public void GetMethodInfo_should_return_method_info_for_generic_method()
		{
			var methodInfo = ReflectionExtensions.GetMethodInfo<SampleClass>(c => c.AGenericMethod(default(int)));

			methodInfo.Name.Should().Be("AGenericMethod");
			methodInfo.GetParameters().First().ParameterType.Should().Be<int>();
		}

		[TestMethod]
		public void GetMethodInfo_should_return_method_info_for_static_method_on_static_class()
		{
			var methodInfo = ReflectionExtensions.GetMethodInfo(() => StaticTestClass.StaticTestMethod());

			methodInfo.Name.Should().Be("StaticTestMethod");
			methodInfo.IsStatic.Should().BeTrue();
		}
	}

	internal static class StaticTestClass
	{
		public static void StaticTestMethod()
		{
		}
	}

	internal class SampleClass
	{
		public void AGenericMethod<T>(T i)
		{
		}

		public void AMethod()
		{
		}
	}
}