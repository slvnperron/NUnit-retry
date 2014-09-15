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
        private readonly int _requiredPassCount;

        private readonly int _tryCount;

        public RetriedTestMethod(MethodInfo method, int tryCount, int requiredPassCount)
            : base(method)
        {
            _tryCount = tryCount;
            _requiredPassCount = requiredPassCount;
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
                        
                        return result;
                    }
                }
                else
                {
                    //Log Fatal retried x times
                    failureResult = result;
                }
            }
            return failureResult;
        }

        private static bool TestFailed(TestResult result)
        {
            return result.ResultState == ResultState.Error ||
                   result.ResultState == ResultState.Failure ||
                   result.ResultState == ResultState.Inconclusive;
        }
    }
}
