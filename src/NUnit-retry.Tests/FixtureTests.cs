// /////////////////////////////////////////////////////////////////////
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// /////////////////////////////////////////////////////////////////////

using NUnit.Framework;

namespace NUnit_retry.Tests
{
    [TestFixture]
    [Retry]
    public class FixtureTests
    {
        [Test]
        public void ShouldSucceed_One_Time_Out_Of_3()
        {
            var run = TestsHelper.GetCurrentMethodExecutionTimes();

            if (run == 0 || run == 1)
            {
                TestsHelper.IncrementCurrentMethodExecutionTimes();
                Assert.Fail();
            }

            Assert.Pass();
        }
    }

    public class InheritedAttribute : FixtureTests
    {
        [Test]
        public void Inherited_ShouldSucceed_One_Time_Out_Of_3()
        {
            var run = TestsHelper.GetCurrentMethodExecutionTimes();

            if (run == 0 || run == 1)
            {
                TestsHelper.IncrementCurrentMethodExecutionTimes();
                Assert.Fail();
            }

            Assert.Pass();
        }
    }
}
