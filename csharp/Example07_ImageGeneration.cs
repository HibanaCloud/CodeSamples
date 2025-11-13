/*
 * 07 - Image Generation
 *
 * This example demonstrates how to generate images using DALL-E models
 * through the Hibana API. The API is fully compatible with OpenAI's
 * image generation endpoints.
 *
 * Model used: dall-e-3 (OpenAI's latest image generation model)
 */

using System;
using System.ClientModel;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Images;

namespace Hibana.Samples
{
    public class Example07_ImageGeneration
    {
        private static readonly string ApiKey = "YOUR_API_KEY";
        private static readonly string BaseUrl = "https://api-ai.hibanacloud.com/v1";

        public static async Task Main(string[] args)
        {
            try
            {
                // Example 1: Simple generation
                await SimpleImageGeneration();

                // Example 2: Different sizes
                await GenerateWithDifferentSizes();

                // Example 3: HD quality
                await GenerateHdQuality();

                // Example 4: Style comparison
                await GenerateWithStyle();

                // Example 5: Download image (commented to avoid file creation)
                // Uncomment to actually download:
                // await DownloadImage();

                // Example 6: Batch generation
                // await BatchImageGeneration();

                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine("Image generation examples completed!");
                Console.WriteLine(new string('=', 60));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        /// <summary>
        /// Generate a simple image with DALL-E 3
        /// </summary>
        private static async Task SimpleImageGeneration()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var imageClient = client.GetImageClient("dall-e-3");

            Console.WriteLine(new string('=', 60));
            Console.WriteLine("Image Generation - DALL-E 3");
            Console.WriteLine(new string('=', 60));

            string prompt = "A serene landscape of the Alborz mountains in Iran at sunset, with snow-capped peaks and golden light";

            Console.WriteLine($"\nPrompt: {prompt}");
            Console.WriteLine("\nGenerating image with DALL-E 3...");

            var result = await imageClient.GenerateImageAsync(
                prompt: prompt,
                options: new ImageGenerationOptions
                {
                    Size = GeneratedImageSize.W1024xH1024,  // DALL-E 3 sizes: 1024x1024, 1024x1792, 1792x1024
                    Quality = GeneratedImageQuality.Standard,  // "standard" or "hd"
                    ResponseFormat = GeneratedImageFormat.Uri
                }
            );

            // Get the image URL
            string imageUrl = result.Value.ImageUri.ToString();

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Image generated successfully!");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"\nImage URL: {imageUrl}");
            Console.WriteLine($"\nRevised Prompt: {result.Value.RevisedPrompt}");

            Console.WriteLine("\nYou can:");
            Console.WriteLine("  1. Open the URL in a browser");
            Console.WriteLine("  2. Download the image programmatically");
            Console.WriteLine("  3. Use it in your application");
        }

        /// <summary>
        /// Generate images with different aspect ratios
        /// </summary>
        private static async Task GenerateWithDifferentSizes()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var imageClient = client.GetImageClient("dall-e-3");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Image Generation - Different Sizes");
            Console.WriteLine(new string('=', 60));

            string prompt = "A minimalist logo for a tech startup, featuring a stylized AI brain";

            var sizes = new[]
            {
                GeneratedImageSize.W1024xH1024,
                GeneratedImageSize.W1024xH1792,
                GeneratedImageSize.W1792xH1024
            };

            Console.WriteLine($"\nPrompt: {prompt}");
            Console.WriteLine("\nGenerating images in different sizes...\n");

            foreach (var size in sizes)
            {
                Console.WriteLine($"Generating {size}...");

                var result = await imageClient.GenerateImageAsync(
                    prompt: prompt,
                    options: new ImageGenerationOptions
                    {
                        Size = size,
                        Quality = GeneratedImageQuality.Standard,
                        ResponseFormat = GeneratedImageFormat.Uri
                    }
                );

                Console.WriteLine($"  URL: {result.Value.ImageUri}");
            }
        }

        /// <summary>
        /// Generate high-quality image with DALL-E 3
        /// </summary>
        private static async Task GenerateHdQuality()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var imageClient = client.GetImageClient("dall-e-3");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Image Generation - HD Quality");
            Console.WriteLine(new string('=', 60));

            string prompt = "A photorealistic portrait of a Persian cat wearing a tiny crown, studio lighting, highly detailed";

            Console.WriteLine($"\nPrompt: {prompt}");
            Console.WriteLine("\nGenerating HD quality image (takes longer, costs more)...");

