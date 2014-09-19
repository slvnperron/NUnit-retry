// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace NUnit_retry.Tests
{
    using NUnit.Framework;

    [SetUpFixture]
    public class SetUpTests
    {
        [SetUp]
        public void ResetStaticCounters()
        {
            InterTestContext.ResetInterTestCounts();
        }
    }
}
