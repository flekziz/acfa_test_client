using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cake.Common.IO;
using Cake.Compression;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}

public class BuildContext : FrostingContext
{
    public string Target { get; }
    public string BuildConfiguration { get; }
    public string BuildPlatform { get; }
    public string RuntimeIdentifier { get; }
    public string Version { get; }


    public BuildContext(ICakeContext context)
        : base(context)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Target = context.Arguments.GetArgument("target");
        BuildConfiguration = context.Arguments.GetArgument("build_configuration") ?? "debug";
        BuildPlatform = context.Arguments.GetArgument("build_platform") ?? "any";
        RuntimeIdentifier = context.Arguments.GetArgument("build_rid") ?? "any";
        Version = context.Arguments.GetArgument("build_version") ?? "0.0.0";
    }
}
