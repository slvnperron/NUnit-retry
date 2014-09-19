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
        private int _i;
        private int _run;

        [SetUp]
        public void SetUp()
        {
        }

        [Test, Category("A"), Retry(10,5)]
        public void Five_out_of_ten()
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

        [Test, Category("B"), Retry(6, 3)]
        public void Three_out_of_six()
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

        [TestCase(), Retry(5,2)]
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

        [TestCase(), Category("C"), Retry(3,2)]
        public void two_out_of_Three()
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

        [TestCase(), Category("D"), Retry()]
        public void Default_one_out_of_Three()
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

        [TestCase(0, "abc"), Category("E")]
        [TestCase(1, "123"), Category("F")]
        [TestCase(3, "#$%"), Category("G"), Retry(11, 10)]
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
    }
}
