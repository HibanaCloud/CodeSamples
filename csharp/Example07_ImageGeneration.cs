/*
 * 07 - Image Generation
 *
 * This example generates an image from a text prompt with the Hibana API,
 * using the official OpenAI .NET SDK's ImageClient.
 *
 * - With no size the image is square (1:1). Other aspect ratios ("16:9",
 *   "9:16", ...) are sent as `size` in a plain HTTP request, because the SDK's
 *   typed size only knows pixel sizes; see https://hibanacloud.ir/docs/dalle.
 * - The response is a temporary URL that expires after about an hour,
 *   so the example downloads the image right away.
 *
 * Model used: gemini-3.1-flash-lite-image
 */

using System;
using System.ClientModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Images;

namespace Hibana.Samples
{
    public class Example07_ImageGeneration
    {
        // Reads your Hibana API key from the HIBANA_API_KEY environment variable
        private static readonly string ApiKey = Environment.GetEnvironmentVariable("HIBANA_API_KEY") ?? "YOUR_API_KEY";
        private static readonly string BaseUrl = "https://api-ai.hibanacloud.ir/v1";
        private const string Model = "gemini-3.1-flash-lite-image";

        public static async Task Main(string[] args)
        {
            try
            {
                var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
                {
                    Endpoint = new Uri(BaseUrl)
                });
                ImageClient imageClient = client.GetImageClient(Model);

                string prompt = "A serene landscape of the Alborz mountains in Iran at sunset, with snow-capped peaks and golden light";

                Console.WriteLine(new string('=', 60));
                Console.WriteLine($"Image Generation - {Model} (1:1)");
                Console.WriteLine(new string('=', 60));
                Console.WriteLine($"\nPrompt: {prompt}");
                Console.WriteLine("\nGenerating...");

                GeneratedImage image = await imageClient.GenerateImageAsync(prompt);

                Console.WriteLine($"\nTemporary URL (valid for about an hour):\n{image.ImageUri}");

                using var http = new HttpClient();
                byte[] bytes = await http.GetByteArrayAsync(image.ImageUri);
                await File.WriteAllBytesAsync("alborz_1x1.png", bytes);
                Console.WriteLine("\nSaved to alborz_1x1.png");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }
    }
}
