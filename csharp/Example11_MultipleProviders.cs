/*
 * 11 - Multiple Providers Comparison
 *
 * This example demonstrates how to use all four LLM providers available
 * through Hibana API: OpenAI, Anthropic, DeepSeek, and Google Gemini.
 * Compare responses, performance, and features across providers.
 */

using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;

namespace Hibana.Samples
{
    public class Example11_MultipleProviders
    {
        private static readonly string ApiKey = "YOUR_API_KEY";
        private static readonly string BaseUrl = "https://api-ai.hibanacloud.com/v1";

        // Define models for each provider
        private static readonly Dictionary<string, string> Models = new()
        {
            ["OpenAI"] = "gpt-5-nano",
            ["Anthropic"] = "claude-haiku-4-5",
            ["DeepSeek"] = "deepseek-chat",
            ["Google"] = "gemini-2.5-flash-lite"
        };

        public static async Task Main(string[] args)
        {
            try
            {
                // Example 1: Basic comparison
                await CompareBasicResponses();

                // Example 2: Coding tasks
                // await CompareCodingTasks();  // Uncomment to run

                // Example 3: Creative writing
                // await CompareCreativeWriting();  // Uncomment to run

                // Example 4: Multilingual
                // await CompareMultilingual();  // Uncomment to run

                // Example 5: Performance benchmark
                await BenchmarkPerformance();

                // Example 6: Provider-specific features
                await ProviderSpecificFeatures();

                // Example 7: Model selection guide
                ChooseBestModelForTask();

                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine("Multiple provider comparison completed!");
                Console.WriteLine(new string('=', 60));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Compare responses from all four providers
        /// </summary>
        private static async Task CompareBasicResponses()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("Comparing All Providers");
            Console.WriteLine(new string('=', 60));

            string question = "What is the future of artificial intelligence? Answer in 2 sentences.";

            Console.WriteLine($"\nQuestion: {question}\n");

            foreach (var kvp in Models)
            {
                string provider = kvp.Key;
                string model = kvp.Value;

                Console.WriteLine($"\n{provider} ({model}):");
                Console.WriteLine(new string('-', 60));

                try
                {
                    var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
                    {
                        Endpoint = new Uri(BaseUrl)
                    });

                    var chatClient = client.GetChatClient(model);
                    var stopwatch = Stopwatch.StartNew();

                    var response = await chatClient.CompleteChatAsync(
                        messages: new[] { new UserChatMessage(question) },
                        options: new ChatCompletionOptions
                        {
                            Temperature = 0.7f,
                            MaxOutputTokenCount = 8000
                        }
                    );

                    stopwatch.Stop();

                    string answer = response.Value.Content[0].Text;
                    int tokens = response.Value.Usage?.TotalTokenCount ?? 0;

                    Console.WriteLine($"Response: {answer}");
                    Console.WriteLine($"\nTime: {stopwatch.Elapsed.TotalSeconds:F2}s | Tokens: {tokens}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Compare coding assistance from different providers
        /// </summary>
        private static async Task CompareCodingTasks()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Coding Task Comparison");
            Console.WriteLine(new string('=', 60));

            string codingQuestion = "Write a Python function to check if a string is a palindrome. Include comments.";

            Console.WriteLine($"\nTask: {codingQuestion}\n");

            foreach (var kvp in Models)
            {
                string provider = kvp.Key;
                string model = kvp.Value;

                Console.WriteLine($"\n{provider} ({model}):");
                Console.WriteLine(new string('-', 60));

                try
                {
                    var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
                    {
                        Endpoint = new Uri(BaseUrl)
                    });

                    var chatClient = client.GetChatClient(model);

                    var response = await chatClient.CompleteChatAsync(
                        messages: new ChatMessage[]
                        {
                            new SystemChatMessage("You are a Python programming expert."),
                            new UserChatMessage(codingQuestion)
                        },
                        options: new ChatCompletionOptions
                        {
                            Temperature = 0.3f,  // Lower temp for coding
                            MaxOutputTokenCount = 8000
                        }
                    );

                    Console.WriteLine(response.Value.Content[0].Text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Compare creative writing capabilities
        /// </summary>
        private static async Task CompareCreativeWriting()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Creative Writing Comparison");
            Console.WriteLine(new string('=', 60));

            string prompt = "Write a two-line poem about technology and humanity.";

            Console.WriteLine($"\nPrompt: {prompt}\n");

            foreach (var kvp in Models)
            {
                string provider = kvp.Key;
                string model = kvp.Value;

                Console.WriteLine($"\n{provider} ({model}):");
                Console.WriteLine(new string('-', 60));

                try
                {
                    var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
                    {
                        Endpoint = new Uri(BaseUrl)
                    });

                    var chatClient = client.GetChatClient(model);

                    var response = await chatClient.CompleteChatAsync(
                        messages: new[] { new UserChatMessage(prompt) },
                        options: new ChatCompletionOptions
                        {
                            Temperature = 1.0f,  // Higher temp for creativity
                            MaxOutputTokenCount = 8000
                        }
                    );

                    Console.WriteLine(response.Value.Content[0].Text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Compare multilingual capabilities (Persian)
        /// </summary>
        private static async Task CompareMultilingual()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Multilingual Capability (Persian)");
            Console.WriteLine(new string('=', 60));

            string persianQuestion = "چرا آموزش هوش مصنوعی مهم است؟";

            Console.WriteLine($"\nQuestion (Persian): {persianQuestion}\n");

            foreach (var kvp in Models)
            {
                string provider = kvp.Key;
                string model = kvp.Value;

                Console.WriteLine($"\n{provider} ({model}):");
                Console.WriteLine(new string('-', 60));

                try
                {
                    var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
                    {
                        Endpoint = new Uri(BaseUrl)
                    });

                    var chatClient = client.GetChatClient(model);

                    var response = await chatClient.CompleteChatAsync(
                        messages: new[] { new UserChatMessage(persianQuestion) },
                        options: new ChatCompletionOptions
                        {
                            Temperature = 0.7f,
                            MaxOutputTokenCount = 8000
                        }
                    );

                    Console.WriteLine(response.Value.Content[0].Text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Benchmark response time and token usage
        /// </summary>
        private static async Task BenchmarkPerformance()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Performance Benchmark");
            Console.WriteLine(new string('=', 60));

            string testMessage = "Explain quantum computing in one sentence.";

            var results = new List<Dictionary<string, object>>();

            Console.WriteLine($"\nTest message: {testMessage}\n");
            Console.WriteLine("Running benchmark...\n");

            foreach (var kvp in Models)
            {
                string provider = kvp.Key;
                string model = kvp.Value;

                try
                {
                    var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
                    {
                        Endpoint = new Uri(BaseUrl)
                    });

                    var chatClient = client.GetChatClient(model);

                    // Warm-up request (not counted)
                    await chatClient.CompleteChatAsync(
                        messages: new[] { new UserChatMessage("Hi") },
                        options: new ChatCompletionOptions { MaxOutputTokenCount = 8000 }
                    );

                    // Actual benchmark
                    var stopwatch = Stopwatch.StartNew();

                    var response = await chatClient.CompleteChatAsync(
                        messages: new[] { new UserChatMessage(testMessage) },
                        options: new ChatCompletionOptions
                        {
                            Temperature = 0.5f,
                            MaxOutputTokenCount = 8000
                        }
                    );

                    stopwatch.Stop();

                    results.Add(new Dictionary<string, object>
                    {
                        ["provider"] = provider,
                        ["model"] = model,
                        ["time"] = stopwatch.Elapsed.TotalSeconds,
                        ["tokens"] = response.Value.Usage?.TotalTokenCount ?? 0,
                        ["response_length"] = response.Value.Content[0].Text.Length
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{provider}: Error - {ex.Message}");
                }
            }

            // Display results
            Console.WriteLine("\nBenchmark Results:");
            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"{"Provider",-15} {"Model",-25} {"Time (s)",-12} {"Tokens",-10} {"Length",-10}");
            Console.WriteLine(new string('-', 80));

            foreach (var result in results.OrderBy(r => (double)r["time"]))
            {
                Console.WriteLine($"{result["provider"],-15} {result["model"],-25} " +
                                  $"{(double)result["time"],-12:F3} {result["tokens"],-10} {result["response_length"],-10}");
            }

            Console.WriteLine(new string('-', 80));

            // Find fastest
            if (results.Any())
            {
                var fastest = results.OrderBy(r => (double)r["time"]).First();
                Console.WriteLine($"\nFastest: {fastest["provider"]} ({(double)fastest["time"]:F3}s)");
            }
        }

        /// <summary>
        /// Demonstrate provider-specific features
        /// </summary>
        private static async Task ProviderSpecificFeatures()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Provider-Specific Features");
            Console.WriteLine(new string('=', 60));

            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            // OpenAI: JSON mode
            Console.WriteLine("\n1. OpenAI - JSON Mode:");
            Console.WriteLine(new string('-', 60));
            try
            {
                var chatClient = client.GetChatClient("gpt-5-nano");
                var response = await chatClient.CompleteChatAsync(
                    messages: new ChatMessage[]
                    {
                        new SystemChatMessage("Return responses as JSON."),
                        new UserChatMessage("Create a JSON object with name and age for a fictional person.")
                    },
                    options: new ChatCompletionOptions
                    {
                        ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
                    }
                );
                Console.WriteLine(response.Value.Content[0].Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Anthropic: Long context
            Console.WriteLine("\n\n2. Anthropic Claude - Long Context:");
            Console.WriteLine(new string('-', 60));
            try
            {
                var chatClient = client.GetChatClient("claude-haiku-4-5");
                string longText = string.Concat(Enumerable.Repeat("AI ", 500));  // Simulate longer context
                var response = await chatClient.CompleteChatAsync(
                    messages: new[] { new UserChatMessage($"Count how many times 'AI' appears in this text: {longText}") },
                    options: new ChatCompletionOptions { MaxOutputTokenCount = 8000 }
                );
                Console.WriteLine(response.Value.Content[0].Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // DeepSeek: Cost-effective
            Console.WriteLine("\n\n3. DeepSeek - Cost-Effective Model:");
            Console.WriteLine(new string('-', 60));
            try
            {
                var chatClient = client.GetChatClient("deepseek-chat");
                var response = await chatClient.CompleteChatAsync(
                    messages: new[] { new UserChatMessage("What makes DeepSeek models cost-effective?") },
                    options: new ChatCompletionOptions { MaxOutputTokenCount = 8000 }
                );
                Console.WriteLine(response.Value.Content[0].Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Gemini: Fast & Efficient
            Console.WriteLine("\n\n4. Google Gemini - Fast & Efficient:");
            Console.WriteLine(new string('-', 60));
            try
            {
                var chatClient = client.GetChatClient("gemini-2.5-flash-lite");
                var response = await chatClient.CompleteChatAsync(
                    messages: new[] { new UserChatMessage("List 3 advantages of Gemini models in one sentence each.") },
                    options: new ChatCompletionOptions { MaxOutputTokenCount = 8000 }
                );
                Console.WriteLine(response.Value.Content[0].Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Recommendation system for choosing models
        /// </summary>
        private static void ChooseBestModelForTask()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Model Selection Guide");
            Console.WriteLine(new string('=', 60));

            var recommendations = new Dictionary<string, (string Model, string Reason)>
            {
                ["Speed & Cost"] = ("gpt-5-nano or deepseek-chat", "Fast responses and low cost"),
                ["Coding"] = ("deepseek-chat or gpt-5-nano", "Strong programming capabilities"),
                ["Creative Writing"] = ("claude-haiku-4-5 or gpt-5-nano", "Excellent creative output"),
                ["Long Context"] = ("claude-haiku-4-5", "Large context window"),
                ["Persian/Multilingual"] = ("gpt-5-nano or gemini-2.5-flash-lite", "Good multilingual support"),
                ["JSON Output"] = ("gpt-5-nano", "Native JSON mode support")
            };

            Console.WriteLine("\nTask-Based Model Recommendations:\n");

            foreach (var kvp in recommendations)
            {
                Console.WriteLine($"{kvp.Key}:");
                Console.WriteLine($"  → {kvp.Value.Model}");
                Console.WriteLine($"  Reason: {kvp.Value.Reason}");
                Console.WriteLine();
            }
        }
    }
}
