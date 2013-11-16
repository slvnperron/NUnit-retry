// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace NUnit_retry
{
    using System.Reflection;

    using NUnit.Core;

    public class RetriedTestMethod : NUnitTestMethod
    {
        private readonly int tryCount;

        public RetriedTestMethod(MethodInfo method, int tryCount)
            : base(method)
        {
            this.tryCount = tryCount;
        }

        public override TestResult Run(EventListener listener, ITestFilter filter)
        {
            var failCount = 0;
            TestResult successResult = null;

            for (var i = 0; i < this.tryCount; i++)
            {
                var result = base.Run(listener, filter);

                if (TestFailed(result))
                {
                    if (++failCount > 1)
                    {
                        return result;
                    }
                }
                else
                {
                    successResult = result;

                    if (i == 0)
                    {
                        return successResult;
                    }
                }
            }

            return successResult;
        }

        private static bool TestFailed(TestResult result)
        {
            return result.ResultState == ResultState.Error || result.ResultState == ResultState.Failure;
        }
    }
}
