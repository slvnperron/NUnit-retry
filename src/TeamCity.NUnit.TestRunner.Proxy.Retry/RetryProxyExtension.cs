// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace TeamCity.NUnit.TestRunner.Proxy.Retry
{
    using System.Xml.Linq;

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
            return output;
        }
    }
}
