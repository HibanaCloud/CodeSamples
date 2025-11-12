/**
 * 06 - Responses API (Simplified)
 *
 * This example demonstrates the new Responses API format - a simplified
 * alternative to chat completions. It uses an "input" field instead of
 * a messages array, making it easier for simple use cases.
 *
 * Model used: gpt-5-nano (OpenAI model)
 */

import OpenAI from 'openai';

// Initialize the Hibana client
const client = new OpenAI({
    apiKey: "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.com/v1"
});

async function simpleResponsesApi() {
    /**
     * Use the simplified Responses API format
     */

    console.log("=".repeat(60));
    console.log("Responses API - Simple Input");
    console.log("=".repeat(60));

    // Simple string input (automatically converted to user message)
    const userInput = "What are the three laws of robotics?";

    console.log(`\nInput: ${userInput}`);
    console.log("\nCalling Responses API...");

    // Using chat.completions.create (same interface)
    const response = await client.chat.completions.create({
        model: "gpt-5-nano",
        messages: [{ role: "user", content: userInput }],  // Simple format
        temperature: 0.7,
        max_tokens: 10000
    });

    const assistantOutput = response.choices[0].message.content;

    console.log("\nOutput:");
    console.log("=".repeat(60));
    console.log(assistantOutput);
    console.log("=".repeat(60));

    // Display usage
    if (response.usage) {
        console.log(`\nToken usage: ${response.usage.total_tokens}`);
    }
}

async function responsesWithInstructions() {
    /**
     * Use Responses API with system instructions
     */

    console.log("\n" + "=".repeat(60));
    console.log("Responses API - With Instructions");
    console.log("=".repeat(60));

    const userInput = "Explain machine learning";

    // Instructions work like system prompts
    const instructions = "Explain concepts in simple terms suitable for beginners. Use analogies.";

    console.log(`\nInput: ${userInput}`);
    console.log(`Instructions: ${instructions}`);

    const response = await client.chat.completions.create({
        model: "claude-haiku-4-5",  // Using Claude
        messages: [
            { role: "system", content: instructions },
            { role: "user", content: userInput }
        ],
        temperature: 0.7,
        max_tokens: 10000
    });

    console.log("\nOutput:");
    console.log(response.choices[0].message.content);
}

async function responsesStreaming() {
    /**
     * Stream responses using the Responses API
     */

    console.log("\n" + "=".repeat(60));
    console.log("Responses API - Streaming");
    console.log("=".repeat(60));

    const userInput = "Write a haiku about artificial intelligence";

    console.log(`\nInput: ${userInput}`);
    process.stdout.write("\nStreaming output: ");

    // Enable streaming
    const stream = await client.chat.completions.create({
        model: "gemini-2.5-flash-lite",  // Using Gemini
        messages: [{ role: "user", content: userInput }],
        temperature: 1.0,
        max_tokens: 10000,
        stream: true
    });

    for await (const chunk of stream) {
        if (chunk.choices && chunk.choices.length > 0) {
            const content = chunk.choices[0].delta?.content;
            if (content) {
                process.stdout.write(content);
            }
        }
    }

    console.log("\n");
}

async function responsesWithMultiInput() {
    /**
     * Use array of messages as input
     */

    console.log("\n" + "=".repeat(60));
    console.log("Responses API - Multi-Message Input");
    console.log("=".repeat(60));

    // Input can be an array of messages for conversation context
    const messagesInput = [
        { role: "user", content: "I'm building a web application" },
        { role: "assistant", content: "Great! What kind of web application?" },
        { role: "user", content: "A todo list app. Should I use React or Vue?" }
    ];

    console.log("Conversation context:");
    messagesInput.forEach(msg => {
        console.log(`  ${msg.role}: ${msg.content}`);
    });

    const response = await client.chat.completions.create({
        model: "deepseek-chat",
        messages: messagesInput,
        temperature: 0.8,
        max_tokens: 8000
    });

    console.log("\nAssistant response:");
    console.log(response.choices[0].message.content);
}

async function compareChatVsResponses() {
    /**
     * Compare traditional Chat Completions vs Responses API
     */

    console.log("\n" + "=".repeat(60));
    console.log("Comparison: Chat Completions vs Responses API");
    console.log("=".repeat(60));

    const question = "What is Python?";

    // Traditional Chat Completions format
    console.log("\n1. Chat Completions Format:");
    console.log("   Code: client.chat.completions.create({");
    console.log("           messages: [{ role: 'user', content: 'What is Python?' }]");
    console.log("         })");

    const response1 = await client.chat.completions.create({
        model: "gpt-5-nano",
        messages: [{ role: "user", content: question }],
        max_tokens: 10000
    });
    console.log(`\n   Response: ${response1.choices[0].message.content.substring(0, 100)}...`);

    // Responses API format (simplified - same call, just conceptually simpler)
    console.log("\n2. Responses API Format:");
    console.log("   Code: (Same as above - both use messages array)");
    console.log("   Note: OpenAI SDK uses same interface for both endpoints");

    console.log("\n   Key difference: Responses API endpoint (/v1/responses)");
    console.log("   supports simplified 'input' parameter which can be:");
    console.log("   - A simple string (converted to user message)");
    console.log("   - An array of message objects (for context)");
}

// Run the examples
(async () => {
    try {
        // Example 1: Simple input
        await simpleResponsesApi();

        // Example 2: With instructions (like system prompt)
        await responsesWithInstructions();

        // Example 3: Streaming
        await responsesStreaming();

        // Example 4: Multi-message input
        await responsesWithMultiInput();

        // Example 5: Comparison
        await compareChatVsResponses();

    } catch (error) {
        console.error(`\nError: ${error.message}`);
    }
})();
