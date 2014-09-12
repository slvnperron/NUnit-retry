using System.Reflection;
using NUnit.Core;

namespace NUnit_retry
{
    public class RetriedParameterizedTestSuiteMethod : ParameterizedMethodSuite
    {
        private readonly int _tryCount;
        private readonly int _requiredPassCount;

        public RetriedParameterizedTestSuiteMethod(MethodInfo method, int run, int requiredPass)
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
                        return result;
                    }
                }
                else
                {
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
}
