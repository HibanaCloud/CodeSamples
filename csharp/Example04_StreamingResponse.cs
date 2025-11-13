/*
 * 04 - Streaming Response
 *
 * This example demonstrates real-time streaming of AI responses using
 * Server-Sent Events (SSE). Instead of waiting for the complete response,
 * tokens are received and displayed progressively as they're generated.
 *
 * Model used: gpt-5-nano (Fast OpenAI model)
 */

using System;
using System.ClientModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;

namespace Hibana.Samples
{
    public class Example04_StreamingResponse
    {
        private static readonly string ApiKey = "YOUR_API_KEY";
        private static readonly string BaseUrl = "https://api-ai.hibanacloud.com/v1";

        public static async Task Main(string[] args)
        {
            try
            {
                // Example 1: Basic streaming
                await StreamingChat();

                // Example 2: Streaming with metadata
                await StreamingWithMetadata();

                // Example 3: Compare streaming vs non-streaming
                await CompareStreamingVsNormal();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Demonstrate streaming responses with real-time output
        /// </summary>
        private static async Task StreamingChat()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gpt-5-nano");

            Console.WriteLine(new string('=', 60));
            Console.WriteLine("Streaming Response Demo");
            Console.WriteLine(new string('=', 60));

            string userMessage = "Write a short story about a robot learning to paint. Make it about 150 words.";

            Console.WriteLine($"\nUser: {userMessage}");
            Console.Write("\nAssistant (streaming): ");

            // Create streaming chat completion
            var streamingUpdates = chatClient.CompleteChatStreamingAsync(
                messages: new[]
                {
                    new UserChatMessage(userMessage)
                },
                options: new ChatCompletionOptions
                {
                    Temperature = 1.0f,
                    MaxOutputTokenCount = 10000
                }
            );

            // Collect the full response for later display
            var fullResponse = new StringBuilder();

            // Process the stream
            await foreach (var update in streamingUpdates)
            {
                foreach (var contentPart in update.ContentUpdate)
                {
                    string content = contentPart.Text;
                    fullResponse.Append(content);

                    // Print the content immediately (streaming effect)
                    Console.Write(content);
                }
            }

            Console.WriteLine("\n\n" + new string('=', 60));
            Console.WriteLine("Streaming complete!");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"\nTotal characters received: {fullResponse.Length}");
        }

        /// <summary>
        /// Streaming with detailed chunk inspection
        /// </summary>
        private static async Task StreamingWithMetadata()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gemini-2.5-flash-lite");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Streaming with Metadata Inspection");
            Console.WriteLine(new string('=', 60));

            string userMessage = "Explain quantum entanglement in 2 sentences.";

            Console.WriteLine($"\nUser: {userMessage}");
            Console.WriteLine("\nStreaming response...\n");

            var streamingUpdates = chatClient.CompleteChatStreamingAsync(
                messages: new[]
                {
                    new UserChatMessage(userMessage)
                },
                options: new ChatCompletionOptions
                {
                    Temperature = 0.7f,
                    MaxOutputTokenCount = 10000
                }
            );

            var fullText = new StringBuilder();
            int chunkCount = 0;
            string finishReason = null;

            await foreach (var update in streamingUpdates)
            {
                chunkCount++;

                // Check for content
                foreach (var contentPart in update.ContentUpdate)
                {
                    fullText.Append(contentPart.Text);
                    Console.Write(contentPart.Text);
                }

                // Check for finish reason
                if (update.FinishReason.HasValue)
                {
                    finishReason = update.FinishReason.ToString();
                }
            }

            if (finishReason != null)
            {
                Console.WriteLine($"\n\nFinish reason: {finishReason}");
            }

            Console.WriteLine($"\n\nTotal chunks received: {chunkCount}");
            Console.WriteLine($"Full response length: {fullText.Length} characters");
        }

        /// <summary>
        /// Compare streaming vs non-streaming response times
        /// </summary>
        private static async Task CompareStreamingVsNormal()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gpt-5-nano");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Comparison: Streaming vs Non-Streaming");
            Console.WriteLine(new string('=', 60));

            string message = "List 5 benefits of cloud computing.";

            // Non-streaming
            Console.WriteLine("\n1. NON-STREAMING:");
            var stopwatch = Stopwatch.StartNew();

            var response = await chatClient.CompleteChatAsync(
                messages: new[]
                {
                    new UserChatMessage(message)
                },
                options: new ChatCompletionOptions
                {
                    MaxOutputTokenCount = 10000
                }
            );

            stopwatch.Stop();
            string responseText = response.Value.Content[0].Text;

            Console.WriteLine($"   Time to first output: {stopwatch.Elapsed.TotalSeconds:F2} seconds");
            Console.WriteLine($"   Response: {responseText.Substring(0, Math.Min(100, responseText.Length))}...");

            // Streaming
            Console.WriteLine("\n2. STREAMING:");
            stopwatch.Restart();
            double? firstChunkTime = null;
            string firstChunk = null;

            var streamingUpdates = chatClient.CompleteChatStreamingAsync(
                messages: new[]
                {
                    new UserChatMessage(message)
                },
                options: new ChatCompletionOptions
                {
                    MaxOutputTokenCount = 10000
                }
            );

            await foreach (var update in streamingUpdates)
            {
                foreach (var contentPart in update.ContentUpdate)
                {
                    if (contentPart.Text != null && firstChunkTime == null)
                    {
                        firstChunkTime = stopwatch.Elapsed.TotalSeconds;
                        firstChunk = contentPart.Text;
                        Console.WriteLine($"   Time to first output: {firstChunkTime:F2} seconds");
                        Console.WriteLine($"   First chunk: {firstChunk}");
                        goto StreamingComplete; // Exit after first chunk
                    }
                }
            }

            StreamingComplete:
            Console.WriteLine("\n   Streaming provides faster time-to-first-token!");
        }
    }
}
