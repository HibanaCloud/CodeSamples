/**
 * 07 - Image Generation
 *
 * This example generates an image from a text prompt with the Hibana API,
 * using the OpenAI Node.js SDK's images.generate().
 *
 * - `size` is an ASPECT RATIO such as "1:1", "16:9" or "9:16"
 *   (all ten are listed at https://hibanacloud.ir/docs/dalle).
 * - The response is a temporary URL that expires after about an hour,
 *   so the example downloads the image right away.
 *
 * Model used: gemini-3.1-flash-lite-image
 */

import OpenAI from 'openai';
import { writeFile } from 'fs/promises';

// Reads your Hibana API key from the HIBANA_API_KEY environment variable
const client = new OpenAI({
    apiKey: process.env.HIBANA_API_KEY ?? "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.ir/v1"
});

const MODEL = "gemini-3.1-flash-lite-image";

async function generateImage(prompt, size, filename) {
    console.log("=".repeat(60));
    console.log(`Image Generation - ${MODEL} (${size})`);
    console.log("=".repeat(60));
    console.log(`\nPrompt: ${prompt}`);
    console.log("\nGenerating...");

    const response = await client.images.generate({
        model: MODEL,
        prompt,
        size,
    });

    const url = response.data[0].url;
    console.log(`\nTemporary URL (valid for about an hour):\n${url}`);

    const image = await fetch(url);
    await writeFile(filename, Buffer.from(await image.arrayBuffer()));
    console.log(`\nSaved to ${filename}`);
}

generateImage(
    "A serene landscape of the Alborz mountains in Iran at sunset, with snow-capped peaks and golden light",
    "16:9",
    "alborz_16x9.png"
).catch((error) => {
    console.error("Error:", error.message);
    process.exitCode = 1;
});
