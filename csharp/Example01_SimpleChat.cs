/*
 * 01 - Simple Chat Completion
 *
 * This example demonstrates the most basic usage of Hibana API
 * using the OpenAI .NET SDK. It shows how to:
 * - Initialize the client with Hibana's base URL
 * - Send a simple chat completion request
 * - Receive and display the response
 *
 * Model used: gpt-5-nano (Fast and efficient GPT model)
 */

using System;
using System.ClientModel;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;

namespace Hibana.Samples
{
    public class Example01_SimpleChat
    {
        // Replace "YOUR_API_KEY" with your actual Hibana API key
        private static readonly string ApiKey = "YOUR_API_KEY";
        private static readonly string BaseUrl = "https://api-ai.hibanacloud.com/v1";

        public static async Task Main(string[] args)
        {
            try
            {
                await SimpleChat();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Send a simple chat message and get a response
        /// </summary>
        private static async Task SimpleChat()
        {
            Console.WriteLine("Sending message to gpt-5-nano...");

            // Initialize the Hibana client
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gpt-5-nano");

            // Create a chat completion
            var completion = await chatClient.CompleteChatAsync(
                messages: new[]
                {
                    new UserChatMessage("Hello! Please introduce yourself in one sentence.")
                },
                options: new ChatCompletionOptions
                {
                    Temperature = 1.0f,        // Controls randomness (0.0 = deterministic, 2.0 = very random)
                    MaxOutputTokenCount = 10000  // Maximum tokens in the response
                }
            );

            // Extract the response
            var assistantMessage = completion.Value.Content[0].Text;
            var finishReason = completion.Value.FinishReason;

            // Display the response
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Response from gpt-5-nano:");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(assistantMessage);
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"\nFinish reason: {finishReason}");

            // Display token usage
            if (completion.Value.Usage != null)
            {
                Console.WriteLine("\nToken Usage:");
                Console.WriteLine($"  Prompt tokens: {completion.Value.Usage.InputTokenCount}");
                Console.WriteLine($"  Completion tokens: {completion.Value.Usage.OutputTokenCount}");
                Console.WriteLine($"  Total tokens: {completion.Value.Usage.TotalTokenCount}");
            }
        }
    }
}
