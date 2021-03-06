# PropertyMapper
Creates dead simple property mappings for C# types

![build status](https://www.bitrise.io/app/c8e79f88c507dd00.svg?token=3lmHZGyGgefyN5nxSVpgjw) [![NuGet](https://img.shields.io/nuget/v/ChipsnCookies.PropertyMapper.svg?label=NuGet)](https://www.nuget.org/packages/ChipsnCookies.PropertyMapper/)

```
Install-Package ChipsnCookies.PropertyMapper
```

## What it does

So we all like [AutoMapper](http://automapper.org/), right? Sometimes it's a little bit overkill and not worth the performance cost.

If all of your mappings have the default configurations (so no custom member mappings), it'd be a lot of easier if you'd just have a class that converts the objects.

This code generates that class. It uses AutoMapper underneath so you can enjoy some nifty AutoMapper features like flattening.

## What it generates

* this interface: `public interface IMapper<in TSource, out TDestination> { TDestination Map(TSource instance); }`
* an interface `IMapper` implementing the generic `IMapper` for all the mappings you've defined
* a class called `PropertyMapper` with `Map` methods for all your mappings so that it implements the non-generic `IMapper` interface

Note that you need to use the `PropertyMapper` instance as `IMapper` because all the map methods have the same name and are implemented explicitly.

## How can you use it

The source includes a .NET Core app that can read a CSV file with your mappings. There's also a NuGet package that has the same functionality exposed as methods.

## Limitations (not supported)

* Non default constructors
* Non public setters