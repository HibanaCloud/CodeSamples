/*
 * 10 - Error Handling
 *
 * This example demonstrates proper error handling when using the Hibana API.
 * It covers common errors, retry strategies, and best practices for building
 * robust applications.
 */

using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;

namespace Hibana.Samples
{
    public class Example10_ErrorHandling
    {
        private static readonly string ApiKey = "YOUR_API_KEY";
        private static readonly string BaseUrl = "https://api-ai.hibanacloud.com/v1";

        public static async Task Main(string[] args)
        {
            try
            {
                // Example 1: Authentication errors
                // await HandleAuthenticationError();  // Uncomment to test

                // Example 2: Model not found
                // await HandleModelNotFound();  // Uncomment to test

                // Example 3: Rate limiting
                // await HandleRateLimit();  // Uncomment to test

                // Example 4: Insufficient balance
                await HandleInsufficientBalance();

                // Example 5: Retry with backoff
                await RetryWithExponentialBackoff();

                // Example 6: Timeout handling
                // await HandleTimeout();  // Uncomment to test

                // Example 7: Comprehensive handler
                await ComprehensiveErrorHandler();

                // Example 8: Input validation
                await ValidateBeforeRequest();

                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine("Error handling examples completed!");
                Console.WriteLine(new string('=', 60));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nUnexpected error in main: {ex.Message}");
            }
        }

        /// <summary>
        /// Handle authentication errors (invalid API key)
        /// </summary>
        private static async Task HandleAuthenticationError()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("Error Handling - Authentication");
            Console.WriteLine(new string('=', 60));

            // Create client with invalid API key
            var badClient = new OpenAIClient(new ApiKeyCredential("INVALID_KEY_123"), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = badClient.GetChatClient("gpt-5-nano");

            try
            {
                await chatClient.CompleteChatAsync(
                    messages: new[] { new UserChatMessage("Hello") }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n✗ Authentication Error Caught!");
                Console.WriteLine($"Error message: {ex.Message}");
                Console.WriteLine("\nSolution:");
                Console.WriteLine("  1. Check that your API key is correct");
                Console.WriteLine("  2. Verify the key is active in your dashboard");
                Console.WriteLine("  3. Ensure proper Bearer token format");
            }
        }

        /// <summary>
        /// Handle errors when requesting unavailable models
        /// </summary>
        private static async Task HandleModelNotFound()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Error Handling - Model Not Found");
            Console.WriteLine(new string('=', 60));

            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("non-existent-model-xyz");

            try
            {
                await chatClient.CompleteChatAsync(
                    messages: new[] { new UserChatMessage("Hello") }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n✗ Model Not Found Error!");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("\nSolution:");
                Console.WriteLine("  1. Use modelClient.GetModelsAsync() to see available models");
                Console.WriteLine("  2. Check for typos in model name");
                Console.WriteLine("  3. Verify model is enabled in your account");
            }
        }

        /// <summary>
        /// Handle rate limiting errors
        /// </summary>
        private static async Task HandleRateLimit()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Error Handling - Rate Limiting");
            Console.WriteLine(new string('=', 60));

            Console.WriteLine("\nSimulating rate limit scenario...");

            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gpt-5-nano");

            try
            {
                // This might trigger rate limiting if called too frequently
                for (int i = 0; i < 5; i++)
                {
                    await chatClient.CompleteChatAsync(
                        messages: new[] { new UserChatMessage($"Request {i + 1}") },
                        options: new ChatCompletionOptions { MaxOutputTokenCount = 10000 }
                    );
                    Console.WriteLine($"Request {i + 1}: Success");
                    await Task.Delay(100);  // Small delay
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n⚠ Rate Limit Error: {ex.Message}");
                Console.WriteLine("\nSolution:");
                Console.WriteLine("  1. Implement exponential backoff");
                Console.WriteLine("  2. Add delays between requests");
                Console.WriteLine("  3. Monitor rate limits for your tier");
                Console.WriteLine("  4. Consider upgrading your plan");
            }
        }

        /// <summary>
        /// Handle insufficient balance errors
        /// </summary>
        private static async Task HandleInsufficientBalance()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Error Handling - Insufficient Balance");
            Console.WriteLine(new string('=', 60));

            Console.WriteLine("\nChecking for balance errors...");

            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gpt-5-nano");

            try
            {
                // This would fail if balance is insufficient
                await chatClient.CompleteChatAsync(
                    messages: new[] { new UserChatMessage("Test") },
                    options: new ChatCompletionOptions { MaxOutputTokenCount = 10000 }
                );
                Console.WriteLine("✓ Request successful - sufficient balance");
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message.ToLower();
                if (errorMsg.Contains("insufficient") || errorMsg.Contains("balance"))
                {
                    Console.WriteLine($"\n✗ Insufficient Balance: {ex.Message}");
                    Console.WriteLine("\nSolution:");
                    Console.WriteLine("  1. Check balance: GET /v1/user/balance");
                    Console.WriteLine("  2. Recharge your account");
                    Console.WriteLine("  3. Use lower-cost models");
                }
            }
        }

        /// <summary>
        /// Implement retry logic with exponential backoff
        /// </summary>
        private static async Task RetryWithExponentialBackoff()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Retry Strategy - Exponential Backoff");
            Console.WriteLine(new string('=', 60));

            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var chatClient = client.GetChatClient("gpt-5-nano");

            int maxRetries = 3;
            int baseDelay = 1000;  // milliseconds

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    Console.WriteLine($"\nAttempt {attempt + 1}/{maxRetries}...");

                    var response = await chatClient.CompleteChatAsync(
                        messages: new[] { new UserChatMessage("Hello") },
                        options: new ChatCompletionOptions { MaxOutputTokenCount = 10000 }
                    );

