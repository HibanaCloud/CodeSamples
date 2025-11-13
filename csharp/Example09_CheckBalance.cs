/*
 * 09 - Check Balance
 *
 * This example demonstrates how to check your wallet balance using
 * the Hibana API. This endpoint is not part of the standard OpenAI API,
 * so we use HttpClient to call it directly.
 *
 * Endpoint: GET /v1/user/balance
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hibana.Samples
{
    public class Example09_CheckBalance
    {
        // API Configuration
        private static readonly string ApiKey = "YOUR_API_KEY";
        private static readonly string BaseUrl = "https://api-ai.hibanacloud.com/v1";

        public static async Task Main(string[] args)
        {
            try
            {
                // Example 1: Simple balance check
                await CheckBalance();

                // Example 2: Detailed balance check
                await CheckBalanceWithDetails();

                // Example 3: Pre-request balance check
                await MonitorBalanceBeforeRequest();

                // Example 4: Balance with usage recommendations
                await BalanceCheckWithUsageHistory();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Check current wallet balance
        /// </summary>
        private static async Task CheckBalance()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("Check Wallet Balance");
            Console.WriteLine(new string('=', 60));

            // Endpoint URL
            string url = $"{BaseUrl}/user/balance";

            Console.WriteLine("\nFetching balance...");

            try
            {
                using var httpClient = new HttpClient();

                // Headers with API key
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", ApiKey);
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // Make GET request
                var response = await httpClient.GetAsync(url);

                // Check if request was successful
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(jsonResponse);
                    var root = doc.RootElement;

                    Console.WriteLine("\n" + new string('=', 60));
                    Console.WriteLine("Balance Information");
                    Console.WriteLine(new string('=', 60));

                    // Display balance information
                    long balance = root.TryGetProperty("balance", out var balanceProp)
                        ? balanceProp.GetInt64() : 0;
                    string currency = root.TryGetProperty("currency", out var currencyProp)
                        ? currencyProp.GetString() : "IRR";

                    Console.WriteLine($"\nCurrent Balance: {balance:N0} {currency}");

                    // Format with thousands separator
                    if (currency == "IRR")
                    {
                        Console.WriteLine($"Formatted: {balance:N0} Rials");
                    }

                    // Estimate usage capability (example calculation)
                    // Note: Actual cost depends on model and usage
                    long avgCostPerRequest = 100;  // Example: 100 Rials per request
                    long estimatedRequests = balance / avgCostPerRequest;

                    Console.WriteLine($"\nEstimated remaining requests: ~{estimatedRequests:N0}");
                    Console.WriteLine("(Based on average cost, actual may vary)");
                }
                else
                {
                    Console.WriteLine($"\nError: {response.StatusCode}");
                    Console.WriteLine($"Message: {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"\nRequest error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Check balance with additional error handling and details
        /// </summary>
        private static async Task CheckBalanceWithDetails()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Detailed Balance Check");
            Console.WriteLine(new string('=', 60));

            string url = $"{BaseUrl}/user/balance";

            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", ApiKey);
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.GetAsync(url);

                Console.WriteLine($"\nStatus Code: {(int)response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(jsonResponse);
                    var root = doc.RootElement;

                    Console.WriteLine("\nFull Response:");
                    Console.WriteLine(new string('-', 60));

                    // Display all fields in response
                    foreach (var property in root.EnumerateObject())
                    {
                        Console.WriteLine($"{property.Name}: {property.Value}");
                    }

                    Console.WriteLine(new string('-', 60));

                    // Check balance status
                    long balance = root.TryGetProperty("balance", out var balanceProp)
                        ? balanceProp.GetInt64() : 0;

                    string status;
                    if (balance > 10000)
                    {
                        status = "✓ Healthy balance";
                    }
                    else if (balance > 1000)
                    {
                        status = "⚠ Low balance";
                    }
                    else
                    {
                        status = "✗ Critical - Please recharge";
                    }

                    Console.WriteLine($"\nStatus: {status}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("\n✗ Authentication Error");
                    Console.WriteLine("Your API key may be invalid or expired.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Console.WriteLine("\n✗ Access Forbidden");
                    Console.WriteLine("Your account may not have permission to access this endpoint.");
                }
                else
                {
                    Console.WriteLine($"\n✗ Unexpected error: {response.StatusCode}");
                    Console.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("\n✗ Request timeout - Server took too long to respond");
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("\n✗ Connection error - Could not reach the server");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Check balance before making an API call
        /// </summary>
        private static async Task MonitorBalanceBeforeRequest()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Pre-Request Balance Check");
            Console.WriteLine(new string('=', 60));

            string url = $"{BaseUrl}/user/balance";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", ApiKey);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Check balance
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);
                var root = doc.RootElement;

                long balance = root.TryGetProperty("balance", out var balanceProp)
                    ? balanceProp.GetInt64() : 0;

                Console.WriteLine($"\nCurrent balance: {balance:N0} Rials");

                // Set minimum balance threshold
                long minimumBalance = 1000;  // 1,000 Rials

                if (balance >= minimumBalance)
                {
                    Console.WriteLine("✓ Sufficient balance to proceed with API call");

                    // Here you would make your actual API call
                    Console.WriteLine("\nProceed with API request...");
                }
                else
                {
                    Console.WriteLine("✗ Insufficient balance");
                    Console.WriteLine($"Minimum required: {minimumBalance:N0} Rials");
                    Console.WriteLine("Please recharge your account before making requests.");
                }
            }
            else
            {
                Console.WriteLine($"Could not check balance: {response.StatusCode}");
            }
        }

        /// <summary>
        /// Check balance and provide usage recommendations
        /// </summary>
        private static async Task BalanceCheckWithUsageHistory()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Balance with Usage Recommendations");
            Console.WriteLine(new string('=', 60));

            string url = $"{BaseUrl}/user/balance";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", ApiKey);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);
                var root = doc.RootElement;

                long balance = root.TryGetProperty("balance", out var balanceProp)
                    ? balanceProp.GetInt64() : 0;

                Console.WriteLine($"\nCurrent Balance: {balance:N0} Rials\n");

                // Model cost estimates (example values)
                var modelCosts = new Dictionary<string, long>
                {
                    ["gpt-5-nano"] = 50,
                    ["claude-haiku-4-5"] = 75,
                    ["deepseek-chat"] = 40,
                    ["gemini-2.5-flash-lite"] = 45,
                    ["dall-e-3"] = 5000
                };

                Console.WriteLine("Estimated requests per model:");
                Console.WriteLine(new string('-', 60));

                foreach (var kvp in modelCosts)
                {
                    long requestsPossible = balance / kvp.Value;
                    Console.WriteLine($"{kvp.Key,-25} ~{requestsPossible,6:N0} requests");
                }

                Console.WriteLine(new string('-', 60));

                // Recommendations
                Console.WriteLine("\nRecommendations:");
                if (balance < 5000)
                {
                    Console.WriteLine("  • Consider recharging soon");
                    Console.WriteLine("  • Use cost-efficient models (gpt-5-nano, deepseek-chat)");
                }
                else if (balance < 20000)
                {
                    Console.WriteLine("  • Balance is moderate");
                    Console.WriteLine("  • Monitor usage regularly");
                }
                else
                {
                    Console.WriteLine("  • Balance is healthy");
                    Console.WriteLine("  • You can use any model freely");
                }
            }
            else
            {
                Console.WriteLine($"Error checking balance: {response.StatusCode}");
            }
        }
    }
}
