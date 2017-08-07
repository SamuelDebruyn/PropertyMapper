var target = Argument("target", "Default");

Task("Default").Does(() => 
{
    DotNetCoreRestore();
    DotNetCoreBuild("mappingapp.csproj");
});

RunTarget(target);