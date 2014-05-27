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
            if (test is NUnitTestMethod)
            {
                var explicitAttrs = member.GetCustomAttributes(typeof(ExplicitAttribute), true).ToArray();
                var ignoreAttrs = member.GetCustomAttributes(typeof(IgnoreAttribute), true).ToArray();

                if (explicitAttrs.Length < 1 && ignoreAttrs.Length < 1)
                {
                    var testMethod = (NUnitTestMethod)test;

                    var attrs = member.GetCustomAttributes(typeof(RetryAttribute), true);
                    RetryAttribute retryAttr = null;

                    if (attrs.Any())
                    {
                        retryAttr = (attrs.First() as RetryAttribute);
                    }

                    if (retryAttr == null && testMethod.FixtureType != null)
                    {
                        var fixtureAttrs =
                            testMethod.FixtureType.GetCustomAttributes(typeof(RetryAttribute), true).ToArray();

                        if (fixtureAttrs.Length > 0)
                        {
                            retryAttr = (fixtureAttrs[0] as RetryAttribute);
                        }
                    }

                    if (retryAttr != null)
                    {
                        test = new RetriedTestMethod(
                            testMethod,
                            retryAttr.Times,
                            retryAttr.RequiredPassCount);
                    }
                }
            }
            else if (test is ParameterizedMethodSuite)
            {
                var testMethodSuite = (ParameterizedMethodSuite)test;
                var attrs = member.GetCustomAttributes(typeof(RetryAttribute), true);
                RetryAttribute retryAttr = null;

                if (attrs.Any())
                {
                    retryAttr = (attrs.First() as RetryAttribute);
                }

                if (retryAttr == null && testMethodSuite.FixtureType != null)
                {
                    var fixtureAttrs =
                        testMethodSuite.FixtureType.GetCustomAttributes(typeof(RetryAttribute), true).ToArray();

                    if (fixtureAttrs.Length > 0)
                    {
                        retryAttr = (fixtureAttrs[0] as RetryAttribute);
                    }
                }

                if (retryAttr != null)
                {
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

            return test;
        }
    }
}