                    Console.WriteLine("✓ Success!");
                    Console.WriteLine($"Response: {response.Value.Content[0].Text}");
                    break;
                }
                catch (Exception ex)
                {
                    if (attempt < maxRetries - 1)
                    {
                        int delay = baseDelay * (int)Math.Pow(2, attempt);  // Exponential backoff
                        Console.WriteLine($"⚠ Error. Retrying in {delay / 1000} seconds...");
                        await Task.Delay(delay);
                    }
                    else
                    {
                        Console.WriteLine($"✗ Failed after {maxRetries} attempts");
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Handle request timeout errors
        /// </summary>
        private static async Task HandleTimeout()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Error Handling - Timeouts");
            Console.WriteLine(new string('=', 60));

            // Set a short timeout for demonstration
            var timeoutClient = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
                // Note: .NET OpenAI client timeout configuration may vary
            });

            var chatClient = timeoutClient.GetChatClient("gpt-5-nano");

            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1)); // Very short timeout
                await chatClient.CompleteChatAsync(
                    messages: new[] { new UserChatMessage("Hello") },
                    cancellationToken: cts.Token
                );
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"\n⚠ Timeout Error: {ex.Message}");
                Console.WriteLine("\nSolution:");
                Console.WriteLine("  1. Increase timeout value");
                Console.WriteLine("  2. Check network connection");
                Console.WriteLine("  3. Try again later if server is slow");
            }
        }

        /// <summary>
        /// A comprehensive error handling wrapper
        /// </summary>
        private static async Task ComprehensiveErrorHandler()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Comprehensive Error Handler");
            Console.WriteLine(new string('=', 60));

            async Task<ChatCompletion?> SafeApiCall(string model, string userMessage, int maxTokens = 10000)
            {
                var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
                {
                    Endpoint = new Uri(BaseUrl)
                });

                var chatClient = client.GetChatClient(model);

                try
                {
                    var response = await chatClient.CompleteChatAsync(
                        messages: new[] { new UserChatMessage(userMessage) },
                        options: new ChatCompletionOptions { MaxOutputTokenCount = maxTokens }
                    );
                    return response.Value;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message.ToLower();

                    if (errorMsg.Contains("authentication") || errorMsg.Contains("api key"))
                    {
                        Console.WriteLine($"✗ Authentication Error: {ex.Message}");
                        Console.WriteLine("Check your API key.");
                    }
                    else if (errorMsg.Contains("not found") || errorMsg.Contains("model"))
                    {
                        Console.WriteLine($"✗ Not Found: {ex.Message}");
                        Console.WriteLine("Model may not exist or be unavailable.");
                    }
                    else if (errorMsg.Contains("rate limit"))
                    {
                        Console.WriteLine($"⚠ Rate Limit: {ex.Message}");
                        Console.WriteLine("Too many requests. Please wait and try again.");
                    }
                    else if (errorMsg.Contains("timeout"))
                    {
                        Console.WriteLine($"⚠ Timeout: {ex.Message}");
                        Console.WriteLine("Request took too long. Try again.");
                    }
                    else if (errorMsg.Contains("bad request"))
                    {
                        Console.WriteLine($"✗ Bad Request: {ex.Message}");
                        Console.WriteLine("Check your request parameters.");
                    }
                    else
                    {
                        Console.WriteLine($"✗ Error: {ex.Message}");
                    }

                    return null;
                }
            }

            // Test the wrapper
            Console.WriteLine("\nTesting with valid request:");
            var result = await SafeApiCall("gpt-5-nano", "Say hello in 3 words");

            if (result != null)
            {
                Console.WriteLine($"✓ Success: {result.Content[0].Text}");
            }
            else
            {
                Console.WriteLine("Request failed gracefully");
            }
        }

        /// <summary>
        /// Validate inputs before making API request
        /// </summary>
        private static async Task ValidateBeforeRequest()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Input Validation");
            Console.WriteLine(new string('=', 60));

            async Task<ChatCompletion?> ValidateAndCall(string model, string userInput)
            {
                // Validate model name
                var validModels = new[] { "gpt-5-nano", "claude-haiku-4-5", "deepseek-chat", "gemini-2.5-flash-lite" };

                if (!validModels.Contains(model))
                {
                    Console.WriteLine($"✗ Invalid model: {model}");
                    Console.WriteLine($"Valid models: {string.Join(", ", validModels)}");
                    return null;
                }

                // Validate user input
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("✗ Empty user input");
                    return null;
                }

                if (userInput.Length > 10000)
                {
                    Console.WriteLine("✗ Input too long (max 10,000 characters)");
                    return null;
                }

                // All validations passed
                Console.WriteLine("✓ Validation passed");

                try
                {
                    var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
                    {
                        Endpoint = new Uri(BaseUrl)
                    });

                    var chatClient = client.GetChatClient(model);

                    var response = await chatClient.CompleteChatAsync(
                        messages: new[] { new UserChatMessage(userInput) },
                        options: new ChatCompletionOptions { MaxOutputTokenCount = 10000 }
                    );
                    return response.Value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Error: {ex.Message}");
                    return null;
                }
            }

            // Test validation
            Console.WriteLine("\nTest 1: Invalid model");
            await ValidateAndCall("invalid-model", "Hello");

            Console.WriteLine("\nTest 2: Empty input");
            await ValidateAndCall("gpt-5-nano", "");

            Console.WriteLine("\nTest 3: Valid request");
            var result = await ValidateAndCall("gpt-5-nano", "Say hi");
            if (result != null)
            {
                Console.WriteLine($"Response: {result.Content[0].Text}");
            }
        }
    }
}
