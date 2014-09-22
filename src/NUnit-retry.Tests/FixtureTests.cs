// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace NUnit_retry.Tests
{
    using NUnit.Framework;

    [TestFixture]
    [Retry]
    public class FixtureTests
    {
        [Test]
        public void ShouldSucceed_One_Time_Out_Of_3()
        {
            InterTestContext.IncrementMethodTries("class_1_on_3");

            if (InterTestContext.InterTestCounts["class_1_on_3"] == 1)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Retry]
        [TestCase(TestName = "TestCaseName")]
        // TODO TestCases don't work at class-level
        public void ShouldSucceed_One_Time_Out_Of_3_TestCase()
        {
            InterTestContext.IncrementMethodTries("class_1_on_3_TestCase");

            if (InterTestContext.InterTestCounts["class_1_on_3_TestCase"] == 1)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        public TestCaseData[] CaseSource
        {
            get { return new[] { new TestCaseData().SetName("TestCaseSourceName"), }; }
        }

        [Retry]
        [TestCase(TestName = "TestCaseName")]
        // TODO TestCaseSources don't work at class level
        [TestCaseSource("CaseSource")]
        public void ShouldSucceed_One_Time_Out_Of_3_TestCaseSource()
        {
            InterTestContext.IncrementMethodTries("class_1_on_3_TestCaseSource");

            if (InterTestContext.InterTestCounts["class_1_on_3_TestCaseSource"] == 1)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }
    }

    [TestFixture]
    [Retry]
    public class BaseFixture
    {
        
    }

    public class InheritedAttribute : BaseFixture
    {
        [Test]
        public void Inherited_ShouldSucceed_One_Time_Out_Of_3()
        {
            InterTestContext.IncrementMethodTries("inherited_1_on_3");

            if (InterTestContext.InterTestCounts["inherited_1_on_3"] == 1)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }
    }
}
