using System;
using AutoMapper;
using PropertyMapper.Sample.Destinations;
using PropertyMapper.Sample.Helpers;
using PropertyMapper.Sample.Sources;

namespace PropertyMapper.Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mappingResult = MappingBuilder.BuildMappings(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sources.PersonExample1, Destinations.PersonExample1>();
                cfg.CreateMap<Location, LocationExample1>();
                cfg.CreateMap<Sources.PersonExample1, Destinations.PersonExample2>();
                cfg.CreateMap<Sources.PersonExample1, Destinations.PersonExample3>();
                cfg.CreateMap<Id, IdExample1>();
                cfg.CreateMap<Sources.PersonExample1, PersonExample4>();
                cfg.CreateMap<Sources.PersonExample1, PersonExample5>();
                cfg.CreateMap<Sources.PersonExample2, PersonExample6>();
                cfg.CreateMap<Sources.PersonExample3, PersonExample7>();
            }), false);

            Console.WriteLine(mappingResult);
            Clipboard.Copy(mappingResult);
        }
    }
}