/**
 * 13 - Staying Online During an Internet Cutoff
 *
 * Foreign models (GPT, Claude, Gemini, ...) depend on Iran's international
 * internet link. Hibana also runs models on its own servers inside Iran,
 * which keep answering when that link is cut. This example tries a foreign
 * model first and falls back to a domestic one if the call fails, with the
 * same API key and the same code - only the model name changes.
 *
 * More: https://hibanacloud.ir/blog/ai-api-without-vpn-international-cutoff
 *
 * Models used: gpt-5-nano (primary), gpt-oss (domestic fallback)
 */

import OpenAI from 'openai';

// Reads your Hibana API key from the HIBANA_API_KEY environment variable
const client = new OpenAI({
    apiKey: process.env.HIBANA_API_KEY ?? "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.ir/v1",
    maxRetries: 0,
    timeout: 60_000,
});

const PRIMARY_MODEL = "gpt-5-nano";
const FALLBACK_MODEL = "gpt-oss"; // runs on Hibana's own servers

async function ask(question) {
    for (const model of [PRIMARY_MODEL, FALLBACK_MODEL]) {
        try {
            const response = await client.chat.completions.create({
                model,
                messages: [{ role: "user", content: question }],
            });
            return { model, answer: response.choices[0].message.content };
        } catch (error) {
            // 4xx errors such as a bad key or an empty wallet will fail on any
            // model, so only switch on connection problems and 5xx errors.
            if (error instanceof OpenAI.APIError && error.status && error.status < 500) throw error;
            console.log(`${model} failed (${error.constructor.name}), trying ${FALLBACK_MODEL}...`);
        }
    }
    throw new Error("Both models failed");
}

console.log("=".repeat(60));
console.log("Cutoff-ready chat: primary model with a domestic fallback");
console.log("=".repeat(60));

ask("In one sentence, what is an API?")
    .then(({ model, answer }) => {
        console.log(`\nAnswered by: ${model}`);
        console.log(`\n${answer}`);
    })
    .catch((error) => {
        console.error("Error:", error.message);
        process.exitCode = 1;
    });
