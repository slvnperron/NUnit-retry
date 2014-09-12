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

        [Test, Category("sgdfg")]
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

        [Test, Category("adsf")]
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

        [TestCase()]
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

        [TestCase()]
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

        [TestCase()]
        public void allPass()
        {
            _i++;
            Console.WriteLine("{0}", _i);
            Console.WriteLine("{0}", _run);

            if (_run == 2 || _run == 3)
            {
                _run++;
                Console.WriteLine("Failed");
                Assert.Fail();
            }
            Console.WriteLine("Passed");
            _run++;

            Assert.Pass();
        }

        [TestCase(0, "abc"),  Category("a")]
        [TestCase(1, "123"),  Category("a")]
        [TestCase(3, "#$%"),  Category("a")]
        public void PassAtSomePoint(int runTimes, string msg)
        {
            _i++;
            Console.WriteLine("{0}", _i);
            Console.WriteLine("{0}", _run);

            if (_run == runTimes)
            {
                _run++;
                Console.WriteLine("Failed {0}", msg);
                Assert.Fail();
            }
            Console.WriteLine("Passed {0}", msg);
            _run++;

            Assert.Pass();
        }

        [TestCase("abc")]
        public void Fail(string output)
        {
            _i++;
            Console.WriteLine("{0}", _i);
            Console.WriteLine("{0}", _run);

            _run++;
            Console.WriteLine("Failed {0}", output);
            Assert.Fail();
        }
    }
}
