/**
 * 04 - Streaming Response
 *
 * This example demonstrates real-time streaming of AI responses using
 * Server-Sent Events (SSE). Instead of waiting for the complete response,
 * tokens are received and displayed progressively as they're generated.
 *
 * Model used: gpt-5-nano (Fast OpenAI model)
 */

import OpenAI from 'openai';

// Initialize the Hibana client
const client = new OpenAI({
    apiKey: "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.com/v1"
});

async function streamingChat() {
    /**
     * Demonstrate streaming responses with real-time output
     */

    console.log("=".repeat(60));
    console.log("Streaming Response Demo");
    console.log("=".repeat(60));

    const userMessage = "Write a short story about a robot learning to paint. Make it about 150 words.";

    console.log(`\nUser: ${userMessage}`);
    process.stdout.write("\nAssistant (streaming): ");

    // Create streaming chat completion
    const stream = await client.chat.completions.create({
        model: "gpt-5-nano",
        messages: [
            {
                role: "user",
                content: userMessage
            }
        ],
        temperature: 1.0,
        max_tokens: 10000,
        stream: true  // Enable streaming
    });

    // Collect the full response for later display
    let fullResponse = "";

    // Process the stream
    for await (const chunk of stream) {
        // Check if chunk has choices and content
        if (chunk.choices && chunk.choices.length > 0) {
            const content = chunk.choices[0].delta?.content;
            if (content) {
                fullResponse += content;
                // Print the content immediately (streaming effect)
                process.stdout.write(content);
            }
        }
    }

    console.log("\n\n" + "=".repeat(60));
    console.log("Streaming complete!");
    console.log("=".repeat(60));
    console.log(`\nTotal characters received: ${fullResponse.length}`);
}

async function streamingWithMetadata() {
    /**
     * Streaming with detailed chunk inspection
     */

    console.log("\n" + "=".repeat(60));
    console.log("Streaming with Metadata Inspection");
    console.log("=".repeat(60));

    const userMessage = "Explain quantum entanglement in 2 sentences.";

    console.log(`\nUser: ${userMessage}`);
    console.log("\nStreaming response...\n");

    const stream = await client.chat.completions.create({
        model: "gemini-2.5-flash-lite",  // Using Gemini model
        messages: [{ role: "user", content: userMessage }],
        temperature: 0.7,
        max_tokens: 10000,
        stream: true
    });

    let fullText = "";
    let chunkCount = 0;

    for await (const chunk of stream) {
        chunkCount++;

        // Inspect chunk structure
        if (chunk.choices && chunk.choices.length > 0) {
            const delta = chunk.choices[0].delta;

            // Check for content
            if (delta.content) {
                fullText += delta.content;
                process.stdout.write(delta.content);
            }

            // Check for finish reason
            if (chunk.choices[0].finish_reason) {
                console.log(`\n\nFinish reason: ${chunk.choices[0].finish_reason}`);
            }
        }
    }

    console.log(`\n\nTotal chunks received: ${chunkCount}`);
    console.log(`Full response length: ${fullText.length} characters`);
}

async function compareStreamingVsNormal() {
    /**
     * Compare streaming vs non-streaming response times
     */

    console.log("\n" + "=".repeat(60));
    console.log("Comparison: Streaming vs Non-Streaming");
    console.log("=".repeat(60));

    const message = "List 5 benefits of cloud computing.";

    // Non-streaming
    console.log("\n1. NON-STREAMING:");
    const startTime = Date.now();
    const response = await client.chat.completions.create({
        model: "gpt-5-nano",
        messages: [{ role: "user", content: message }],
        max_tokens: 10000,
        stream: false
    });
    const endTime = Date.now();

    console.log(`   Time to first output: ${((endTime - startTime) / 1000).toFixed(2)} seconds`);
    console.log(`   Response: ${response.choices[0].message.content.substring(0, 100)}...`);

    // Streaming
    console.log("\n2. STREAMING:");
    const startTimeStream = Date.now();
    let firstChunkTime = null;

    const stream = await client.chat.completions.create({
        model: "gpt-5-nano",
        messages: [{ role: "user", content: message }],
        max_tokens: 10000,
        stream: true
    });

    let i = 0;
    for await (const chunk of stream) {
        if (chunk.choices && chunk.choices.length > 0 && chunk.choices[0].delta?.content) {
            if (i === 0) {
                firstChunkTime = Date.now();
                console.log(`   Time to first output: ${((firstChunkTime - startTimeStream) / 1000).toFixed(2)} seconds`);
                console.log(`   First chunk: ${chunk.choices[0].delta.content}`);
                break;
            }
            i++;
        }
    }

    console.log("\n   Streaming provides faster time-to-first-token!");
}

// Run the examples
(async () => {
    try {
        // Example 1: Basic streaming
        await streamingChat();

        // Example 2: Streaming with metadata
        await streamingWithMetadata();

        // Example 3: Compare streaming vs non-streaming
        await compareStreamingVsNormal();

    } catch (error) {
        console.error(`\nError: ${error.message}`);
    }
})();
