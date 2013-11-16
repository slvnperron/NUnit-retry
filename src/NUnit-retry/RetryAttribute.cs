// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace NUnit_retry
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RetryAttribute : Attribute
    {
        public RetryAttribute(int times = 5)
        {
            this.Times = times;
        }

        public int Times { get; set; }
    }
}
