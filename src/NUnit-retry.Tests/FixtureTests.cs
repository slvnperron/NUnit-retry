// /////////////////////////////////////////////////////////////////////
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// /////////////////////////////////////////////////////////////////////

using System;
using NUnit.Framework;

namespace NUnit_retry.Tests
{
    [TestFixture]
    [Retry]// 3 run 1 requiredPass
    public class FixtureTests
    {
        private int _i;
        private int _run;

        [Test]
        public void ShouldSucceed_One_Time_Out_Of_3()
        {
            _i++;
            Console.WriteLine("{0}", _i);
            Console.WriteLine("{0}", _run);

            if (_run == 0 || _run == 1)
            {
                _run++;
                Console.WriteLine("Failed");
                Assert.Fail();
            }
            Console.WriteLine("Passed");
            _run++;

            Assert.Pass();
        }
    }

    public class InheritedAttribute : FixtureTests
    {
        private int _i;
        private int _run;

        [Test]
        public void Inherited_ShouldSucceed_One_Time_Out_Of_3()
        {
            _i++;
            Console.WriteLine("{0}", _i);
            Console.WriteLine("{0}", _run);

            if (_run == 0 || _run == 1)
            {
                _run++;
                Console.WriteLine("Failed");
                Assert.Fail();
            }
            Console.WriteLine("Passed");
            _run++;

            Assert.Pass();
        }
    }
}