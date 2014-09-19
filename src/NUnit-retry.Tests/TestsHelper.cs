// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Diagnostics;

namespace NUnit_retry.Tests
{
    public static class TestsHelper
    {
        private static Dictionary<string, int> ExecutionsByMethodName { get; set; }

        public static string GetCurrentMethodName()
        {
            return GetCurrentMethodNameInternal();
        }

        public static int GetCurrentMethodExecutionTimes()
        {
            var method = GetCurrentMethodNameInternal();

            if (!ExecutionsByMethodName.ContainsKey(method))
            {
                ExecutionsByMethodName.Add(method, 0);
            }

            return ExecutionsByMethodName[method];
        }

        public static void IncrementCurrentMethodExecutionTimes()
        {
            var method = GetCurrentMethodNameInternal();

            if (!ExecutionsByMethodName.ContainsKey(method))
            {
                ExecutionsByMethodName.Add(method, 0);
            }

            ExecutionsByMethodName[method] += 1;
        }

        private static string GetCurrentMethodNameInternal()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(2);

            var currentMethodName = sf.GetMethod();

            return currentMethodName.Name;
        }
    }
}