/*
 * 02 - Chat with System Prompt
 *
 * This example demonstrates how to use system prompts to control
 * the AI's behavior, tone, and expertise. System prompts set the
 * context and guidelines for how the assistant should respond.
 *
 * Model used: claude-haiku-4-5 (Claude Haiku - Fast Anthropic model)
 */

using System;
using System.ClientModel;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;

namespace Hibana.Samples
{
    public class Example02_ChatWithSystemPrompt
    {
        private static readonly string ApiKey = "YOUR_API_KEY";
        private static readonly string BaseUrl = "https://api-ai.hibanacloud.com/v1";

        public static async Task Main(string[] args)
        {
            try
            {
                // Example 1: System prompt for English responses
                await ChatWithSystemPrompt();

                // Example 2: Comparison with/without system prompt
                await CompareWithWithoutSystem();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Demonstrate using system prompts to control AI behavior
        /// </summary>
        private static async Task ChatWithSystemPrompt()
        {
            // Initialize the Hibana client
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("claude-haiku-4-5");

            // Define a custom system prompt
            string systemPrompt = @"You are a English-speaking AI assistant specializing in
explaining technical concepts in simple terms. Always respond in English
and use analogies from everyday life to explain complex topics. Be friendly and
encouraging.";

            string userQuestion = "What is machine learning and how does it work?";

            Console.WriteLine("Sending message to claude-haiku-4-5 with custom system prompt...");
            Console.WriteLine($"\nSystem Prompt: {systemPrompt.Substring(0, Math.Min(100, systemPrompt.Length))}...");
            Console.WriteLine($"User Question: {userQuestion}");

            // Create chat completion with system prompt
            var completion = await chatClient.CompleteChatAsync(
                messages: new ChatMessage[]
                {
                    new SystemChatMessage(systemPrompt),  // System message defines AI behavior
                    new UserChatMessage(userQuestion)
                },
                options: new ChatCompletionOptions
                {
                    Temperature = 0.7f,  // Slightly lower for more consistent responses
                    MaxOutputTokenCount = 10000
                }
            );

            // Extract and display response
            var assistantMessage = completion.Value.Content[0].Text;

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Response from claude-haiku-4-5:");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(assistantMessage);
            Console.WriteLine(new string('=', 60));

            // Display usage statistics
            if (completion.Value.Usage != null)
            {
                Console.WriteLine("\nToken Usage:");
                Console.WriteLine($"  Total tokens: {completion.Value.Usage.TotalTokenCount}");
            }
        }

        /// <summary>
        /// Compare responses with and without system prompt
        /// </summary>
        private static async Task CompareWithWithoutSystem()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gpt-5-nano");

            string userMessage = "Explain quantum computing";

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Comparison: With vs Without System Prompt");
            Console.WriteLine(new string('=', 60));

            // Response WITHOUT system prompt
            Console.WriteLine("\n1. WITHOUT system prompt:");
            var response1 = await chatClient.CompleteChatAsync(
                messages: new[]
                {
                    new UserChatMessage(userMessage)
                },
                options: new ChatCompletionOptions
                {
                    MaxOutputTokenCount = 10000
                }
            );
            Console.WriteLine(response1.Value.Content[0].Text);

            // Response WITH system prompt
            Console.WriteLine("\n2. WITH system prompt (explain like I'm 10 years old):");
            var response2 = await chatClient.CompleteChatAsync(
                messages: new ChatMessage[]
                {
                    new SystemChatMessage("Explain everything as if talking to a 10-year-old child. Use simple words and fun examples."),
                    new UserChatMessage(userMessage)
                },
                options: new ChatCompletionOptions
                {
                    MaxOutputTokenCount = 10000
                }
            );
            Console.WriteLine(response2.Value.Content[0].Text);
        }
    }
}
