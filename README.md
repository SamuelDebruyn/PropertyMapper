# PropertyMapper
Creates dead simple property mappings for C# types

![build status](https://www.bitrise.io/app/c8e79f88c507dd00.svg?token=3lmHZGyGgefyN5nxSVpgjw)

## What it does

So we all like [AutoMapper](http://automapper.org/), right? Sometimes it's a little bit overkill and not worth the performance cost.

If all of your mappings have the default configurations (so no custom member mappings), it'd be a lot of easier if you'd just have a class that converts the objects.

This code generates that class. It uses AutoMapper underneath so you can enjoy some nifty AutoMapper features like flattening.

## What it generates

* this interface: `public interface IMapper<TSource, TDestination>{ TDestination Map(TSource instance); };`
* an interface `IMapper` implementing the generic `IMapper` for all the mappings you've defined
* a class called `PropertyMapper` with `Map` methods for all your mappings so that it implements the non-generic `IMapper` interface

## How can you use it

The source includes a .NET Core app that can read a CSV file with your mappings. There's also a NuGet package that has the same functionality exposed as methods.
