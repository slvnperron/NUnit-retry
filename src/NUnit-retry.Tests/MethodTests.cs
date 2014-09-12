// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

using System;

namespace NUnit_retry.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class MethodTests
    {
        private int _i = 0;
        private int _run = 0;

        [SetUp]
        public void SetUp()
        {
        }

        [Test, Retry(3,1)]
        public void one_out_of_three()
        {
            _i++;
            Console.WriteLine("{0}",_i);
            Console.WriteLine("{0}",_run);

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

        [Test, Retry(4, 2)]
        public void Two_out_of_four()
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
            Assert.Pass();
        }

        [TestCase(), Retry(5, 2)]
        public void two_Out_of_five()
        {
            _i++;
            Console.WriteLine("{0}", _i);
            Console.WriteLine("{0}", _run);

            if (_run == 0 || _run == 1 || _run == 2)
            {
                _run++;
                Console.WriteLine("Failed");
                Assert.Fail();
            }
            Console.WriteLine("Passed");
            _run++;

            Assert.Pass();
        }

        [TestCase(), Retry(4, 2)]
        public void two_Out_of_four1()
        {
            _i++;
            Console.WriteLine("{0}", _i);
            Console.WriteLine("{0}", _run);

            if (_run == 1 || _run == 3)
            {
                _run++;
                Console.WriteLine("Failed");
                Assert.Fail();
            }
            Console.WriteLine("Passed");
            _run++;

            Assert.Pass();
        }

        [TestCase(), Retry(3, 3)]
        public void allPass()
        {
            _i++;
            Console.WriteLine("{0}", _i);
            Console.WriteLine("{0}", _run);

            if (_run == 10 || _run == 11)
            {
                _run++;
                Console.WriteLine("Failed");
                Assert.Fail();
            }
            Console.WriteLine("Passed");
            _run++;

            Assert.Pass();
        }

        [TestCase(0), Retry]
        [TestCase(1), Retry]
        [TestCase(3), Retry]
        public void PassAtSomePoint(int runTimes)
        {
            _i++;
            Console.WriteLine("{0}", _i);
            Console.WriteLine("{0}", _run);

            if (_run == runTimes)
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
