/*
 * 08 - List Available Models
 *
 * This example demonstrates how to retrieve all available models
 * from the Hibana API. This is useful for discovering what models
 * are enabled for your account.
 *
 * Endpoint: GET /v1/models
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hibana.Samples
{
    public class Example08_ListModels
    {
        private static readonly string ApiKey = "YOUR_API_KEY";
        private static readonly string BaseUrl = "https://api-ai.hibanacloud.com/v1";

        // Model info class for deserialization
        public class ModelInfo
        {
            public string id { get; set; } = "";
            public string @object { get; set; } = "";
            public long created { get; set; }
            public string owned_by { get; set; } = "";
        }

        public class ModelsResponse
        {
            public string @object { get; set; } = "";
            public List<ModelInfo> data { get; set; } = new();
        }

        public static async Task Main(string[] args)
        {
            try
            {
                // Example 1: List all models
                await ListAllModels();

                // Example 2: Group by provider
                await GroupModelsByProvider();

                // Example 3: Filter by type
                await FilterModelsByType();

                // Example 4: Get specific model info
                await GetSpecificModelInfo();

                // Example 5: Check availability
                await CheckModelAvailability();

                // Example 6: Display as table
                await DisplayModelsTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Helper method to fetch models from API
        /// </summary>
        private static async Task<List<ModelInfo>> FetchModels()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", ApiKey);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string url = $"{BaseUrl}/models";
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API returned {response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var modelsResponse = JsonSerializer.Deserialize<ModelsResponse>(jsonResponse);
            return modelsResponse?.data ?? new List<ModelInfo>();
        }

        /// <summary>
        /// List all available models
        /// </summary>
        private static async Task ListAllModels()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("Listing All Available Models");
            Console.WriteLine(new string('=', 60));

            var modelList = await FetchModels();

            Console.WriteLine($"\nTotal models available: {modelList.Count}\n");

            // Display each model
            for (int i = 0; i < modelList.Count; i++)
            {
                var model = modelList[i];
                Console.WriteLine($"{i + 1}. {model.id}");
                if (!string.IsNullOrEmpty(model.owned_by))
                    Console.WriteLine($"   Owner: {model.owned_by}");
                if (model.created > 0)
                {
                    var createdDate = DateTimeOffset.FromUnixTimeSeconds(model.created);
                    Console.WriteLine($"   Created: {createdDate:yyyy-MM-dd}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Group models by their provider (OpenAI, Anthropic, etc.)
        /// </summary>
        private static async Task GroupModelsByProvider()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("Models Grouped by Provider");
            Console.WriteLine(new string('=', 60));

            var models = await FetchModels();

            // Group models by owner/provider
            var grouped = new Dictionary<string, List<string>>();
            foreach (var model in models)
            {
                string provider = model.owned_by ?? "unknown";
                if (!grouped.ContainsKey(provider))
                {
                    grouped[provider] = new List<string>();
                }
                grouped[provider].Add(model.id);
            }

            // Display grouped models
            foreach (var kvp in grouped)
            {
                Console.WriteLine($"\n{kvp.Key.ToUpper()}");
                Console.WriteLine(new string('-', 40));
                foreach (var modelId in kvp.Value)
                {
                    Console.WriteLine($"  {modelId}");
                }
            }

            Console.WriteLine($"\nTotal providers: {grouped.Count}");
        }

        /// <summary>
        /// Filter and categorize models by their type
        /// </summary>
        private static async Task FilterModelsByType()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Models by Type");
            Console.WriteLine(new string('=', 60));

            var models = await FetchModels();

            // Categorize models
            var chatModels = new List<string>();
            var imageModels = new List<string>();
            var otherModels = new List<string>();

            foreach (var model in models)
            {
                string modelId = model.id.ToLower();

                if (modelId.Contains("dall-e") || modelId.Contains("dalle"))
                {
                    imageModels.Add(model.id);
                }
                else if (new[] { "gpt", "claude", "deepseek", "gemini" }.Any(x => modelId.Contains(x)))
                {
                    chatModels.Add(model.id);
                }
                else
                {
                    otherModels.Add(model.id);
                }
            }

            // Display categorized models
            Console.WriteLine("\nCHAT/TEXT MODELS:");
            Console.WriteLine(new string('-', 40));
            foreach (var model in chatModels)
            {
                Console.WriteLine($"   {model}");
            }

            Console.WriteLine("\n\nIMAGE GENERATION MODELS:");
            Console.WriteLine(new string('-', 40));
            foreach (var model in imageModels)
            {
                Console.WriteLine($"   {model}");
            }

            if (otherModels.Any())
            {
                Console.WriteLine("\n\nOTHER MODELS:");
                Console.WriteLine(new string('-', 40));
                foreach (var model in otherModels)
                {
                    Console.WriteLine($"   {model}");
                }
            }
        }

        /// <summary>
        /// Get information about a specific model
        /// </summary>
        private static async Task GetSpecificModelInfo()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Specific Model Information");
            Console.WriteLine(new string('=', 60));

            string modelId = "gpt-5-nano";

            Console.WriteLine($"\nQuerying model: {modelId}");

            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", ApiKey);
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string url = $"{BaseUrl}/models/{modelId}";
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var model = JsonSerializer.Deserialize<ModelInfo>(jsonResponse);

                    if (model != null)
                    {
                        Console.WriteLine("\nModel Details:");
                        Console.WriteLine(new string('-', 40));
                        Console.WriteLine($"ID: {model.id}");
                        if (!string.IsNullOrEmpty(model.owned_by))
                            Console.WriteLine($"Owned by: {model.owned_by}");

                        if (model.created > 0)
                        {
                            var createdDate = DateTimeOffset.FromUnixTimeSeconds(model.created);
                            Console.WriteLine($"Created: {createdDate:yyyy-MM-dd HH:mm:ss}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Error retrieving model: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving model: {ex.Message}");
            }
        }

        /// <summary>
        /// Check if specific models are available
        /// </summary>
        private static async Task CheckModelAvailability()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Model Availability Check");
            Console.WriteLine(new string('=', 60));

            // Models to check
            var modelsToCheck = new[]
            {
                "gpt-5-nano",
                "claude-haiku-4-5",
                "deepseek-chat",
                "gemini-2.5-flash-lite",
                "dall-e-3"
            };

            Console.WriteLine("\nChecking availability...\n");

            // Get all available models
            var availableModels = await FetchModels();
            var availableIds = availableModels.Select(m => m.id).ToList();

            // Check each model
            foreach (var modelId in modelsToCheck)
            {
                bool isAvailable = availableIds.Contains(modelId);
                string status = isAvailable ? "✓ Available" : "✗ Not Available";
                Console.WriteLine($"{status.PadRight(20)} {modelId}");
            }
        }

        /// <summary>
        /// Display models in a formatted table
        /// </summary>
        private static async Task DisplayModelsTable()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Models Table");
            Console.WriteLine(new string('=', 60));

            var models = await FetchModels();

            // Table header
            Console.WriteLine($"\n{"Model ID",-40} {"Provider",-15} {"Type",-10}");
            Console.WriteLine(new string('-', 70));

            // Table rows
            foreach (var model in models)
            {
                string modelId = model.id;
                string provider = model.owned_by ?? "unknown";

                // Determine type
                string modelType;
                if (modelId.ToLower().Contains("dall-e"))
                {
                    modelType = "Image";
                }
                else if (new[] { "gpt", "claude", "deepseek", "gemini" }.Any(x => modelId.ToLower().Contains(x)))
                {
                    modelType = "Chat";
                }
                else
                {
                    modelType = "Other";
                }

                Console.WriteLine($"{modelId,-40} {provider,-15} {modelType,-10}");
            }

            Console.WriteLine();
        }
    }
}
