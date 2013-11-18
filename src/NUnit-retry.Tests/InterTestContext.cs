// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace NUnit_retry.Tests
{
    using System.Collections.Generic;

    public static class InterTestContext
    {
        public static readonly Dictionary<string, int> InterTestCounts = new Dictionary<string, int>();

        public static void IncrementMethodTries(string methodKey)
        {
            if (!InterTestCounts.ContainsKey(methodKey))
            {
                InterTestCounts.Add(methodKey, 0);
            }

            InterTestCounts[methodKey] = InterTestCounts[methodKey] + 1;
        }
    }
}
