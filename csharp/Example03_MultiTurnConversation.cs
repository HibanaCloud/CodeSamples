/*
 * 03 - Multi-Turn Conversation
 *
 * This example demonstrates how to maintain conversation context
 * across multiple messages. The AI can reference previous messages
 * and maintain a coherent conversation flow.
 *
 * Model used: deepseek-chat (DeepSeek conversational model)
 */

using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;

namespace Hibana.Samples
{
    public class Example03_MultiTurnConversation
    {
        private static readonly string ApiKey = "YOUR_API_KEY";
        private static readonly string BaseUrl = "https://api-ai.hibanacloud.com/v1";

        public static async Task Main(string[] args)
        {
            try
            {
                // Run multi-turn conversation example
                await MultiTurnConversation();

                // Show interactive conversation pattern
                InteractiveConversation();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Demonstrate a multi-turn conversation with context
        /// </summary>
        private static async Task MultiTurnConversation()
        {
            // Initialize the Hibana client
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("deepseek-chat");

            // Conversation history - this maintains context across turns
            var conversationHistory = new List<ChatMessage>
            {
                new SystemChatMessage("You are a helpful programming tutor specializing in Python."),
                new UserChatMessage("Hi! I'm learning Python. Can you help me?")
            };

            Console.WriteLine(new string('=', 60));
            Console.WriteLine("Multi-Turn Conversation with deepseek-chat");
            Console.WriteLine(new string('=', 60));

            // First exchange
            Console.WriteLine("\nUser: Hi! I'm learning Python. Can you help me?");

            var response1 = await chatClient.CompleteChatAsync(
                messages: conversationHistory,
                options: new ChatCompletionOptions
                {
                    Temperature = 0.8f,
                    MaxOutputTokenCount = 8192
                }
            );

            string assistantReply1 = response1.Value.Content[0].Text;
            Console.WriteLine($"\nAssistant: {assistantReply1}");

            // Add assistant's response to conversation history
            conversationHistory.Add(new AssistantChatMessage(assistantReply1));

            // Second turn - ask a follow-up question
            string userMessage2 = "What's the difference between a list and a tuple?";
            conversationHistory.Add(new UserChatMessage(userMessage2));

            Console.WriteLine($"\nUser: {userMessage2}");

            var response2 = await chatClient.CompleteChatAsync(
                messages: conversationHistory,
                options: new ChatCompletionOptions
                {
                    Temperature = 0.8f,
                    MaxOutputTokenCount = 8192
                }
            );

            string assistantReply2 = response2.Value.Content[0].Text;
            Console.WriteLine($"\nAssistant: {assistantReply2}");

            // Add to history for potential future turns
            conversationHistory.Add(new AssistantChatMessage(assistantReply2));

            // Third turn - reference previous context
            string userMessage3 = "Can you show me an example of each?";
            conversationHistory.Add(new UserChatMessage(userMessage3));

            Console.WriteLine($"\nUser: {userMessage3}");

            var response3 = await chatClient.CompleteChatAsync(
                messages: conversationHistory,
                options: new ChatCompletionOptions
                {
                    Temperature = 0.8f,
                    MaxOutputTokenCount = 8192
                }
            );

            string assistantReply3 = response3.Value.Content[0].Text;
            Console.WriteLine($"\nAssistant: {assistantReply3}");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine($"Total messages in conversation: {conversationHistory.Count + 1}");
            Console.WriteLine(new string('=', 60));

            // Display total usage
            if (response3.Value.Usage != null)
            {
                Console.WriteLine("\nLast request token usage:");
                Console.WriteLine($"  Total tokens: {response3.Value.Usage.TotalTokenCount}");
            }
        }

        /// <summary>
        /// Interactive conversation loop (demonstration - not running)
        /// </summary>
        private static void InteractiveConversation()
        {
            // Uncomment this function to enable interactive mode
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Interactive mode (demonstration - not running)");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(@"
    // To create an interactive chatbot, use this pattern:

    var conversation = new List<ChatMessage>
    {
        new SystemChatMessage(""You are a helpful assistant."")
    };

    while (true)
    {
        Console.Write(""You: "");
        string userInput = Console.ReadLine();
        if (userInput.ToLower() is ""exit"" or ""quit"" or ""bye"")
            break;

        conversation.Add(new UserChatMessage(userInput));

        var response = await chatClient.CompleteChatAsync(
            messages: conversation,
            options: new ChatCompletionOptions
            {
                Temperature = 0.8f
            }
        );

        string assistantMessage = response.Value.Content[0].Text;
        Console.WriteLine($""Assistant: {assistantMessage}"");

        conversation.Add(new AssistantChatMessage(assistantMessage));
    }
    ");
        }
    }
}
