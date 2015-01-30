using System;
using System.Diagnostics;

// ReSharper disable CodeCleanup
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace Net.Common.Extensions
{
    public static class TraceHelper
    {
        public static void TraceInformationWithStamp(string info)
        {
            Trace.TraceInformation(GetTimeStamp(info));
        }

        public static void TraceInformationWithStamp(string info, params object[] args)
        {
            Trace.TraceInformation(GetTimeStamp(info), args);
        }

        public static void TraceErrorWithStamp(string error)
        {
            Trace.TraceError(GetTimeStamp(error));
        }

        public static void TraceErrorWithStamp(string error, params object[] args)
        {
            Trace.TraceError(GetTimeStamp(error), args);
        }

        private static string GetTimeStamp(string message)
        {
            return "{0}: {1}".FormatString(DateTime.UtcNow.ToString("u"), message);
        }
    }
}