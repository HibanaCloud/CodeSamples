/**
 * 01 - Simple Chat Completion
 *
 * This example demonstrates the most basic usage of Hibana API
 * using the OpenAI Node.js SDK. It shows how to:
 * - Initialize the client with Hibana's base URL
 * - Send a simple chat completion request
 * - Receive and display the response
 *
 * Model used: gpt-5-nano (Fast and efficient GPT model)
 */

import OpenAI from 'openai';

// Initialize the Hibana client
// Replace "YOUR_API_KEY" with your actual Hibana API key
const client = new OpenAI({
    apiKey: "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.com/v1"
});

async function simpleChat() {
    /**
     * Send a simple chat message and get a response
     */

    console.log("Sending message to gpt-5-nano...");

    // Create a chat completion
    const response = await client.chat.completions.create({
        model: "gpt-5-nano",  // Fast OpenAI model
        messages: [
            {
                role: "user",
                content: "Hello! Please introduce yourself in one sentence."
            }
        ],
        temperature: 1.0,  // Controls randomness (0.0 = deterministic, 2.0 = very random)
        max_tokens: 10000    // Maximum tokens in the response
    });

    // Extract the response
    const assistantMessage = response.choices[0].message.content;
    const finishReason = response.choices[0].finish_reason;

    // Display the response
    console.log("\n" + "=".repeat(60));
    console.log("Response from gpt-5-nano:");
    console.log("=".repeat(60));
    console.log(assistantMessage);
    console.log("=".repeat(60));
    console.log(`\nFinish reason: ${finishReason}`);

    // Display token usage
    if (response.usage) {
        console.log(`\nToken Usage:`);
        console.log(`  Prompt tokens: ${response.usage.prompt_tokens}`);
        console.log(`  Completion tokens: ${response.usage.completion_tokens}`);
        console.log(`  Total tokens: ${response.usage.total_tokens}`);
        if (response.usage.cost_rial) {
            console.log(`  Cost: ${response.usage.cost_rial} Rials`);
        }
    }
}

// Run the example
(async () => {
    try {
        await simpleChat();
    } catch (error) {
        console.error(`Error: ${error.message}`);
    }
})();
