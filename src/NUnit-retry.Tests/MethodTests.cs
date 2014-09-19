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
        [SetUp]
        public void SetUp()
        {
        }

        [Test, Category("A"), Retry(10,5)]
        public void Five_out_of_ten()
        {
            TestsHelper.IncrementCurrentMethodExecutionTimes();
            var run = TestsHelper.GetCurrentMethodExecutionTimes();

            if (run % 2 == 0)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test, Category("B"), Retry(6, 3)]
        public void Three_out_of_six()
        {
            TestsHelper.IncrementCurrentMethodExecutionTimes();
            var run = TestsHelper.GetCurrentMethodExecutionTimes();

            if (run % 2 == 0)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [TestCase, Retry(5,2)]
        public void two_Out_of_five()
        {
            TestsHelper.IncrementCurrentMethodExecutionTimes();
            var run = TestsHelper.GetCurrentMethodExecutionTimes();

            if (run <= 3)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [TestCase, Category("C"), Retry(3,2)]
        public void two_out_of_Three()
        {
            TestsHelper.IncrementCurrentMethodExecutionTimes();
            var run = TestsHelper.GetCurrentMethodExecutionTimes();

            if (run <= 1)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [TestCase, Category("D"), Retry]
        public void Default_one_out_of_Three()
        {
            TestsHelper.IncrementCurrentMethodExecutionTimes();
            var run = TestsHelper.GetCurrentMethodExecutionTimes();

            if (run <= 2)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [TestCase(0, "abc"), Category("E")]
        [TestCase(1, "123"), Category("F")]
        [TestCase(3, "#$%"), Category("G"), Retry(11, 10)]
        public void PassAtSomePoint(int runTimes, string msg)
        {
            TestsHelper.IncrementCurrentMethodExecutionTimes();
            var run = TestsHelper.GetCurrentMethodExecutionTimes();

            if (run <= 1)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }
    }
}
