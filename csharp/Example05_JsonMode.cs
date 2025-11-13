/*
 * 05 - JSON Mode
 *
 * This example demonstrates how to get structured JSON responses from the AI.
 * JSON mode ensures the model outputs valid JSON that can be easily parsed
 * and used in your applications.
 *
 * Model used: gpt-5-nano (OpenAI model with JSON support)
 */

using System;
using System.ClientModel;
using System.Text.Json;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;

namespace Hibana.Samples
{
    public class Example05_JsonMode
    {
        private static readonly string ApiKey = "YOUR_API_KEY";
        private static readonly string BaseUrl = "https://api-ai.hibanacloud.com/v1";

        public static async Task Main(string[] args)
        {
            try
            {
                // Example 1: Basic JSON mode
                // await BasicJsonMode();

                // Example 2: Data extraction to JSON
                // await JsonForDataExtraction();

                // Example 3: JSON array response
                await JsonArrayResponse();

                // Example 4: Complex nested structure
                // await JsonComplexStructure();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Get a structured JSON response
        /// </summary>
        private static async Task BasicJsonMode()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gpt-5-nano");

            Console.WriteLine(new string('=', 60));
            Console.WriteLine("JSON Mode - Basic Example");
            Console.WriteLine(new string('=', 60));

            // Request must explicitly mention JSON in the prompt
            string userMessage = @"Create a JSON object for a book with the following fields:
    - title
    - author
    - year
    - genre
    - summary (short)

    Make up a fictional book.";

            Console.WriteLine($"\nUser: {userMessage.Substring(0, Math.Min(80, userMessage.Length))}...");
            Console.WriteLine("\nRequesting JSON response from gpt-5-nano...");

            var response = await chatClient.CompleteChatAsync(
                messages: new ChatMessage[]
                {
                    new SystemChatMessage("You are a helpful assistant that outputs data in JSON format."),
                    new UserChatMessage(userMessage)
                },
                options: new ChatCompletionOptions
                {
                    Temperature = 0.8f,
                    MaxOutputTokenCount = 10000,
                    ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()  // Enable JSON mode
                }
            );

            // Get the response
            string jsonResponse = response.Value.Content[0].Text;

            Console.WriteLine("\nRaw JSON Response:");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(jsonResponse);
            Console.WriteLine(new string('=', 60));

            // Parse and pretty-print the JSON
            try
            {
                using var doc = JsonDocument.Parse(jsonResponse);
                Console.WriteLine("\nParsed JSON (pretty-printed):");
                Console.WriteLine(JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true }));

                // Access specific fields
                Console.WriteLine("\nAccessing specific fields:");
                if (doc.RootElement.TryGetProperty("title", out var title))
                    Console.WriteLine($"  Title: {title}");
                if (doc.RootElement.TryGetProperty("author", out var author))
                    Console.WriteLine($"  Author: {author}");
                if (doc.RootElement.TryGetProperty("year", out var year))
                    Console.WriteLine($"  Year: {year}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
            }
        }

        /// <summary>
        /// Use JSON mode to extract structured data from text
        /// </summary>
        private static async Task JsonForDataExtraction()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gpt-5-nano");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("JSON Mode - Data Extraction");
            Console.WriteLine(new string('=', 60));

            string textToAnalyze = @"
    John Smith works as a Senior Software Engineer at TechCorp Inc.
    He can be reached at john.smith@techcorp.com or by phone at +1-555-0123.
    His office is located in San Francisco, California.
    ";

            string prompt = $@"Extract contact information from the following text and return it as JSON:

Text: {textToAnalyze}

Return JSON with these fields: name, job_title, company, email, phone, location";

            Console.WriteLine("\nExtracting structured data from text...");

            var response = await chatClient.CompleteChatAsync(
                messages: new ChatMessage[]
                {
                    new SystemChatMessage("You extract information from text and return it as JSON."),
                    new UserChatMessage(prompt)
                },
                options: new ChatCompletionOptions
                {
                    Temperature = 0.3f,  // Lower temperature for more consistent extraction
                    ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
                }
            );

            string jsonData = response.Value.Content[0].Text;
            using var doc = JsonDocument.Parse(jsonData);

            Console.WriteLine("\nExtracted Data:");
            Console.WriteLine(JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true }));
        }

        /// <summary>
        /// Get a JSON array as response
        /// </summary>
        private static async Task JsonArrayResponse()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gpt-5-nano");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("JSON Mode - Array Response");
            Console.WriteLine(new string('=', 60));

            string prompt = @"Generate a list of 5 programming languages with their primary use cases.
    Return as JSON array where each item has 'language' and 'use_case' fields.";

            Console.WriteLine("\nRequesting array of programming languages...");

            var response = await chatClient.CompleteChatAsync(
                messages: new[]
                {
                    new UserChatMessage(prompt)
                },
                options: new ChatCompletionOptions
                {
                    Temperature = 0.7f,
                    ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
                }
            );

            string jsonResult = response.Value.Content[0].Text;
            using var doc = JsonDocument.Parse(jsonResult);

            Console.WriteLine("\nProgramming Languages:");
            Console.WriteLine(JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true }));

            // Process the array
            if (doc.RootElement.TryGetProperty("languages", out var languages))
            {
                Console.WriteLine("\nFormatted output:");
                int index = 1;
                foreach (var lang in languages.EnumerateArray())
                {
                    string language = lang.TryGetProperty("language", out var l) ? l.GetString() : "N/A";
                    string useCase = lang.TryGetProperty("use_case", out var u) ? u.GetString() : "N/A";
                    Console.WriteLine($"{index}. {language}: {useCase}");
                    index++;
                }
            }
        }

        /// <summary>
        /// Create a complex nested JSON structure
        /// </summary>
        private static async Task JsonComplexStructure()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gpt-5-nano");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("JSON Mode - Complex Nested Structure");
            Console.WriteLine(new string('=', 60));

            string prompt = @"Create a JSON object representing a university course with:
    - course_id
    - course_name
    - instructor (object with name and email)
    - schedule (array of class sessions with day, time, room)
    - students (array of 3 students with name and student_id)
    - grading (object with assignments percentage, exams percentage, participation percentage)
    ";

            Console.WriteLine("\nGenerating complex JSON structure...");

            var response = await chatClient.CompleteChatAsync(
                messages: new ChatMessage[]
                {
                    new SystemChatMessage("Generate realistic but fictional data in JSON format."),
                    new UserChatMessage(prompt)
                },
                options: new ChatCompletionOptions
                {
                    Temperature = 0.8f,
                    MaxOutputTokenCount = 10000,
                    ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
                }
            );

            string complexJson = response.Value.Content[0].Text;
            using var doc = JsonDocument.Parse(complexJson);

            Console.WriteLine("\nComplex JSON Structure:");
            Console.WriteLine(JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
