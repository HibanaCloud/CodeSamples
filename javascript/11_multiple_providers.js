/**
 * 11 - Multiple Providers Comparison
 *
 * This example demonstrates how to use all four LLM providers available
 * through Hibana API: OpenAI, Anthropic, DeepSeek, and Google Gemini.
 * Compare responses, performance, and features across providers.
 */

import OpenAI from 'openai';

// Initialize the Hibana client
const client = new OpenAI({
    apiKey: "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.com/v1"
});

// Define models for each provider
const MODELS = {
    "OpenAI": "gpt-5-nano",
    "Anthropic": "claude-haiku-4-5",
    "DeepSeek": "deepseek-chat",
    "Google": "gemini-2.5-flash-lite"
};

async function compareBasicResponses() {
    /**
     * Compare responses from all four providers
     */

    console.log("=".repeat(60));
    console.log("Comparing All Providers");
    console.log("=".repeat(60));

    const question = "What is the future of artificial intelligence? Answer in 2 sentences.";

    console.log(`\nQuestion: ${question}\n`);

    for (const [provider, model] of Object.entries(MODELS)) {
        console.log(`\n${provider} (${model}):`);
        console.log("-".repeat(60));

        try {
            const startTime = Date.now();

            const response = await client.chat.completions.create({
                model: model,
                messages: [{ role: "user", content: question }],
                temperature: 0.7,
                max_tokens: 8000
            });

            const elapsed = Date.now() - startTime;

            const answer = response.choices[0].message.content;
            const tokens = response.usage?.total_tokens || 0;

            console.log(`Response: ${answer}`);
            console.log(`\nTime: ${(elapsed / 1000).toFixed(2)}s | Tokens: ${tokens}`);

        } catch (error) {
            console.log(`Error: ${error.message}`);
        }
    }
}

async function compareCodingTasks() {
    /**
     * Compare coding assistance from different providers
     */

    console.log("\n" + "=".repeat(60));
    console.log("Coding Task Comparison");
    console.log("=".repeat(60));

    const codingQuestion = "Write a JavaScript function to check if a string is a palindrome. Include comments.";

    console.log(`\nTask: ${codingQuestion}\n`);

    for (const [provider, model] of Object.entries(MODELS)) {
        console.log(`\n${provider} (${model}):`);
        console.log("-".repeat(60));

        try {
            const response = await client.chat.completions.create({
                model: model,
                messages: [
                    {
                        role: "system",
                        content: "You are a JavaScript programming expert."
                    },
                    {
                        role: "user",
                        content: codingQuestion
                    }
                ],
                temperature: 0.3,  // Lower temp for coding
                max_tokens: 8000
            });

            console.log(response.choices[0].message.content);

        } catch (error) {
            console.log(`Error: ${error.message}`);
        }
    }
}

async function compareCreativeWriting() {
    /**
     * Compare creative writing capabilities
     */

    console.log("\n" + "=".repeat(60));
    console.log("Creative Writing Comparison");
    console.log("=".repeat(60));

    const prompt = "Write a two-line poem about technology and humanity.";

    console.log(`\nPrompt: ${prompt}\n`);

    for (const [provider, model] of Object.entries(MODELS)) {
        console.log(`\n${provider} (${model}):`);
        console.log("-".repeat(60));

        try {
            const response = await client.chat.completions.create({
                model: model,
                messages: [{ role: "user", content: prompt }],
                temperature: 1.0,  // Higher temp for creativity
                max_tokens: 8000
            });

            console.log(response.choices[0].message.content);

        } catch (error) {
            console.log(`Error: ${error.message}`);
        }
    }
}

async function benchmarkPerformance() {
    /**
     * Benchmark response time and token usage
     */

    console.log("\n" + "=".repeat(60));
    console.log("Performance Benchmark");
    console.log("=".repeat(60));

    const testMessage = "Explain quantum computing in one sentence.";

    const results = [];

    console.log(`\nTest message: ${testMessage}\n`);
    console.log("Running benchmark...\n");

    for (const [provider, model] of Object.entries(MODELS)) {
        try {
            // Warm-up request (not counted)
            await client.chat.completions.create({
                model: model,
                messages: [{ role: "user", content: "Hi" }],
                max_tokens: 8000
            });

            // Actual benchmark
            const start = Date.now();

            const response = await client.chat.completions.create({
                model: model,
                messages: [{ role: "user", content: testMessage }],
                temperature: 0.5,
                max_tokens: 8000
            });

            const elapsed = Date.now() - start;

            results.push({
                provider: provider,
                model: model,
                time: elapsed / 1000,
                tokens: response.usage?.total_tokens || 0,
                response_length: response.choices[0].message.content.length
            });

        } catch (error) {
            console.log(`${provider}: Error - ${error.message}`);
        }
    }

    // Display results
    console.log("\nBenchmark Results:");
    console.log("-".repeat(80));
    console.log(`${"Provider".padEnd(15)} ${"Model".padEnd(25)} ${"Time (s)".padEnd(12)} ${"Tokens".padEnd(10)} ${"Length".padEnd(10)}`);
    console.log("-".repeat(80));

    results.sort((a, b) => a.time - b.time).forEach(result => {
        console.log(
            `${result.provider.padEnd(15)} ${result.model.padEnd(25)} ` +
            `${result.time.toFixed(3).padEnd(12)} ${result.tokens.toString().padEnd(10)} ${result.response_length.toString().padEnd(10)}`
        );
    });

    console.log("-".repeat(80));

    // Find fastest
    if (results.length > 0) {
        const fastest = results[0];
        console.log(`\nFastest: ${fastest.provider} (${fastest.time.toFixed(3)}s)`);
    }
}

