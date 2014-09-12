// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

using System;
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
            var testCaseBuilders = host.GetExtensionPoint("TestDecorators");
            testCaseBuilders.Install(this);
            return true;
        }

        public Test Decorate(Test test, MemberInfo member)
        {
            if (test is NUnitTestMethod)
            {
                var testMethod = (NUnitTestMethod)test;

                var attrs = member.GetCustomAttributes(typeof(RetryAttribute), true);

                if (testMethod.FixtureType != null)
                {
                    var fixtureAttrs =
                        testMethod.FixtureType.GetCustomAttributes(typeof(RetryAttribute), true).ToArray();

                    if (fixtureAttrs.Length > 0)
                    {
                        var retryAttr = (fixtureAttrs[0] as RetryAttribute);

                        if (retryAttr != null)
                        {
                            test = new TestMethodExtension(
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

                    test = new TestMethodExtension(testMethod.Method, retryAttr.Times, retryAttr.RequiredPassCount);
                }
            }

            else
            {
                if (test is ParameterizedMethodSuite)
                {
                    var suite = test as ParameterizedMethodSuite;

                    var outputSuite = new ParameterizedMethodSuiteExtension(member as MethodInfo, 6, 3);
                    NUnitFramework.ApplyCommonAttributes(member, outputSuite);

                    outputSuite.RunState = suite.RunState;
                    
                    foreach (NUnitTestMethod testMethod in suite.Tests)
                    {
                        outputSuite.Add(testMethod);
                    }
                    return outputSuite;
                }
            }
            return test;
        }

        public class ParameterizedMethodSuiteExtension : ParameterizedMethodSuite
        {
            private readonly int _tryCount;
            private readonly int _requiredPassCount;
            public ParameterizedMethodSuiteExtension(MethodInfo method, int run, int requiredPass)
                : base(method)
            {
                _tryCount = run;
                _requiredPassCount = requiredPass;
            }

            public override TestResult Run(EventListener listener, ITestFilter filter)
            {
                var successCount = 0;
                TestResult failureResult = null;

                for (var i = 0; i < _tryCount; i++)
                {
                    var result = base.Run(listener, filter);

                    if (!TestFailed(result))
                    {
                        if (++successCount >= _requiredPassCount)
                        {
                            //result.SetResult(result.ResultState, "", result.StackTrace, result.FailureSite);
                            return result;
                        }
                    }
                    else
                    {
                        //result.SetResult(result.ResultState, "", result.StackTrace, result.FailureSite);
                        failureResult = result;
                    }
                }

                return failureResult;
            }
            private static bool TestFailed(TestResult result)
            {
                return result.ResultState == ResultState.Error || result.ResultState == ResultState.Failure;
            }
        }

        public class TestMethodExtension : RetriedTestMethod
        {
            //private readonly List<Attribute> _tags;
            public TestMethodExtension(MethodInfo methodInfo, int run, int requiredPass)
                : base(methodInfo,run,requiredPass)
            {
                //_tags = tags;
            }

            //public override TestResult Run(EventListener listener, ITestFilter filter)
            //{
            //    TestResult result = base.Run(listener, filter);
            //    //
            //    return result;
            //}
        }
    }
}
