// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

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
            var testCaseBuilders = host.GetExtensionPoint("TestDecorators");
            testCaseBuilders.Install(this);
            return true;
        }

        public Test Decorate(Test test, MemberInfo member)
        {
            var attrs = member.GetCustomAttributes(typeof(RetryAttribute), true);

            if (test is NUnitTestMethod)
            {
                var testMethod = (NUnitTestMethod)test;
                
                if (testMethod.FixtureType != null)
                {
                    var fixtureAttrs =
                        testMethod.FixtureType.GetCustomAttributes(typeof(RetryAttribute), true).ToArray();

                    if (fixtureAttrs.Length > 0)
                    {
                        var retryAttr = (fixtureAttrs[0] as RetryAttribute);

                        if (retryAttr != null)
                        {
                            test = new RetriedTestMethod(
                                testMethod.Method,
                                retryAttr.Times,
                                retryAttr.RequiredPassCount);
                        }
                    }
                }

                if (attrs.Any())
                {
                    var retryAttr = (attrs.First() as RetryAttribute);

                    if (retryAttr == null)
                    {
                        return test;
                    }

                    test = new RetriedTestMethod(testMethod.Method, retryAttr.Times, retryAttr.RequiredPassCount);
                }

                NUnitFramework.ApplyCommonAttributes(member, test);
            }

            if (test is ParameterizedMethodSuite)
            {
                var suite = test as ParameterizedMethodSuite;
                RetriedParameterizedTestSuiteMethod outputSuite;

                if (attrs.Any())
                {
                    var retryAttr = (attrs.First() as RetryAttribute);

                    if (retryAttr == null) return test;

                    outputSuite = new RetriedParameterizedTestSuiteMethod(member as MethodInfo, retryAttr.Times, retryAttr.RequiredPassCount);
                }
                else
                {
                    return test;
                }

                NUnitFramework.ApplyCommonAttributes(member, outputSuite);

                outputSuite.RunState = suite.RunState;
                foreach (NUnitTestMethod testMethod in suite.Tests)
                {
                    outputSuite.Add(testMethod);
                }
                return outputSuite;
            }

            return test;
        }
    }
}
