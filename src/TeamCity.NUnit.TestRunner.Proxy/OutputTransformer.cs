// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace TeamCity.NUnit.TestRunner.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using TypeScanner;

    public class OutputTransformer : IOutputTransformer
    {
        private readonly IList<IProxyExtension> extensions = new List<IProxyExtension>();

        public OutputTransformer(IEnumerable<string> proxies, ITypeScanner scanner)
        {
            var types = scanner.GetTypesOf<IProxyExtension>().Where(x => x.IsClass && !x.IsAbstract);

            var instances = types.Select(GetProxyFromType).ToArray();

            foreach (var proxy in proxies)
            {
                if (instances.All(x => !MatchesProxy(x, proxy)))
                {
                    Console.Error.WriteLine("Error: Could not find a proxy named " + proxy);
                    Environment.Exit(ExitCodes.ProxyNotFound);
                }

                this.extensions.Add(instances.First(x => MatchesProxy(x, proxy)));
            }
        }

        public XDocument TransformOutput(XDocument output)
        {
            return this.GetOrderedProxies()
                .Aggregate(output, (current, proxyExtension) => proxyExtension.TransformOutput(current));
        }

        private static IProxyExtension GetProxyFromType(Type x)
        {
            return (IProxyExtension)Activator.CreateInstance(x);
        }

        private static bool MatchesProxy(IProxyExtension x, string proxy)
        {
            return String.Equals(x.ExtensionName, proxy, StringComparison.InvariantCultureIgnoreCase);
        }

        private IEnumerable<IProxyExtension> GetOrderedProxies()
        {
            return this.extensions.OrderByDescending(x => x.Priority);
        }
    }
}
