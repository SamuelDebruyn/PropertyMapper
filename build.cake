var target = Argument("target", "Default");

Task("Default").Does(() => 
{
    DotNetCoreRestore();
    DotNetCoreBuild("SimpleMapper.sln");
});

RunTarget(target);