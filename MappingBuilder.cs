using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;

namespace SimpleMapper
{
    public static class MappingBuilder
    {
        public static string BuildMappings(MapperConfiguration config)
        {
            var typeMaps = BuildCompiledMaps(config);
            return BuildOutput(typeMaps);
        }

        public static string BuildMappings(Dictionary<string, string> givenTypeMaps, bool scanReferencedAssembliesForTypes = true)
        {
            var config = BuildMapperConfig(givenTypeMaps, scanReferencedAssembliesForTypes);
            return BuildMappings(config);
        }

        static string BuildOutput(ICollection<TypeMap> typeMaps)
        {
            var stringBuilder = new StringBuilder();

            AppendInterfaces(typeMaps, stringBuilder);
            AppendClasses(typeMaps, stringBuilder);

            return stringBuilder.ToString();
        }

        static void AppendClasses(IEnumerable<TypeMap> typeMaps, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("public class SimpleMapper: IMapper {");

            foreach (var typeMap in typeMaps)
            {
                stringBuilder.AppendLine($"public {typeMap.DestinationType.FullName} Map({typeMap.SourceType.FullName} instance) => new {typeMap.DestinationType.FullName}");
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
                stringBuilder.Append($" IMapper<{typeMap.SourceType.FullName}, {typeMap.DestinationType.FullName}>,");
            }

            stringBuilder.Length--;

            stringBuilder.AppendLine("{");

            foreach (var typeMap in typeMaps)
            {
                stringBuilder.AppendLine($"{typeMap.DestinationType.FullName} Map({typeMap.SourceType.FullName} instance);");
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

        static MapperConfiguration BuildMapperConfig(Dictionary<string, string> givenTypeMaps, bool scanReferencedAssembliesForTypes = false)
        {
            var assemblyTypes = (scanReferencedAssembliesForTypes
                ? Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load).SelectMany(a => a.DefinedTypes)
                : Assembly.GetEntryAssembly().DefinedTypes).ToList();

            var usingFullNames = givenTypeMaps.SelectMany(kvp => new []{kvp.Key, kvp.Value}).Any(s => s.Contains("."));

            return new MapperConfiguration(cfg =>
            {
                foreach (var givenTypeMap in givenTypeMaps)
                {
                    var source = assemblyTypes.First(ti => usingFullNames ? ti.FullName.Contains(givenTypeMap.Key) : ti.Name.Equals(givenTypeMap.Key));
                    var destination = assemblyTypes.First(ti => usingFullNames ? ti.FullName.Contains(givenTypeMap.Value): ti.Name.Equals(givenTypeMap.Value));

                    cfg.CreateMap(source.AsType(), destination.AsType());
                }
            });
        }
    }
}