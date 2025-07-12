using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Net.Http.Json;
using BeachWeatherStation.Console.DTOs;

class Program
{
    static async Task Main(string[] args)
    {
        bool exit = false;
        
        // Main application loop
        while (!exit)
        {
            // Display menu options
            Console.WriteLine("\n=== Beach Weather Station Data Tool ===");
            Console.WriteLine("Choose operation:");
            Console.WriteLine("1. Export data from Cosmos DB");
            Console.WriteLine("2. Import data to API");
            Console.WriteLine("3. Exit");
            Console.Write("\nEnter choice (1-3): ");
            
            var choice = Console.ReadLine();
            
            // Process user choice
            switch (choice)
            {
                case "1":
                    await Export();
                    break;
                case "2":
                    await Import();
                    break;
                case "3":
                    exit = true;
                    Console.WriteLine("Exiting application...");
                    break;
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        }
    }

    private static async Task Import()
    {
        string importDirectory = "./export";
        //string defaultApiUrl = "http://localhost:7071/v2/readings";
        string defaultApiUrl = "http://iot.jacobmohl.dk/v2/readings?code=5geW9gCmH2QjJLdSqJNY1ujISCGRQQCtOer9DJ28Oli5oeaFssui2w==";
        int importedCount = 0;
        int errorCount = 0;
        int skippedCount = 0;
        
        Console.WriteLine("Starting import process...");
        
        Console.WriteLine($"Enter API URL (default: {defaultApiUrl}):");
        string? customApiUrl = Console.ReadLine();
        string apiBaseUrl = string.IsNullOrEmpty(customApiUrl) ? defaultApiUrl : customApiUrl;
        
        Console.WriteLine("Do you want to import all files or specify a date range? (all/range)");
        var rangeOption = Console.ReadLine()?.ToLower();
        
        // Get all JSON files from export directory
        var allFiles = Directory.GetFiles(importDirectory, "*.json");
        
        if (allFiles.Length == 0)
        {
            Console.WriteLine("No export files found in directory.");
            return;
        }
        
        List<string> files = new List<string>();
        
        if (rangeOption == "range")
        {
            Console.WriteLine("Enter start date (yyyy-MM):");
            var startDate = Console.ReadLine();
            
            Console.WriteLine("Enter end date (yyyy-MM):");
            var endDate = Console.ReadLine();
            
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                files = allFiles.Where(f => 
                {
                    var fileName = Path.GetFileNameWithoutExtension(f);
                    return string.Compare(fileName, startDate) >= 0 && 
                           string.Compare(fileName, endDate) <= 0;
                }).ToList();
                
                Console.WriteLine($"Found {files.Count} files in the specified date range.");
            }
        }
        else
        {
            files = allFiles.ToList();
            Console.WriteLine($"Found {files.Count} files to process.");
        }
        
        if (files.Count == 0)
        {
            Console.WriteLine("No files to process based on your selection.");
            return;
        }
        
        using var httpClient = new HttpClient();
        
        foreach (var file in files)
        {
            Console.WriteLine($"Processing file: {Path.GetFileName(file)}");
            
            string json = await File.ReadAllTextAsync(file);
            var readings = JsonConvert.DeserializeObject<List<SensorReadingDto>>(json);
            
            if (readings == null || readings.Count == 0)
            {
                Console.WriteLine($"No readings found in {file}");
                continue;
            }
            
            Console.WriteLine($"Found {readings.Count} readings in file.");
            
            Console.WriteLine("Enter batch size (number of readings to import at once, default 100):");
            string? batchSizeInput = Console.ReadLine();
            int batchSize = string.IsNullOrEmpty(batchSizeInput) ? 100 : int.Parse(batchSizeInput);
            
            // Process each reading
            foreach (var reading in readings)
            {
                // Only process water temperature readings
                if (reading.SensorType == "WaterTemperature")
                {
                    try
                    {
                        var dto = new TemperatureReadingDto
                        {
                            DeviceId = "Sensor1",
                            CreatedAt = reading.CapturedAt,
                            Temperature = reading.Reading,
                            SignalStrength = reading.SignalStrength
                        };
                        
                        var response = await httpClient.PostAsJsonAsync(apiBaseUrl, dto);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            importedCount++;
                            if (importedCount % batchSize == 0)
                            {
                                Console.WriteLine($"Imported {importedCount} readings so far...");
                                
                                // Give the API a small break after each batch
                                await Task.Delay(500);
                            }
                        }
                        else
                        {
                            errorCount++;
                            Console.WriteLine($"Error importing reading {reading.Id}: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        Console.WriteLine($"Exception importing reading {reading.Id}: {ex.Message}");
                    }
                }
                else
                {
                    skippedCount++;
                }
            }
        }
        
        Console.WriteLine($"Import completed. Imported {importedCount} readings with {errorCount} errors. Skipped {skippedCount} non-water temperature readings.");
    }


    private static async Task Export()
    {
        // TODO: Replace with your actual CosmosDB connection details
        string endpointUri = "";
        string primaryKey = "";
        string databaseId = "";
        string containerId = "";
        string outputFile = "./export";

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
            string fileName = $"{outputFile}/{kvp.Key}.json";
            string json = JsonConvert.SerializeObject(kvp.Value, Formatting.Indented);
            await File.WriteAllTextAsync(fileName, json);
            Console.WriteLine($"Exported {kvp.Value.Count} documents to {fileName}");
        }
    }
}
