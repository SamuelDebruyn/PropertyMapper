using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;

namespace mappingapp
{
    class Program
    {
        const string DefaultMappingsCsv = "mappings.csv";
        const string Delimiter = ";";

        static void Main(string[] args)
        {
            var givenTypeMaps = BuildTypeMaps(args);
            var config = BuildMapperConfig(givenTypeMaps);
            var typeMaps = BuildCompiledMaps(config);
            var output = BuildOutput(typeMaps);
            Console.WriteLine(output);
        }

        static string BuildOutput(ICollection<TypeMap> typeMaps)
        {
            var stringBuilder = new StringBuilder();

            AppendInterfaces(typeMaps, stringBuilder);
            AppendClasses(typeMaps, stringBuilder);

            return stringBuilder.ToString();
        }

        static void AppendClasses(ICollection<TypeMap> typeMaps, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("public class SimpleMapper: IMapper {");

            foreach (var typeMap in typeMaps)
            {
                stringBuilder.AppendLine($"public {typeMap.DestinationType.Name} Map({typeMap.SourceType.Name} instance) => new {typeMap.DestinationType.Name}");
                stringBuilder.AppendLine("{");
                var propertyMaps = typeMap.GetPropertyMaps();

                foreach (var propertyMap in propertyMaps)
                {
                    stringBuilder.AppendLine($"{propertyMap.SourceMember.Name} = instance.{propertyMap.DestinationProperty.Name},");
                }
                stringBuilder.AppendLine("};");
            }

            stringBuilder.AppendLine("}");
        }

        static void AppendInterfaces(ICollection<TypeMap> typeMaps, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("public interface IMapper<TSource, TDestination>{ TDestination Map(TSource instance); }");

            stringBuilder.Append("public interface IMapper:");
            foreach (var typeMap in typeMaps)
            {
                stringBuilder.Append($" IMapper<{typeMap.SourceType.Name}, {typeMap.DestinationType.Name}>,");
            }

            stringBuilder.Length--;

            stringBuilder.AppendLine("{");

            foreach (var typeMap in typeMaps)
            {
                stringBuilder.AppendLine($"{typeMap.DestinationType.Name} Map({typeMap.SourceType.Name} instance);");
            }

            stringBuilder.AppendLine("}");
        }

        static TypeMap[] BuildCompiledMaps(IConfigurationProvider config)
        {
            var mapper = config.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            mapper.ConfigurationProvider.CompileMappings();

            return mapper.ConfigurationProvider.GetAllTypeMaps();
        }

        static MapperConfiguration BuildMapperConfig(Dictionary<string, string> givenTypeMaps)
        {
            var assemblyTypes = Assembly.GetEntryAssembly().DefinedTypes.ToList();

            return new MapperConfiguration(cfg =>
            {
                foreach (var givenTypeMap in givenTypeMaps)
                {
                    var source = assemblyTypes.First(ti => ti.Name.Equals(givenTypeMap.Key));
                    var destination = assemblyTypes.First(ti => ti.Name.Equals(givenTypeMap.Value));

                    cfg.CreateMap(source.AsType(), destination.AsType());
                }
            });
        }

        static Dictionary<string, string> BuildTypeMaps(IEnumerable<string> args)
        {
            var givenTypeMaps = new Dictionary<string, string>();

            var givenPath = args.FirstOrDefault();
            var path = string.IsNullOrEmpty(givenPath) ? DefaultMappingsCsv : givenPath;
            using (var fileReader = File.OpenText(path))
            {
                using (var csvReader = new CsvReader(fileReader, new CsvConfiguration{HasHeaderRecord = false, SkipEmptyRecords = true, Delimiter = Delimiter}))
                {
                    while (csvReader.Read())
                    {
                        var record = csvReader.CurrentRecord;
                        givenTypeMaps.Add(record[0], record[1]);
                    }
                }
            }
            return givenTypeMaps;
        }
    }
}
