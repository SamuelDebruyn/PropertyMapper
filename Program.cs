using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleMapper
{
    class Program
    {
        const string DefaultMappingsCsv = "mappings.csv";
        const string Delimiter = ";";

        static void Main(string[] args)
        {
            var givenTypeMaps = BuildTypeMaps(args);
            var output = MappingBuilder.BuildMappings(givenTypeMaps, false);
            Console.WriteLine(output);
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
