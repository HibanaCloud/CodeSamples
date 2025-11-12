/**
 * 02 - Chat with System Prompt
 *
 * This example demonstrates how to use system prompts to control
 * the AI's behavior, tone, and expertise. System prompts set the
 * context and guidelines for how the assistant should respond.
 *
 * Model used: claude-haiku-4-5 (Claude Haiku - Fast Anthropic model)
 */

import OpenAI from 'openai';

// Initialize the Hibana client
const client = new OpenAI({
    apiKey: "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.com/v1"
});

async function chatWithSystemPrompt() {
    /**
     * Demonstrate using system prompts to control AI behavior
     */

    // Define a custom system prompt
    const systemPrompt = `You are a English-speaking AI assistant specializing in
explaining technical concepts in simple terms. Always respond in English
and use analogies from everyday life to explain complex topics. Be friendly and
encouraging.`;

    const userQuestion = "What is machine learning and how does it work?";

    console.log("Sending message to claude-haiku-4-5 with custom system prompt...");
    console.log(`\nSystem Prompt: ${systemPrompt.substring(0, 100)}...`);
    console.log(`User Question: ${userQuestion}`);

    // Create chat completion with system prompt
    const response = await client.chat.completions.create({
        model: "claude-haiku-4-5",  // Anthropic's Claude Haiku model
        messages: [
            {
                role: "system",  // System message defines AI behavior
                content: systemPrompt
            },
            {
                role: "user",
                content: userQuestion
            }
        ],
        temperature: 0.7,  // Slightly lower for more consistent responses
        max_tokens: 10000
    });

    // Extract and display response
    const assistantMessage = response.choices[0].message.content;

    console.log("\n" + "=".repeat(60));
    console.log("Response from claude-haiku-4-5:");
    console.log("=".repeat(60));
    console.log(assistantMessage);
    console.log("=".repeat(60));

    // Display usage statistics
    if (response.usage) {
        console.log(`\nToken Usage:`);
        console.log(`  Total tokens: ${response.usage.total_tokens}`);
        if (response.usage.cost_rial) {
            console.log(`  Cost: ${response.usage.cost_rial} Rials`);
        }
    }
}

async function compareWithWithoutSystem() {
    /**
     * Compare responses with and without system prompt
     */

    const userMessage = "Explain quantum computing";

    console.log("\n" + "=".repeat(60));
    console.log("Comparison: With vs Without System Prompt");
    console.log("=".repeat(60));

    // Response WITHOUT system prompt
    console.log("\n1. WITHOUT system prompt:");
    const response1 = await client.chat.completions.create({
        model: "gpt-5-nano",
        messages: [{ role: "user", content: userMessage }],
        max_tokens: 10000
    });
    console.log(response1.choices[0].message.content);

    // Response WITH system prompt
    console.log("\n2. WITH system prompt (explain like I'm 10 years old):");
    const response2 = await client.chat.completions.create({
        model: "gpt-5-nano",
        messages: [
            {
                role: "system",
                content: "Explain everything as if talking to a 10-year-old child. Use simple words and fun examples."
            },
            {
                role: "user",
                content: userMessage
            }
        ],
        max_tokens: 10000
    });
    console.log(response2.choices[0].message.content);
}

// Run the examples
(async () => {
    try {
        // Example 1: System prompt for English responses
        await chatWithSystemPrompt();

        // Example 2: Comparison with/without system prompt
        await compareWithWithoutSystem();

    } catch (error) {
        console.error(`Error: ${error.message}`);
    }
})();
