// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        // TODO: Replace with your actual CosmosDB connection details

        string outputFile = "./export/";

        CosmosClient client = new CosmosClient(endpointUri, primaryKey);
        Container container = client.GetContainer(databaseId, containerId);

        var query = "SELECT * FROM c ORDER BY c._ts ASC";
        var iterator = container.GetItemQueryIterator<dynamic>(query);
        var groups = new Dictionary<string, List<dynamic>>();

        while (iterator.HasMoreResults)
        {
            foreach (var item in await iterator.ReadNextAsync())
            {
                // Get _ts value and convert to year-month
                if (item._ts != null)
                {
                    var dt = DateTimeOffset.FromUnixTimeSeconds((long)item._ts);
                    string ym = $"{dt:yyyy-MM}";
                    
                    if (!groups.ContainsKey(ym))
                    {
                        Console.WriteLine($"New group {ym} created");
                        groups[ym] = new List<dynamic>();
                    }

                    groups[ym].Add(item);
                }
            }
        }

        foreach (var kvp in groups)
        {
            string fileName = $"{kvp.Key}.json";
            string json = JsonConvert.SerializeObject(kvp.Value, Formatting.Indented);
            await File.WriteAllTextAsync(fileName, json);
            Console.WriteLine($"Exported {kvp.Value.Count} documents to {fileName}");
        }
    }
}
