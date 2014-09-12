//// /////////////////////////////////////////////////////////////////////
////  This is free software licensed under the NUnit license. You
////  may obtain a copy of the license as well as information regarding
////  copyright ownership at http://nunit.org.    
//// /////////////////////////////////////////////////////////////////////

//using System;

//namespace NUnit_retry.Tests
//{
//    using NUnit.Framework;

//    [TestFixture]
//    //[Retry]
//    public class FixtureTests
//    {
//        [TestCase]
//        public void ShouldSucceed_One_Time_Out_Of_3()
//        {
//            InterTestContext.IncrementMethodTries("class_1_on_3");

//            Console.WriteLine(InterTestContext.InterTestCounts["class_1_on_3"]);

//            if (InterTestContext.InterTestCounts["class_1_on_3"] == 1 || InterTestContext.InterTestCounts["class_1_on_3"] == 2)
//            {
//                Assert.Fail();
//            }

//            Assert.Pass();
//        }
//    }

//    public class InheritedAttribute : FixtureTests
//    {
//        [Test]
//        public void Inherited_ShouldSucceed_One_Time_Out_Of_3()
//        {
//            InterTestContext.IncrementMethodTries("inherited_1_on_3");

//            if (InterTestContext.InterTestCounts["inherited_1_on_3"] == 1)
//            {
//                Assert.Fail();
//            }

//            Assert.Pass();
//        }
//    }
//}
