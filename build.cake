var target = Argument("target", "Default");

Task("Default").Does(() => 
{
    DotNetCoreRestore();
    DotNetCoreBuild("SimpleMapper.csproj");
    DotNetCorePack("SimpleMapper.csproj", new DotNetCorePackSettings
    {
        VersionSuffix = EnvironmentVariable("BITRISE_BUILD_NUMBER"),
        OutputDirectory = new DirectoryPath(EnvironmentVariable("BITRISE_DEPLOY_DIR"))
    });
});

RunTarget(target);