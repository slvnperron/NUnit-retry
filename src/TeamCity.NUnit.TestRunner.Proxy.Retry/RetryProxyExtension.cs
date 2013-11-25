// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace TeamCity.NUnit.TestRunner.Proxy.Retry
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class RetryProxyExtension : IProxyExtension
    {
        public string ExtensionName
        {
            get
            {
                return "retry";
            }
        }

        public ProcessPriority Priority
        {
            get
            {
                return ProcessPriority.Normal;
            }
        }

        public XDocument TransformOutput(XDocument output)
        {
            var elements = output.XPathSelectElements("//jetbrains.buildServer.messages.BuildMessage1"
                                                      + "[./myTypeId[text()='BlockStart'] and "
                                                      + ".//blockType[text() ='$TEST_BLOCK$']]");

            var tests = elements.Select(x => new TestResult 
            { 
                FullName = x.XPathSelectElement(".//blockName").Value, 
                RunTime = long.Parse(x.XPathSelectElement(".//myTimestamp").Value),
                Succeeded = x.NextNode.XPathSelectElement(".//myTypeId").Value != "TestFailure"
            })
            .OrderBy(x => x.RunTime);

            var testsToRemove = from testResult in tests.GroupBy(x => x.FullName)
                where testResult.Any(x => x.Succeeded)
                select testResult.Key;

            foreach (var testToRemove in testsToRemove)
            {
                output.XPathSelectElements(
                    "//jetbrains.buildServer.messages.BuildMessage1[./myTypeId[text()='TestFailure'] and .//testName[text()='"
                    + testToRemove + "']]").Remove();
            }

            return output;
        }

        public class TestResult
        {
            public string FullName { get; set; }

            public long RunTime { get; set; }

            public bool Succeeded { get; set; }
        }
    }
}
