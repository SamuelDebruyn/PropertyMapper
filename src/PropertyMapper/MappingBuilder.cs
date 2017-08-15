using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;

namespace PropertyMapper
{
    internal static class Constants
    {
        internal const string InterfaceName = "IMapper";
        internal const string ClassName = "PropertyMapper";
        internal const string InstanceName = "instance";
        internal const string MapMethodName = "Map";
    }

    public static class MappingBuilder
    {
        public static string BuildMappings(MapperConfiguration config, bool enableStringCopy = false)
        {
            var typeMaps = BuildCompiledMaps(config);
            return BuildOutput(typeMaps, enableStringCopy);
        }

        public static string BuildMappings(Dictionary<string, string> givenTypeMaps,
            bool scanReferencedAssembliesForTypes = true, bool enableStringCopy = false)
        {
            var config = BuildMapperConfig(givenTypeMaps, scanReferencedAssembliesForTypes);
            return BuildMappings(config, enableStringCopy);
        }

        static string BuildOutput(ICollection<TypeMap> typeMaps, bool enableStringCopy = false)
        {
            var stringBuilder = new StringBuilder();

            AppendInterfaces(typeMaps, stringBuilder);
            AppendClasses(typeMaps, stringBuilder, enableStringCopy);

            return stringBuilder.ToString();
        }

        static void AppendClasses(IEnumerable<TypeMap> typeMaps, StringBuilder stringBuilder,
            bool enableStringCopy = false)
        {
            stringBuilder.AppendLine($"public class {Constants.ClassName}: {Constants.InterfaceName}");
            stringBuilder.AppendLine("{");

            foreach (var typeMap in typeMaps)
            {
                AppendClassMapping(typeMap, stringBuilder, typeMaps, enableStringCopy);
            }

            stringBuilder.AppendLine("}");
        }

        static void AppendClassMapping(TypeMap typeMap, StringBuilder stringBuilder,
            IEnumerable<TypeMap> parentTypeMaps, bool enableStringCopy = false)
        {
            stringBuilder.AppendLine(
                $"global::{typeMap.DestinationType.FullName} {Constants.InterfaceName}<global::{typeMap.SourceType.FullName}, global::{typeMap.DestinationType.FullName}>.{Constants.MapMethodName}(global::{typeMap.SourceType.FullName} {Constants.InstanceName}) => new global::{typeMap.DestinationType.FullName}");
            stringBuilder.AppendLine("{");
            var propertyMaps = typeMap.GetPropertyMaps();

            foreach (var propertyMap in propertyMaps)
            {
                AppendPropertyMapping(stringBuilder, propertyMap, parentTypeMaps, enableStringCopy);
            }

            stringBuilder.AppendLine("};");
        }

