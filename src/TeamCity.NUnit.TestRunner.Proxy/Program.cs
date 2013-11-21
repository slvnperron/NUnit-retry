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

    using TypeScanner;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var argsBindingConfiguration = Args.Configuration.Configure<ArgsConfiguration>();
            var config = argsBindingConfiguration.CreateAndBind(args);
            ValidateConfiguration(config);

            var outputTransformer = new OutputTransformer(config.Proxies, new TypeScanner());
            var proxy = new SynchronousTestRunnerProxy(outputTransformer);

            proxy.Execute(config.TestRunnerPath, args);
        }

        private static void ValidateConfiguration(ArgsConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration.TestRunnerPath))
            {
                Console.Error.WriteLine("Bad configuration: TestRunnerPath must be configured");
                Environment.Exit(ExitCodes.BadConfiguration);
            }

            if (configuration.Proxies.Count <= 0)
            {
                Console.Error.WriteLine("Bad configuration: You must specify at least one proxy to use.");
                Environment.Exit(ExitCodes.BadConfiguration);
            }

            if (!File.Exists(configuration.TestRunnerPath))
            {
                Console.Error.WriteLine("Bad configuration: Path to TestRunner must be valid and file must exist.");
                Environment.Exit(ExitCodes.BadConfiguration);
            }
        }

        private class ArgsConfiguration
        {
            public List<string> Proxies { get; set; }
            public string TestRunnerPath { get; set; }
        }
    }
}
