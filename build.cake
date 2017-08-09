var target = Argument("target", "Default");

Task("Default").Does(() => 
{
    Information("Running cake script");

    Information("Restoring packages");
    DotNetCoreRestore("src/PropertyMapper.sln");

    Information("Building projects");
    DotNetCoreBuild("src/PropertyMapper.sln", new DotNetCoreBuildSettings()
    {
        Configuration = "Release"
    });

    Information("Creating NuGet package");
    NuGetPack("package.nuspec", new NuGetPackSettings
    {
        Version = EnvironmentVariable("MAIN_VERSION") + "-build" + EnvironmentVariable("BITRISE_BUILD_NUMBER"),
        OutputDirectory = new DirectoryPath(EnvironmentVariable("BITRISE_DEPLOY_DIR"))
    });
});

RunTarget(target);