            var result = await imageClient.GenerateImageAsync(
                prompt: prompt,
                options: new ImageGenerationOptions
                {
                    Size = GeneratedImageSize.W1024xH1024,
                    Quality = GeneratedImageQuality.High,  // Higher quality, more detailed
                    Style = GeneratedImageStyle.Natural,  // "vivid" or "natural"
                    ResponseFormat = GeneratedImageFormat.Uri
                }
            );

            Console.WriteLine("\nHD Image generated!");
            Console.WriteLine($"URL: {result.Value.ImageUri}");
        }

        /// <summary>
        /// Compare vivid vs natural styles
        /// </summary>
        private static async Task GenerateWithStyle()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var imageClient = client.GetImageClient("dall-e-3");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Image Generation - Style Comparison");
            Console.WriteLine(new string('=', 60));

            string prompt = "A futuristic city with flying cars and neon lights";

            var styles = new[] { GeneratedImageStyle.Vivid, GeneratedImageStyle.Natural };

            Console.WriteLine($"\nPrompt: {prompt}");
            Console.WriteLine("\nGenerating with different styles...\n");

            foreach (var style in styles)
            {
                Console.WriteLine($"Style: {style}");

                var result = await imageClient.GenerateImageAsync(
                    prompt: prompt,
                    options: new ImageGenerationOptions
                    {
                        Size = GeneratedImageSize.W1024xH1024,
                        Quality = GeneratedImageQuality.Standard,
                        Style = style,
                        ResponseFormat = GeneratedImageFormat.Uri
                    }
                );

                Console.WriteLine($"  URL: {result.Value.ImageUri}");
                Console.WriteLine($"  Revised: {result.Value.RevisedPrompt.Substring(0, Math.Min(80, result.Value.RevisedPrompt.Length))}...");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Generate and download an image to local file
        /// </summary>
        private static async Task DownloadImage()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var imageClient = client.GetImageClient("dall-e-3");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Image Generation - Download to File");
            Console.WriteLine(new string('=', 60));

            string prompt = "A beautiful Persian garden with traditional architecture and fountains";

            Console.WriteLine($"\nPrompt: {prompt}");
            Console.WriteLine("\nGenerating and downloading image...");

            // Generate image
            var result = await imageClient.GenerateImageAsync(
                prompt: prompt,
                options: new ImageGenerationOptions
                {
                    Size = GeneratedImageSize.W1024xH1024,
                    Quality = GeneratedImageQuality.Standard,
                    ResponseFormat = GeneratedImageFormat.Uri
                }
            );

            string imageUrl = result.Value.ImageUri.ToString();

            // Download the image
            using var httpClient = new HttpClient();
            var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

            // Save to file with timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filename = $"generated_image_{timestamp}.png";

            await File.WriteAllBytesAsync(filename, imageBytes);

            Console.WriteLine("\nImage downloaded successfully!");
            Console.WriteLine($"Saved as: {filename}");
            Console.WriteLine($"File size: {imageBytes.Length} bytes");
        }

        /// <summary>
        /// Generate multiple images with different prompts
        /// </summary>
        private static async Task BatchImageGeneration()
        {
            var client = new OpenAIClient(new ApiKeyCredential(ApiKey), new OpenAIClientOptions
            {
                Endpoint = new Uri(BaseUrl)
            });

            var imageClient = client.GetImageClient("dall-e-3");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Image Generation - Batch Processing");
            Console.WriteLine(new string('=', 60));

            var prompts = new[]
            {
                "A coffee cup on a wooden table, morning light",
                "Abstract geometric patterns in blue and gold",
                "A cozy reading nook with bookshelves"
            };

            Console.WriteLine("\nGenerating multiple images...\n");

            var results = new List<Dictionary<string, string>>();

            for (int i = 0; i < prompts.Length; i++)
            {
                string prompt = prompts[i];
                Console.WriteLine($"{i + 1}. Generating: {prompt}");

                try
                {
                    var result = await imageClient.GenerateImageAsync(
                        prompt: prompt,
                        options: new ImageGenerationOptions
                        {
                            Size = GeneratedImageSize.W1024xH1024,
                            Quality = GeneratedImageQuality.Standard,
                            ResponseFormat = GeneratedImageFormat.Uri
                        }
                    );

                    results.Add(new Dictionary<string, string>
                    {
                        ["prompt"] = prompt,
                        ["url"] = result.Value.ImageUri.ToString(),
                        ["revised_prompt"] = result.Value.RevisedPrompt
                    });

                    Console.WriteLine("    Success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    Error: {ex.Message}");
                }
            }

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("Batch Results:");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
