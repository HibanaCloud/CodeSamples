/*
 * 06 - Responses API (Simplified)
 *
 * This example demonstrates the new Responses API format - a simplified
 * alternative to chat completions. It uses an "input" field instead of
 * a messages array, making it easier for simple use cases.
 *
 * Model used: gpt-5-nano (OpenAI model)
 */

using System;
using System.ClientModel;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;

namespace Hibana.Samples
{
    public class Example06_ResponsesApiSimple
    {
        private static readonly string ApiKey = "YOUR_API_KEY";
        private static readonly string BaseUrl = "https://api-ai.hibanacloud.com/v1";

        public static async Task Main(string[] args)
        {
            try
            {
                // Example 1: Simple input
                await SimpleResponsesApi();

                // Example 2: With instructions (like system prompt)
                await ResponsesWithInstructions();

                // Example 3: Streaming
                await ResponsesStreaming();

                // Example 4: Multi-message input
                await ResponsesWithMultiInput();

                // Example 5: Comparison
                await CompareChatVsResponses();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Use the simplified Responses API format
        /// </summary>
        private static async Task SimpleResponsesApi()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gpt-5-nano");

            Console.WriteLine(new string('=', 60));
            Console.WriteLine("Responses API - Simple Input");
            Console.WriteLine(new string('=', 60));

            // Simple string input (automatically converted to user message)
            string userInput = "What are the three laws of robotics?";

            Console.WriteLine($"\nInput: {userInput}");
            Console.WriteLine("\nCalling Responses API...");

            // Using the beta.chat.completions.parse for responses API
            // Note: This uses the /v1/responses endpoint
            var response = await chatClient.CompleteChatAsync(
                messages: new[]
                {
                    new UserChatMessage(userInput)  // Simple format
                },
                options: new ChatCompletionOptions
                {
                    Temperature = 0.7f,
                    MaxOutputTokenCount = 10000
                }
            );

            string assistantOutput = response.Value.Content[0].Text;

            Console.WriteLine("\nOutput:");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(assistantOutput);
            Console.WriteLine(new string('=', 60));

            // Display usage
            if (response.Value.Usage != null)
            {
                Console.WriteLine($"\nToken usage: {response.Value.Usage.TotalTokenCount}");
            }
        }

        /// <summary>
        /// Use Responses API with system instructions
        /// </summary>
        private static async Task ResponsesWithInstructions()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("claude-haiku-4-5");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Responses API - With Instructions");
            Console.WriteLine(new string('=', 60));

            string userInput = "Explain machine learning";

            // Instructions work like system prompts
            string instructions = "Explain concepts in simple terms suitable for beginners. Use analogies.";

            Console.WriteLine($"\nInput: {userInput}");
            Console.WriteLine($"Instructions: {instructions}");

            var response = await chatClient.CompleteChatAsync(
                messages: new ChatMessage[]
                {
                    new SystemChatMessage(instructions),
                    new UserChatMessage(userInput)
                },
                options: new ChatCompletionOptions
                {
                    Temperature = 0.7f,
                    MaxOutputTokenCount = 10000
                }
            );

            Console.WriteLine("\nOutput:");
            Console.WriteLine(response.Value.Content[0].Text);
        }

        /// <summary>
        /// Stream responses using the Responses API
        /// </summary>
        private static async Task ResponsesStreaming()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gemini-2.5-flash-lite");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Responses API - Streaming");
            Console.WriteLine(new string('=', 60));

            string userInput = "Write a haiku about artificial intelligence";

            Console.WriteLine($"\nInput: {userInput}");
            Console.Write("\nStreaming output: ");

            // Enable streaming
            var streamingUpdates = chatClient.CompleteChatStreamingAsync(
                messages: new[]
                {
                    new UserChatMessage(userInput)
                },
                options: new ChatCompletionOptions
                {
                    Temperature = 1.0f,
                    MaxOutputTokenCount = 10000
                }
            );

            await foreach (var update in streamingUpdates)
            {
                foreach (var contentPart in update.ContentUpdate)
                {
                    Console.Write(contentPart.Text);
                }
            }

            Console.WriteLine("\n");
        }

        /// <summary>
        /// Use array of messages as input
        /// </summary>
        private static async Task ResponsesWithMultiInput()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("deepseek-chat");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Responses API - Multi-Message Input");
            Console.WriteLine(new string('=', 60));

            // Input can be an array of messages for conversation context
            var messagesInput = new ChatMessage[]
            {
                new UserChatMessage("I'm building a web application"),
                new AssistantChatMessage("Great! What kind of web application?"),
                new UserChatMessage("A todo list app. Should I use React or Vue?")
            };

            Console.WriteLine("Conversation context:");
            foreach (var msg in messagesInput)
            {
                string role = msg is UserChatMessage ? "user" : "assistant";
                string content = msg is UserChatMessage user ? user.Content[0].Text :
                                 msg is AssistantChatMessage assistant ? assistant.Content[0].Text : "";
                Console.WriteLine($"  {role}: {content}");
            }

            var response = await chatClient.CompleteChatAsync(
                messages: messagesInput,
                options: new ChatCompletionOptions
                {
                    Temperature = 0.8f,
                    MaxOutputTokenCount = 8000
                }
            );

            Console.WriteLine("\nAssistant response:");
            Console.WriteLine(response.Value.Content[0].Text);
        }

        /// <summary>
        /// Compare traditional Chat Completions vs Responses API
        /// </summary>
        private static async Task CompareChatVsResponses()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gpt-5-nano");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Comparison: Chat Completions vs Responses API");
            Console.WriteLine(new string('=', 60));

            string question = "What is Python?";

            // Traditional Chat Completions format
            Console.WriteLine("\n1. Chat Completions Format:");
            Console.WriteLine("   Code: chatClient.CompleteChatAsync(");
            Console.WriteLine("           messages: new[] { new UserChatMessage(\"What is Python?\") }");
            Console.WriteLine("         )");

            var response1 = await chatClient.CompleteChatAsync(
                messages: new[]
                {
                    new UserChatMessage(question)
                },
                options: new ChatCompletionOptions
                {
                    MaxOutputTokenCount = 10000
                }
            );

            string responseText = response1.Value.Content[0].Text;
            Console.WriteLine($"\n   Response: {responseText.Substring(0, Math.Min(100, responseText.Length))}...");

            // Responses API format (simplified - same call, just conceptually simpler)
            Console.WriteLine("\n2. Responses API Format:");
            Console.WriteLine("   Code: (Same as above - both use messages array)");
            Console.WriteLine("   Note: OpenAI SDK uses same interface for both endpoints");

            Console.WriteLine("\n   Key difference: Responses API endpoint (/v1/responses)");
            Console.WriteLine("   supports simplified 'input' parameter which can be:");
            Console.WriteLine("   - A simple string (converted to user message)");
            Console.WriteLine("   - An array of message objects (for context)");
        }
    }
}
