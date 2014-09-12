// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace NUnit_retry
{
    using System;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RetryAttribute : Attribute
    {
        public RetryAttribute(int times = 5, int requiredPassCount = 1)
        {
            if (requiredPassCount > times)
            {
                throw new Exception("Required Pass Count must be lower or equal than the number of retries.");
            }

            Times = times;
            RequiredPassCount = requiredPassCount;
        }

        public int RequiredPassCount { get; set; }
        public int Times { get; set; }
    }
}
