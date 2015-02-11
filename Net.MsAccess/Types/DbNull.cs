using System;
using Common.Annotations;

namespace Net.MsAccess.Types
{
	[PublicAPI]
	[Flags]
	public enum DbNull
	{
		[PublicAPI]
		Null,

		[PublicAPI]
		NotNull
	}
}