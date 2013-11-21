// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace TeamCity.NUnit.TestRunner.Proxy
{
    public interface IProxyExtension : IOutputTransformer
    {
        string ExtensionName { get; }
        ProcessPriority Priority { get; }
    }
}
