// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace NUnit_retry.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class HelloWorld
    {
        private static readonly Dictionary<string, int> InterTestCounts = new Dictionary<string, int>();

        [Test]
        public void UnAnnotatedTest()
        {
            Assert.IsTrue(true);
        }

        [Test]
        [Retry]
        public void When_SucceedOnce_Pass()
        {
            IncrementMethodTries("once");

            if (InterTestCounts["once"] == 2)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        [Retry(3)]
        public void One_Failure_On_Three_Should_Pass()
        {
            IncrementMethodTries("1_on_3");

            if (InterTestCounts["1_on_3"] == 1)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        private static void IncrementMethodTries(string methodKey)
        {
            if (!InterTestCounts.ContainsKey(methodKey))
            {
                InterTestCounts.Add(methodKey, 0);
            }

            InterTestCounts[methodKey] = InterTestCounts[methodKey] + 1;
        }
    }
}
