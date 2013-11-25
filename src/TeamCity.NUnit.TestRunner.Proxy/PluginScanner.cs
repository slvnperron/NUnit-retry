// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace TeamCity.NUnit.TestRunner.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class PluginScanner : IPluginScanner
    {
        public IEnumerable<Type> Scan()
        {
            var pluginDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var pluginAssemblies = from file in new DirectoryInfo(pluginDirectory).GetFiles()
                where file.Extension == ".dll"
                select Assembly.LoadFile(file.FullName);

            return from dll in pluginAssemblies
                from type in dll.GetExportedTypes()
                where typeof(IProxyExtension).IsAssignableFrom(type)
                where !type.IsAbstract
                where !type.IsGenericTypeDefinition
                select type;
        }
    }
}
