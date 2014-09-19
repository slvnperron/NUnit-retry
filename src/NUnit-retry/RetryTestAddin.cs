// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace NUnit_retry
{
    using System.Linq;
    using System.Reflection;

    using NUnit.Core;
    using NUnit.Core.Extensibility;

    [NUnitAddin(Type = ExtensionType.Core, Description = "Retries an intermittently failing test.")]
    public class RetryTestAddin : IAddin, ITestDecorator
    {
        public bool Install(IExtensionHost host)
        {
            var testDecorators = host.GetExtensionPoint("TestDecorators");
            testDecorators.Install(this);
            return true;
        }

        public Test Decorate(Test test, MemberInfo member)
        {
            var retryAttr = GetRetryAttribute(member, test);

            if (retryAttr == null) return test;

            if (test is NUnitTestMethod)
            {
                return new RetriedTestMethod(
                    (NUnitTestMethod)test,
                    retryAttr.Times,
                    retryAttr.RequiredPassCount);
            }

            if (!(test is ParameterizedMethodSuite)) return test;

            var testMethodSuite = (ParameterizedMethodSuite)test;

            System.Collections.IList newTests = new List<Test>();
            foreach (Test childTest in testMethodSuite.Tests)
            {
                if (childTest is NUnitTestMethod)
                {
                    var oldTest = (NUnitTestMethod)childTest;
                    var newTest = new RetriedTestMethod(
                        oldTest,
                        retryAttr.Times,
                        retryAttr.RequiredPassCount);

                    newTests.Add(newTest);
                }
                else
                {
                    newTests.Add(childTest);
                }
            }

            testMethodSuite.Tests.Clear();
            foreach (Test newTest in newTests)
            {
                testMethodSuite.Add(newTest);
            }

            return testMethodSuite;
        }

        private static RetryAttribute GetRetryAttribute(MemberInfo member, Test testMethodSuite)
        {
            var retryAttr = GetRetryAttribute(member);

            if (retryAttr == null && testMethodSuite.FixtureType != null)
            {
                retryAttr = GetRetryAttribute(testMethodSuite.FixtureType);
            }

            return retryAttr;
        }

        private static RetryAttribute GetRetryAttribute(MemberInfo member)
        {
            var attrs = member.GetCustomAttributes(typeof(RetryAttribute), true)
                .Cast<RetryAttribute>();

            return attrs.FirstOrDefault();
        }
    }
}