        static void AppendPropertyMapping(StringBuilder stringBuilder, PropertyMap propertyMap,
            IEnumerable<TypeMap> parentTypeMaps, bool enableStringCopy = false)
        {
            string valueExp;


            if ((propertyMap.SourceType.GetTypeInfo().IsPrimitive &&
                 propertyMap.DestinationPropertyType.GetTypeInfo().IsPrimitive)
                || (propertyMap.SourceType == typeof(string) && propertyMap.DestinationPropertyType == typeof(string) &&
                    !enableStringCopy)
                || (propertyMap.SourceType == typeof(object) && propertyMap.DestinationPropertyType == typeof(object)))
            {
                valueExp = $"{Constants.InstanceName}{GetSourceMembersPath(propertyMap)}";
            }
            else if (enableStringCopy && propertyMap.SourceType == typeof(string) &&
                     propertyMap.DestinationPropertyType == typeof(string))
            {
                valueExp = $"new string({Constants.InstanceName}{GetSourceMembersPath(propertyMap)}.ToCharArray())";
            }
            else if (propertyMap.SourceType.GetTypeInfo().IsEnum &&
                     propertyMap.DestinationPropertyType.GetTypeInfo().IsEnum)
            {
                valueExp =
                    $"(global::{propertyMap.DestinationPropertyType.FullName})(int){Constants.InstanceName}{GetSourceMembersPath(propertyMap)}";
            }
            else if (propertyMap.SourceType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IEnumerable)) &&
                     propertyMap.DestinationPropertyType.GetTypeInfo().ImplementedInterfaces
                         .Contains(typeof(IEnumerable)))
            {
                var genericSourceType = propertyMap.SourceType.GenericTypeArguments.FirstOrDefault();
                var genericDestinationType = propertyMap.DestinationPropertyType.GenericTypeArguments.FirstOrDefault();
                if (genericSourceType == null || genericDestinationType == null)
                {
                    throw new ArgumentException($"Generics are required in {propertyMap.SourceMember.Name} ({propertyMap.TypeMap.SourceType.FullName}) and {propertyMap.DestinationProperty.Name} ({propertyMap.TypeMap.DestinationType.FullName}).");
                }
                
                if (!parentTypeMaps.Any(pt =>
                    pt.SourceType == genericSourceType &&
                    pt.DestinationType == genericDestinationType))
                {
                    throw new ArgumentException(
                        $"The mapping from property {propertyMap.SourceMember.Name} to property {propertyMap.DestinationProperty.Name} requires a type map from {genericSourceType.FullName} to {genericDestinationType.FullName}.");
                }
                
                
                var destinationGenerics = $"<global::{genericDestinationType.FullName}>";

                valueExp = $"new global::{propertyMap.DestinationPropertyType.GetNameWithoutGenericArity()}{destinationGenerics}(System.Linq.Enumerable.Select({Constants.InstanceName}{GetSourceMembersPath(propertyMap)}, x => (({Constants.InterfaceName}<global::{genericSourceType.FullName}, global::{genericDestinationType.FullName}>)this).{Constants.MapMethodName}(x)))";
            }
            else
            {
                if (!parentTypeMaps.Any(pt =>
                    pt.SourceType == propertyMap.SourceType &&
                    pt.DestinationType == propertyMap.DestinationPropertyType))
                {
                    throw new ArgumentException(
                        $"The mapping from property {propertyMap.SourceMember.Name} to property {propertyMap.DestinationProperty.Name} requires a type map from {propertyMap.SourceType.FullName} to {propertyMap.DestinationPropertyType.FullName}.");
                }

                valueExp =
                    $"(({Constants.InterfaceName}<global::{propertyMap.SourceType.FullName}, global::{propertyMap.DestinationPropertyType.FullName}>)this).{Constants.MapMethodName}({Constants.InstanceName}{GetSourceMembersPath(propertyMap)})";
            }

            stringBuilder.AppendLine($"{propertyMap.DestinationProperty.Name} = {valueExp},");
        }

        static string GetSourceMembersPath(PropertyMap propertyMap)
        {
            var inlineBuilder = new StringBuilder();

            foreach (var member in propertyMap.SourceMembers)
            {
                inlineBuilder.Append("." + member.Name);
            }

            return inlineBuilder.ToString();
        }

        static void AppendInterfaces(IEnumerable<TypeMap> typeMaps, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine($"public interface {Constants.InterfaceName}<in TSource, out TDestination>");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine($"TDestination {Constants.MapMethodName}(TSource {Constants.InstanceName});");
            stringBuilder.AppendLine("}");
            stringBuilder.Append($"public interface {Constants.InterfaceName}:");

            foreach (var typeMap in typeMaps)
                stringBuilder.Append(
                    $" {Constants.InterfaceName}<global::{typeMap.SourceType.FullName}, global::{typeMap.DestinationType.FullName}>,");

            stringBuilder.Length--;
            stringBuilder.AppendLine("{ }");
        }

        static TypeMap[] BuildCompiledMaps(IConfigurationProvider config)
        {
            var mapper = config.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            mapper.ConfigurationProvider.CompileMappings();

            return mapper.ConfigurationProvider.GetAllTypeMaps();
        }

        static MapperConfiguration BuildMapperConfig(Dictionary<string, string> givenTypeMaps,
            bool scanReferencedAssembliesForTypes = false)
        {
            var assemblyTypes = (scanReferencedAssembliesForTypes
                ? Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load)
                    .SelectMany(a => a.DefinedTypes)
                : Assembly.GetEntryAssembly().DefinedTypes).ToList();

            var usingFullNames = givenTypeMaps.SelectMany(kvp => new[] {kvp.Key, kvp.Value}).Any(s => s.Contains("."));

            return new MapperConfiguration(cfg =>
            {
                foreach (var givenTypeMap in givenTypeMaps)
                {
                    var source = assemblyTypes.First(ti =>
                        usingFullNames ? ti.FullName.Contains(givenTypeMap.Key) : ti.Name.Equals(givenTypeMap.Key));
                    var destination = assemblyTypes.First(ti =>
                        usingFullNames ? ti.FullName.Contains(givenTypeMap.Value) : ti.Name.Equals(givenTypeMap.Value));

                    cfg.CreateMap(source.AsType(), destination.AsType());
                }
            });
        }

        static string GetNameWithoutGenericArity(this Type type)
        {
            var name = type.FullName;
            var index = name.IndexOf('`');
            return index == -1 ? name : name.Substring(0, index);
        }
    }
}