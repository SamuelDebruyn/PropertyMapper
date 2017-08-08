var target = Argument("target", "Default");

Task("Default").Does(() => 
{
    DotNetCoreRestore("src/SimpleMapper.sln");
    DotNetCoreBuild("src/SimpleMapper.sln", new DotNetCoreBuildSettings()
    {
        Configuration = "Release"
    });
    NuGetPack("package.nuspec", new NuGetPackSettings
    {
        Version = "1.0.EnvironmentVariable("BITRISE_BUILD_NUMBER")",
        OutputDirectory = new DirectoryPath(EnvironmentVariable("BITRISE_DEPLOY_DIR"))
    });
});

RunTarget(target);