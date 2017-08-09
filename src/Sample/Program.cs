using System;
using AutoMapper;

namespace PropertyMapper.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var mappingResult = MappingBuilder.BuildMappings(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sources.Person, Destinations.PersonExample1>();
            }));
            
            Console.WriteLine(mappingResult);
            Clipboard.Copy(mappingResult);
        }
    }
}
