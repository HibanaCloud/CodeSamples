package com.hibana.samples;

import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.models.images.ImageGenerateParams;
import com.openai.models.images.ImagesResponse;

import java.io.InputStream;
import java.net.URL;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardCopyOption;

/**
 * 07 - Image Generation
 *
 * This example generates an image from a text prompt with the Hibana API,
 * using the official OpenAI Java SDK.
 *
 * - `size` is an ASPECT RATIO such as "1:1", "16:9" or "9:16"
 *   (all ten are listed at https://hibanacloud.ir/docs/dalle).
 * - The response is a temporary URL that expires after about an hour,
 *   so the example downloads the image right away.
 *
 * Model used: gemini-3.1-flash-lite-image
 */
public class Example07_ImageGeneration {

    // Reads your Hibana API key from the HIBANA_API_KEY environment variable
    private static final String API_KEY = System.getenv().getOrDefault("HIBANA_API_KEY", "YOUR_API_KEY");
    private static final String BASE_URL = "https://api-ai.hibanacloud.ir/v1";
    private static final String MODEL = "gemini-3.1-flash-lite-image";

    public static void main(String[] args) {
        try {
            generateImage(
                    "A serene landscape of the Alborz mountains in Iran at sunset, with snow-capped peaks and golden light",
                    "16:9",
                    "alborz_16x9.png");
        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void generateImage(String prompt, String size, String filename) throws Exception {
        System.out.println("=".repeat(60));
        System.out.println("Image Generation - " + MODEL + " (" + size + ")");
        System.out.println("=".repeat(60));
        System.out.println("\nPrompt: " + prompt);
        System.out.println("\nGenerating...");

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        ImageGenerateParams params = ImageGenerateParams.builder()
                .model(MODEL)
                .prompt(prompt)
                .size(ImageGenerateParams.Size.of(size))
                .build();

        ImagesResponse response = client.images().generate(params);
        String url = response.data().orElseThrow().get(0).url().orElseThrow();
        System.out.println("\nTemporary URL (valid for about an hour):\n" + url);

        try (InputStream in = new URL(url).openStream()) {
            Files.copy(in, Path.of(filename), StandardCopyOption.REPLACE_EXISTING);
        }
        System.out.println("\nSaved to " + filename);
    }
}
