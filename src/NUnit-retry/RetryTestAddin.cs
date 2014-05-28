// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using NUnit.Framework;

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
            var explicitAttrs = member.GetCustomAttributes(typeof(ExplicitAttribute), true);
            var ignoreAttrs = member.GetCustomAttributes(typeof(IgnoreAttribute), true).ToArray();

            if (!explicitAttrs.Any() && !ignoreAttrs.Any())
            {
                RetryAttribute retryAttr = GetRetryAttribute(member, test);

                if (retryAttr != null)
                {
                    if (test is NUnitTestMethod)
                    {
                        return new RetriedTestMethod(
                            (NUnitTestMethod)test,
                            retryAttr.Times,
                            retryAttr.RequiredPassCount);
                    }
                    
                    if (test is ParameterizedMethodSuite)
                    {
                        var testMethodSuite = (ParameterizedMethodSuite)test;

                        System.Collections.IList newTests = new List<Test>();
                        foreach (Test childTest in testMethodSuite.Tests)
                        {
                            if (childTest is NUnitTestMethod)
                            {
                                NUnitTestMethod oldTest = (NUnitTestMethod)childTest;
                                RetriedTestMethod newTest = new RetriedTestMethod(
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
                }
            }
            return test;
        }

        private static RetryAttribute GetRetryAttribute(MemberInfo member, Test testMethodSuite)
        {
            RetryAttribute retryAttr = GetRetryAttribute(member);

            if (retryAttr == null && testMethodSuite.FixtureType != null)
            {
                retryAttr = GetRetryAttribute(testMethodSuite.FixtureType);
            }
            return retryAttr;
        }

        private static RetryAttribute GetRetryAttribute(MemberInfo member)
        {
            IEnumerable<RetryAttribute> attrs = member.GetCustomAttributes(typeof(RetryAttribute), true)
                .Cast<RetryAttribute>();
            return attrs.FirstOrDefault();
        }
    }
}
