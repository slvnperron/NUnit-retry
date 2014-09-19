NUnit-retry [![Build Status](http://teamcity.protorisk.com/app/rest/builds/buildType:(id:NUnitRetry_NUnitRetry)/statusIcon)](http://teamcity.protorisk.com)
===========

A NUnit plugin that retries intermittently failing tests.

Why?
----

Maybe you have an automated regression test suite. As part of that suite you have tests dependent on external libraries, consuming remote resources or running under unreliable tools.
Then you have a continuous integration server running this test suite and it's always failing because of some intermittently failing tests.
The goal of this plugin is to ensure that tests are passing at least X times out of N tries – thus hedging against intermittently failing components of the suite.

What you must consider
----------------------

Martin Fowler described very well [in an article](http://martinfowler.com/articles/nonDeterminism.html) why non-deterministic tests are bad and why you should get rid of them.
This library is not intended to bypass these recommandations but help you manage your quarantined tests.
You must be aware that by using this library, you are not testing the entire process anymore but are rather testing for specific functionnality scenarios.

Installation
------------
- Copy nunit-retry.dll inside your NUnit 2.6.3 addins folder.
- Add a reference to nunit-retry.dll to your project. The project is available on NuGet (Install-Package NUnit-retry)

How to use it
-------------

Just use the RetryAttribute on a TestMethod or a test fixture:
``` c#
        
        private static int run = 0;
        
        ...
        
        [Test]
        [Retry(Times = 3, RequiredPassCount = 2)]
        public void One_Failure_On_Three_Should_Pass()
        {
            run++;

            if (run == 1)
            {
                Assert.Fail();
            }

            Assert.Pass();
        }
```