async function providerSpecificFeatures() {
    /**
     * Demonstrate provider-specific features
     */

    console.log("\n" + "=".repeat(60));
    console.log("Provider-Specific Features");
    console.log("=".repeat(60));

    // OpenAI: JSON mode
    console.log("\n1. OpenAI - JSON Mode:");
    console.log("-".repeat(60));
    try {
        const response = await client.chat.completions.create({
            model: "gpt-5-nano",
            messages: [
                {
                    role: "system",
                    content: "Return responses as JSON."
                },
                {
                    role: "user",
                    content: "Create a JSON object with name and age for a fictional person."
                }
            ],
            response_format: { type: "json_object" }
        });
        console.log(response.choices[0].message.content);
    } catch (error) {
        console.log(`Error: ${error.message}`);
    }

    // Anthropic: Long context
    console.log("\n\n2. Anthropic Claude - Long Context:");
    console.log("-".repeat(60));
    try {
        const longText = "AI ".repeat(500);  // Simulate longer context
        const response = await client.chat.completions.create({
            model: "claude-haiku-4-5",
            messages: [
                {
                    role: "user",
                    content: `Count how many times 'AI' appears in this text: ${longText}`
                }
            ],
            max_tokens: 8000
        });
        console.log(response.choices[0].message.content);
    } catch (error) {
        console.log(`Error: ${error.message}`);
    }

    // DeepSeek: Cost-effective
    console.log("\n\n3. DeepSeek - Cost-Effective Model:");
    console.log("-".repeat(60));
    try {
        const response = await client.chat.completions.create({
            model: "deepseek-chat",
            messages: [
                {
                    role: "user",
                    content: "What makes DeepSeek models cost-effective?"
                }
            ],
            max_tokens: 8000
        });
        console.log(response.choices[0].message.content);
        if (response.usage?.cost_rial) {
            console.log(`\nCost: ${response.usage.cost_rial} Rials`);
        }
    } catch (error) {
        console.log(`Error: ${error.message}`);
    }

    // Gemini: Fast & Efficient
    console.log("\n\n4. Google Gemini - Fast & Efficient:");
    console.log("-".repeat(60));
    try {
        const response = await client.chat.completions.create({
            model: "gemini-2.5-flash-lite",
            messages: [
                {
                    role: "user",
                    content: "List 3 advantages of Gemini models in one sentence each."
                }
            ],
            max_tokens: 8000
        });
        console.log(response.choices[0].message.content);
    } catch (error) {
        console.log(`Error: ${error.message}`);
    }
}

function chooseBestModelForTask() {
    /**
     * Recommendation system for choosing models
     */

    console.log("\n" + "=".repeat(60));
    console.log("Model Selection Guide");
    console.log("=".repeat(60));

    const recommendations = {
        "Speed & Cost": {
            model: "gpt-5-nano or deepseek-chat",
            reason: "Fast responses and low cost"
        },
        "Coding": {
            model: "deepseek-chat or gpt-5-nano",
            reason: "Strong programming capabilities"
        },
        "Creative Writing": {
            model: "claude-haiku-4-5 or gpt-5-nano",
            reason: "Excellent creative output"
        },
        "Long Context": {
            model: "claude-haiku-4-5",
            reason: "Large context window"
        },
        "Multilingual": {
            model: "gpt-5-nano or gemini-2.5-flash-lite",
            reason: "Good multilingual support"
        },
        "JSON Output": {
            model: "gpt-5-nano",
            reason: "Native JSON mode support"
        }
    };

    console.log("\nTask-Based Model Recommendations:\n");

    for (const [task, info] of Object.entries(recommendations)) {
        console.log(`${task}:`);
        console.log(`  ’ ${info.model}`);
        console.log(`  Reason: ${info.reason}`);
        console.log();
    }
}

// Run the examples
(async () => {
    try {
        // Example 1: Basic comparison
        await compareBasicResponses();

        // Example 2: Coding tasks
        // await compareCodingTasks();  // Uncomment to run

        // Example 3: Creative writing
        // await compareCreativeWriting();  // Uncomment to run

        // Example 4: Performance benchmark
        await benchmarkPerformance();

        // Example 5: Provider-specific features
        await providerSpecificFeatures();

        // Example 6: Model selection guide
        chooseBestModelForTask();

        console.log("\n" + "=".repeat(60));
        console.log("Multiple provider comparison completed!");
        console.log("=".repeat(60));

    } catch (error) {
        console.error(`\nError: ${error.message}`);
    }
})();
