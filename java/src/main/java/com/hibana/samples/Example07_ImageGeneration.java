package com.hibana.samples;

import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.models.Image;
import com.openai.models.ImageGenerateParams;
import com.openai.models.ImagesResponse;

/**
 * 07 - Image Generation
 *
 * This example demonstrates how to generate images using AI models
 * like DALL-E. The API accepts text prompts and returns image URLs
 * that can be downloaded or displayed.
 *
 * Model used: dall-e-3
 */
public class Example07_ImageGeneration {

    private static final String API_KEY = "YOUR_API_KEY";
    private static final String BASE_URL = "https://api-ai.hibanacloud.com/v1";

    public static void main(String[] args) {
        try {
            basicImageGeneration();
            System.out.println("\n");
            imageGenerationWithOptions();
            System.out.println("\n");
            multipleImages();
        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void basicImageGeneration() {
        /**
         * Basic image generation
         */

        System.out.println("=".repeat(60));
        System.out.println("Basic Image Generation");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        String prompt = "A serene Japanese garden with cherry blossoms and a koi pond";

        System.out.println("\nPrompt: " + prompt);
        System.out.println("\nGenerating image...");

        ImageGenerateParams params = ImageGenerateParams.builder()
                .model("dall-e-3")
                .prompt(prompt)
                .n(1L)
                .size(ImageGenerateParams.Size._1024X1024)
                .build();

        ImagesResponse response = client.images().generate(params);

        System.out.println("\n" + "-".repeat(60));
        System.out.println("Image generated successfully!");
        System.out.println("-".repeat(60));

        for (int i = 0; i < response.data().size(); i++) {
            Image image = response.data().get(i);
            System.out.println("\nImage " + (i + 1) + ":");
            image.url().ifPresent(url -> System.out.println("  URL: " + url));
            image.revisedPrompt().ifPresent(revised -> System.out.println("  Revised prompt: " + revised));
        }
    }

    private static void imageGenerationWithOptions() {
        /**
         * Image generation with specific options
         */

        System.out.println("=".repeat(60));
        System.out.println("Image Generation with Options");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        String prompt = "A futuristic cityscape at sunset with flying cars, cyberpunk style";

        System.out.println("\nPrompt: " + prompt);
        System.out.println("Quality: HD");
        System.out.println("Style: Vivid");
        System.out.println("\nGenerating image...");

        ImageGenerateParams params = ImageGenerateParams.builder()
                .model("dall-e-3")
                .prompt(prompt)
                .n(1L)
                .size(ImageGenerateParams.Size._1024X1024)
                .quality(ImageGenerateParams.Quality.HD)
                .style(ImageGenerateParams.Style.VIVID)
                .build();

        ImagesResponse response = client.images().generate(params);

        System.out.println("\n" + "-".repeat(60));
        System.out.println("High-quality image generated!");
        System.out.println("-".repeat(60));

        response.data().forEach(image -> {
            image.url().ifPresent(url -> {
                System.out.println("\nImage URL: " + url);
                System.out.println("Download this URL to view your generated image.");
            });
            image.revisedPrompt().ifPresent(revised -> {
                System.out.println("\nRevised prompt: " + revised);
            });
        });
    }

    private static void multipleImages() {
        /**
         * Generate multiple image variations
         */

        System.out.println("=".repeat(60));
        System.out.println("Multiple Image Variations");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        String prompt = "A cute robot reading a book in a cozy library";

        System.out.println("\nPrompt: " + prompt);
        System.out.println("Generating 2 variations...\n");

        ImageGenerateParams params = ImageGenerateParams.builder()
                .model("dall-e-3")
                .prompt(prompt)
                .n(2L)  // Generate 2 images
                .size(ImageGenerateParams.Size._1024X1024)
                .build();

        ImagesResponse response = client.images().generate(params);

        System.out.println("-".repeat(60));
        System.out.println("Generated " + response.data().size() + " variations:");
        System.out.println("-".repeat(60));

        for (int i = 0; i < response.data().size(); i++) {
            Image image = response.data().get(i);
            System.out.println("\nVariation " + (i + 1) + ":");
            image.url().ifPresent(url -> System.out.println("  " + url));
        }

        System.out.println("\n" + "=".repeat(60));
        System.out.println("Tip: Save these URLs to download the images!");
        System.out.println("=".repeat(60));
    }
}
