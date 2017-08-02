# SimpleMapper
Creates dead simple property mappings for C# types

## What it does

So we all like [AutoMapper](http://automapper.org/), right? Sometimes it's a little bit overkill and not worth the performance cost.

If all of your mappings have the default configurations (so no custom member mappings), it'd be a lot of easier if you'd just have a class that converts the objects.

This code generates that class. It uses AutoMapper underneath so you can enjoy some nifty AutoMapper features like flattening.

## What it generates

* this interface: `public interface IMapper<TSource, TDestination>{ TDestination Map(TSource instance); };`
* an interface `IMapper` implementing the generic `IMapper` for all the mappings you've defined
* a class called `SimpleMapper` with `Map` methods for all your mappings so that it implements the non-generic `IMapper` interface

## How do you use it

1. Create a CSV-file (delimiter is `;` or change it in the source code of `Program.cs`) with source types and destination types. Just use the class names, no namespaces required.
1. Install the [.NET Core SDK](https://www.microsoft.com/net/download/core#/sdk)
1. Put all the required classes in the same directory as this code
1. Run `dotnet run path_to_your.csv`

It outputs the code on STDOUT, use ` > SimpleMapper.cs` to pipe it to a file.

## What still needs to be fixed:

* Support namespaces
