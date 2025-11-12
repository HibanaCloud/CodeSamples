/**
 * 07 - Image Generation
 *
 * This example demonstrates how to generate images using DALL-E models
 * through the Hibana API. The API is fully compatible with OpenAI's
 * image generation endpoints.
 *
 * Model used: dall-e-3 (OpenAI's latest image generation model)
 */

import OpenAI from 'openai';
import fs from 'fs';
import https from 'https';

// Initialize the Hibana client
const client = new OpenAI({
    apiKey: "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.com/v1"
});

async function simpleImageGeneration() {
    /**
     * Generate a simple image with DALL-E 3
     */

    console.log("=".repeat(60));
    console.log("Image Generation - DALL-E 3");
    console.log("=".repeat(60));

    const prompt = "A serene landscape of the Alborz mountains in Iran at sunset, with snow-capped peaks and golden light";

    console.log(`\nPrompt: ${prompt}`);
    console.log("\nGenerating image with DALL-E 3...");

    const response = await client.images.generate({
        model: "dall-e-3",
        prompt: prompt,
        size: "1024x1024",  // DALL-E 3 sizes: 1024x1024, 1024x1792, 1792x1024
        quality: "standard",  // "standard" or "hd"
        n: 1  // Number of images (DALL-E 3 only supports 1)
    });

    // Get the image URL
    const imageUrl = response.data[0].url;

    console.log("\n" + "=".repeat(60));
    console.log("Image generated successfully!");
    console.log("=".repeat(60));
    console.log(`\nImage URL: ${imageUrl}`);
    console.log(`\nRevised Prompt: ${response.data[0].revised_prompt}`);

    console.log("\nYou can:");
    console.log("  1. Open the URL in a browser");
    console.log("  2. Download the image programmatically");
    console.log("  3. Use it in your application");
}

async function generateWithDifferentSizes() {
    /**
     * Generate images with different aspect ratios
     */

    console.log("\n" + "=".repeat(60));
    console.log("Image Generation - Different Sizes");
    console.log("=".repeat(60));

    const prompt = "A minimalist logo for a tech startup, featuring a stylized AI brain";

    const sizes = ["1024x1024", "1024x1792", "1792x1024"];

    console.log(`\nPrompt: ${prompt}`);
    console.log("\nGenerating images in different sizes...\n");

    for (const size of sizes) {
        console.log(`Generating ${size}...`);

        const response = await client.images.generate({
            model: "dall-e-3",
            prompt: prompt,
            size: size,
            quality: "standard"
        });

        console.log(`  URL: ${response.data[0].url}`);
    }
}

async function generateHdQuality() {
    /**
     * Generate high-quality image with DALL-E 3
     */

    console.log("\n" + "=".repeat(60));
    console.log("Image Generation - HD Quality");
    console.log("=".repeat(60));

    const prompt = "A photorealistic portrait of a Persian cat wearing a tiny crown, studio lighting, highly detailed";

    console.log(`\nPrompt: ${prompt}`);
    console.log("\nGenerating HD quality image (takes longer, costs more)...");

    const response = await client.images.generate({
        model: "dall-e-3",
        prompt: prompt,
        size: "1024x1024",
        quality: "hd",  // Higher quality, more detailed
        style: "natural"  // "vivid" or "natural"
    });

    console.log("\nHD Image generated!");
    console.log(`URL: ${response.data[0].url}`);
}

async function generateWithStyle() {
    /**
     * Compare vivid vs natural styles
     */

    console.log("\n" + "=".repeat(60));
    console.log("Image Generation - Style Comparison");
    console.log("=".repeat(60));

    const prompt = "A futuristic city with flying cars and neon lights";

    const styles = ["vivid", "natural"];

    console.log(`\nPrompt: ${prompt}`);
    console.log("\nGenerating with different styles...\n");

    for (const style of styles) {
        console.log(`Style: ${style}`);

        const response = await client.images.generate({
            model: "dall-e-3",
            prompt: prompt,
            size: "1024x1024",
            quality: "standard",
            style: style
        });

        console.log(`  URL: ${response.data[0].url}`);
        console.log(`  Revised: ${response.data[0].revised_prompt.substring(0, 80)}...`);
        console.log();
    }
}

async function downloadImage() {
    /**
     * Generate and download an image to local file
     */

    console.log("\n" + "=".repeat(60));
    console.log("Image Generation - Download to File");
    console.log("=".repeat(60));

    const prompt = "A beautiful Persian garden with traditional architecture and fountains";

    console.log(`\nPrompt: ${prompt}`);
    console.log("\nGenerating and downloading image...");

    // Generate image
    const response = await client.images.generate({
        model: "dall-e-3",
        prompt: prompt,
        size: "1024x1024",
        quality: "standard"
    });

    const imageUrl = response.data[0].url;

    // Download the image
    const timestamp = new Date().toISOString().replace(/[:.]/g, '-').slice(0, -5);
    const filename = `generated_image_${timestamp}.png`;

    return new Promise((resolve, reject) => {
        https.get(imageUrl, (res) => {
            const fileStream = fs.createWriteStream(filename);
            res.pipe(fileStream);

            fileStream.on('finish', () => {
                fileStream.close();
                console.log(`\nImage downloaded successfully!`);
                console.log(`Saved as: ${filename}`);
                resolve();
            });
        }).on('error', (err) => {
            reject(err);
        });
    });
}

async function batchImageGeneration() {
    /**
     * Generate multiple images with different prompts
     */

    console.log("\n" + "=".repeat(60));
    console.log("Image Generation - Batch Processing");
    console.log("=".repeat(60));

    const prompts = [
        "A coffee cup on a wooden table, morning light",
        "Abstract geometric patterns in blue and gold",
        "A cozy reading nook with bookshelves"
    ];

    console.log("\nGenerating multiple images...\n");

    const results = [];

    for (let i = 0; i < prompts.length; i++) {
        const prompt = prompts[i];
        console.log(`${i + 1}. Generating: ${prompt}`);

        try {
            const response = await client.images.generate({
                model: "dall-e-3",
                prompt: prompt,
                size: "1024x1024",
                quality: "standard"
            });

            results.push({
                prompt: prompt,
                url: response.data[0].url,
                revised_prompt: response.data[0].revised_prompt
            });

            console.log(`    Success`);

        } catch (error) {
            console.log(`    Error: ${error.message}`);
        }
    }

    console.log("\n" + "=".repeat(60));
    console.log("Batch Results:");
    console.log("=".repeat(60));
    console.log(JSON.stringify(results, null, 2));
}

// Run the examples
(async () => {
    try {
        // Example 1: Simple generation
        await simpleImageGeneration();

        // Example 2: Different sizes
        await generateWithDifferentSizes();

        // Example 3: HD quality
        await generateHdQuality();

        // Example 4: Style comparison
        await generateWithStyle();

        // Example 5: Download image (commented to avoid file creation)
        // Uncomment to actually download:
        // await downloadImage();

        // Example 6: Batch generation
        // await batchImageGeneration();

        console.log("\n" + "=".repeat(60));
        console.log("Image generation examples completed!");
        console.log("=".repeat(60));

    } catch (error) {
        console.error(`\nError: ${error.message}`);
    }
})();
