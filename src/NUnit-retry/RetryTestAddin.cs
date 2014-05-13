// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

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
                    var testMethod = (NUnitTestMethod) test;

                    var attrs = member.GetCustomAttributes(typeof (RetryAttribute), true);
                    var properties = test.Properties;
                    RetryAttribute retryAttr = null;

                    if (attrs.Any())
                    {
                        retryAttr = (attrs.First() as RetryAttribute);
                    }

                    if (retryAttr == null && testMethod.FixtureType != null)
                    {
                        var fixtureAttrs =
                            testMethod.FixtureType.GetCustomAttributes(typeof (RetryAttribute), true).ToArray();

                        if (fixtureAttrs.Length > 0)
                        {
                            retryAttr = (fixtureAttrs[0] as RetryAttribute);
                        }
                    }

                    if (retryAttr != null)
                    {
                        test = new RetriedTestMethod(
                            testMethod.Method,
                            retryAttr.Times,
                            retryAttr.RequiredPassCount);
                        test.Properties = properties;
                    }
                }
            }

            return test;
        }
    }
}
