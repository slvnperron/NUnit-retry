// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace NUnit_retry.Tests
{
    using System;
    using NUnit.Framework;

    public class BaseTestSuite
    {
        public object test = null;

        public virtual void Init() {
            test = new object();
        }

        public virtual void UnInit() {
            test = null;
        }
    }
    [TestFixture]
    public class SetupTeardownTests : BaseTestSuite
    {
        [SetUp]
        public override void Init() {
            base.Init();
        }

        [TearDown]
        public override void UnInit() {
            base.UnInit();
        }
            
        [Test]
        [Retry(Times = 3, RequiredPassCount = 2)]
        public void FixtureSetupTeardown()
        {
            Assert.That(base.test != null, "Did not call [Setup]");
        }
    }
}

