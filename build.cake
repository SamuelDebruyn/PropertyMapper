var target = Argument("target", "Default");

Task("Default").Does(() => 
{
    DotNetCoreRestore();
    DotNetCorePack("SimpleMapper.csproj", new DotNetCorePackSettings
    {
        VersionSuffix = "alpha" + EnvironmentVariable("BITRISE_BUILD_NUMBER"),
        // OutputDirectory = new DirectoryPath(EnvironmentVariable("BITRISE_DEPLOY_DIR"))
    });
});

RunTarget(target);