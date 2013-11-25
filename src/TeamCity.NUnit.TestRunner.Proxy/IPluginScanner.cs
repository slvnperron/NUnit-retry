// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace TeamCity.NUnit.TestRunner.Proxy
{
    using System;
    using System.Collections.Generic;

    public interface IPluginScanner
    {
        IEnumerable<Type> Scan();
    }
}
