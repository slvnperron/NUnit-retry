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
            if (test is NUnitTestMethod)
            {
                var testMethod = (NUnitTestMethod)test;
                var attrs = member.GetCustomAttributes(typeof(RetryAttribute), true);

                if (attrs.Any())
                {
                    var retryAttr = (attrs.First() as RetryAttribute);
                    if (retryAttr == null)
                    {
                        return test;
                    }

                    test = new RetriedTestMethod(testMethod.Method, retryAttr.Times);
                }
            }

            return test;
        }
    }
}
