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
                cfg.CreateMap<Person, PersonExample1>();
            }));

            Console.WriteLine(mappingResult);
            Clipboard.Copy(mappingResult);
        }
    }
}