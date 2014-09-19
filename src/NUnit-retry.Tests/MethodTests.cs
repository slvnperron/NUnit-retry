// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace NUnit_retry.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class MethodTests
    {
        [Test]
        [Retry(Times = 3, RequiredPassCount = 2)]
        public void One_Failure_On_Three_Should_Pass()
        {
            InterTestContext.IncrementMethodTries("1_on_3");

            if (InterTestContext.InterTestCounts["1_on_3"] == 1)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void UnAnnotatedTest()
        {
            Assert.IsTrue(true);
        }

        [Test]
        [Retry]
        public void When_SucceedOnce_Pass()
        {
            InterTestContext.IncrementMethodTries("once");

            if (InterTestContext.InterTestCounts["once"] == 2)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [TestCase(TestName = "TestCaseName")]
        [Retry]
        public void ShouldSucceed_One_Time_Out_Of_3_TestCase()
        {
            InterTestContext.IncrementMethodTries("1_on_3_TestCase");

            if (InterTestContext.InterTestCounts["1_on_3_TestCase"] == 1)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        public TestCaseData[] CaseSource
        {
            get { return new TestCaseData[] { new TestCaseData().SetName("TestCaseSourceName"), }; }
        }

        [TestCaseSource("CaseSource")]
        [Retry]
        public void ShouldSucceed_One_Time_Out_Of_3_TestCaseSource()
        {
            InterTestContext.IncrementMethodTries("1_on_3_TestCaseSource");

            if (InterTestContext.InterTestCounts["1_on_3_TestCaseSource"] == 1)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }
    }
}
