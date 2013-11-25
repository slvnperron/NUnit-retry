// /////////////////////////////////////////////////////////////////////
//  This is free software licensed under the NUnit license. You
//  may obtain a copy of the license as well as information regarding
//  copyright ownership at http://nunit.org.    
// /////////////////////////////////////////////////////////////////////

namespace TeamCity.NUnit.TestRunner.Proxy
{
    using System;
    using System.Diagnostics;
    using System.Xml.Linq;

    public class SynchronousTestRunnerProxy
    {
        private readonly IOutputTransformer transformer;

        public SynchronousTestRunnerProxy(IOutputTransformer transformer)
        {
            this.transformer = transformer;
        }

        public void Execute(string testRunnerPath, string[] args)
        {
            var processStartInfo = new ProcessStartInfo(testRunnerPath, string.Join(" ", args))
                                   {
                                       RedirectStandardOutput = true,
                                       UseShellExecute = false
                                   };

            var processs = new Process { StartInfo = processStartInfo };

            processs.Start();
            var output = processs.StandardOutput.ReadToEnd();
            processs.WaitForExit();

            var xmlDocument = this.TranformToXDocument(output);

            xmlDocument = this.transformer.TransformOutput(xmlDocument);

            Console.Out.Write(this.TransformToString(xmlDocument));
        }

        public XDocument TranformToXDocument(string document)
        {
            return XDocument.Parse(string.Format("<root>{0}</root>", document));
        }

        public string TransformToString(XDocument document)
        {
            return document.Root == null ? string.Empty : string.Join("", document.Root.Nodes());
        }
    }
}
