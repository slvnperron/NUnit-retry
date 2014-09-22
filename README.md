NUnit-retry [![Build Status](http://teamcity.protorisk.com/app/rest/builds/buildType:(id:NUnitRetry_NUnitRetry)/statusIcon)](http://teamcity.protorisk.com)
===========

A NUnit plugin that retries intermittently failing tests.

Why?
--------

Maybe you have an automated regression test suite. As part of that suite you have tests dependent on external libraries, consuming remote resources or running under unreliable tools.
Then you have a continuous integration server running this test suite and it's failing because of some intermittently failing tests.
The goal of this plugin is to ensure that tests are passing at least X times out of N tries – thus hedging against intermittently failing components of the suite.

Get it on NuGet
---------------------
```
Install-Package NUnit-retry
```

Installation 
--------------
- Add a reference to nunit-retry.dll to your project. The project is available on NuGet
- (Optional*) Copy nunit-retry.dll inside your NUnit 2.6.3 addins folder.

* Only if you need the Retry capability on that machine.

Compatibility
-----------------

I couldn't get it working with ReSharper's Test Runner. The only way I could get the addin loaded is via the nunit-console or unit-gui. In TeamCity, I use nunit-console and export the test results in an xml file then add a build feature that parses the xml.

How to use it
------------------

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

Contributions
------------------

Contributions are welcomed, but before you start working towards a new feature or bug, please visit the Issue section. Make sure there's an issue (or create one) so we can discuss the feature or bug before you spend time coding it. It will ease the development and the code review.

Contact
----------

Email: sylvain.perron@protorisk.com
