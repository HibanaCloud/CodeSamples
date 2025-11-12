/**
 * 10 - Error Handling
 *
 * This example demonstrates proper error handling when using the Hibana API.
 * It covers common errors, retry strategies, and best practices for building
 * robust applications.
 */

import OpenAI from 'openai';

// Initialize the Hibana client
const client = new OpenAI({
    apiKey: "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.com/v1"
});

async function handleAuthenticationError() {
    /**
     * Handle authentication errors (invalid API key)
     */

    console.log("=".repeat(60));
    console.log("Error Handling - Authentication");
    console.log("=".repeat(60));

    // Create client with invalid API key
    const badClient = new OpenAI({
        apiKey: "INVALID_KEY_123",
        baseURL: "https://api-ai.hibanacloud.com/v1"
    });

    try {
        await badClient.chat.completions.create({
            model: "gpt-5-nano",
            messages: [{ role: "user", content: "Hello" }]
        });
    } catch (error) {
        if (error.status === 401) {
            console.log("\n Authentication Error Caught!");
            console.log(`Error message: ${error.message}`);
            console.log("\nSolution:");
            console.log("  1. Check that your API key is correct");
            console.log("  2. Verify the key is active in your dashboard");
            console.log("  3. Ensure proper Bearer token format");
        }
    }
}

async function handleModelNotFound() {
    /**
     * Handle errors when requesting unavailable models
     */

    console.log("\n" + "=".repeat(60));
    console.log("Error Handling - Model Not Found");
    console.log("=".repeat(60));

    try {
        await client.chat.completions.create({
            model: "non-existent-model-xyz",
            messages: [{ role: "user", content: "Hello" }]
        });
    } catch (error) {
        if (error.status === 404 || error.message.includes('model')) {
            console.log("\n Model Not Found Error!");
            console.log(`Error: ${error.message}`);
            console.log("\nSolution:");
            console.log("  1. Use client.models.list() to see available models");
            console.log("  2. Check for typos in model name");
            console.log("  3. Verify model is enabled in your account");
        }
    }
}

async function handleRateLimit() {
    /**
     * Handle rate limiting errors
     */

    console.log("\n" + "=".repeat(60));
    console.log("Error Handling - Rate Limiting");
    console.log("=".repeat(60));

    console.log("\nSimulating rate limit scenario...");

    try {
        // This might trigger rate limiting if called too frequently
        for (let i = 0; i < 5; i++) {
            await client.chat.completions.create({
                model: "gpt-5-nano",
                messages: [{ role: "user", content: `Request ${i + 1}` }],
                max_tokens: 10000
            });
            console.log(`Request ${i + 1}: Success`);
            await new Promise(resolve => setTimeout(resolve, 100));  // Small delay
        }

    } catch (error) {
        if (error.status === 429) {
            console.log(`\n Rate Limit Error: ${error.message}`);
            console.log("\nSolution:");
            console.log("  1. Implement exponential backoff");
            console.log("  2. Add delays between requests");
            console.log("  3. Monitor rate limits for your tier");
            console.log("  4. Consider upgrading your plan");
        }
    }
}

async function handleInsufficientBalance() {
    /**
     * Handle insufficient balance errors
     */

    console.log("\n" + "=".repeat(60));
    console.log("Error Handling - Insufficient Balance");
    console.log("=".repeat(60));

    console.log("\nChecking for balance errors...");

    try {
        // This would fail if balance is insufficient
        const response = await client.chat.completions.create({
            model: "gpt-5-nano",
            messages: [{ role: "user", content: "Test" }],
            max_tokens: 10000
        });
        console.log(" Request successful - sufficient balance");

    } catch (error) {
        if (error.message.toLowerCase().includes('insufficient') ||
            error.message.toLowerCase().includes('balance')) {
            console.log(`\n Insufficient Balance: ${error.message}`);
            console.log("\nSolution:");
            console.log("  1. Check balance: GET /v1/user/balance");
            console.log("  2. Recharge your account");
            console.log("  3. Use lower-cost models");
        }
    }
}

async function retryWithExponentialBackoff() {
    /**
     * Implement retry logic with exponential backoff
     */

    console.log("\n" + "=".repeat(60));
    console.log("Retry Strategy - Exponential Backoff");
    console.log("=".repeat(60));

    const maxRetries = 3;
    const baseDelay = 1000;  // milliseconds

    for (let attempt = 0; attempt < maxRetries; attempt++) {
        try {
            console.log(`\nAttempt ${attempt + 1}/${maxRetries}...`);

            const response = await client.chat.completions.create({
                model: "gpt-5-nano",
                messages: [{ role: "user", content: "Hello" }],
                max_tokens: 10000
            });

            console.log(" Success!");
            console.log(`Response: ${response.choices[0].message.content}`);
            break;

        } catch (error) {
            if (error.status === 429 && attempt < maxRetries - 1) {
                const delay = baseDelay * Math.pow(2, attempt);  // Exponential backoff
                console.log(` Rate limited. Retrying in ${delay / 1000} seconds...`);
                await new Promise(resolve => setTimeout(resolve, delay));
            } else if (attempt < maxRetries - 1) {
                console.log(` API Error: ${error.message}`);
                const delay = baseDelay * Math.pow(2, attempt);
                console.log(`Retrying in ${delay / 1000} seconds...`);
                await new Promise(resolve => setTimeout(resolve, delay));
            } else {
                console.log(` Failed after ${maxRetries} attempts`);
                throw error;
            }
        }
    }
}

async function handleTimeout() {
    /**
     * Handle request timeout errors
     */

    console.log("\n" + "=".repeat(60));
    console.log("Error Handling - Timeouts");
    console.log("=".repeat(60));

    // Set a short timeout for demonstration
    const timeoutClient = new OpenAI({
        apiKey: "YOUR_API_KEY",
        baseURL: "https://api-ai.hibanacloud.com/v1",
        timeout: 1  // Very short timeout (will likely fail)
    });

    try {
        await timeoutClient.chat.completions.create({
            model: "gpt-5-nano",
            messages: [{ role: "user", content: "Hello" }]
        });
    } catch (error) {
        if (error.code === 'ETIMEDOUT' || error.message.includes('timeout')) {
            console.log(`\n Timeout Error: ${error.message}`);
            console.log("\nSolution:");
            console.log("  1. Increase timeout value");
            console.log("  2. Check network connection");
            console.log("  3. Try again later if server is slow");
        }
    }
}

async function comprehensiveErrorHandler() {
    /**
     * A comprehensive error handling wrapper
     */

    console.log("\n" + "=".repeat(60));
    console.log("Comprehensive Error Handler");
    console.log("=".repeat(60));

    async function safeApiCall(model, messages, options = {}) {
        /**
         * Wrapper function with comprehensive error handling
         */

        try {
            const response = await client.chat.completions.create({
                model,
                messages,
                ...options
            });
            return response;

        } catch (error) {
            if (error.status === 401) {
                console.log(` Authentication Error: ${error.message}`);
                console.log("Check your API key.");
                return null;
            } else if (error.status === 404) {
                console.log(` Not Found: ${error.message}`);
                console.log("Model may not exist or be unavailable.");
                return null;
            } else if (error.status === 429) {
                console.log(` Rate Limit: ${error.message}`);
                console.log("Too many requests. Please wait and try again.");
                return null;
            } else if (error.code === 'ETIMEDOUT') {
                console.log(` Timeout: ${error.message}`);
                console.log("Request took too long. Try again.");
                return null;
            } else if (error.status === 400) {
                console.log(` Bad Request: ${error.message}`);
                console.log("Check your request parameters.");
                return null;
            } else if (error.status >= 500) {
                console.log(` API Error: ${error.message}`);
                console.log("An error occurred on the server side.");
                return null;
            } else {
                console.log(` Unexpected Error: ${error.message}`);
                return null;
            }
        }
    }

    // Test the wrapper
    console.log("\nTesting with valid request:");
    const result = await safeApiCall(
        "gpt-5-nano",
        [{ role: "user", content: "Say hello in 3 words" }],
        { max_tokens: 10000 }
    );

    if (result) {
        console.log(` Success: ${result.choices[0].message.content}`);
    } else {
        console.log("Request failed gracefully");
    }
}

async function validateBeforeRequest() {
    /**
     * Validate inputs before making API request
     */

    console.log("\n" + "=".repeat(60));
    console.log("Input Validation");
    console.log("=".repeat(60));

    async function validateAndCall(model, userInput) {
        /**
         * Validate inputs before API call
         */

        // Validate model name
        const validModels = ["gpt-5-nano", "claude-haiku-4-5", "deepseek-chat", "gemini-2.5-flash-lite"];

        if (!validModels.includes(model)) {
            console.log(` Invalid model: ${model}`);
            console.log(`Valid models: ${validModels.join(', ')}`);
            return null;
        }

        // Validate user input
        if (!userInput || !userInput.trim()) {
            console.log(" Empty user input");
            return null;
        }

        if (userInput.length > 10000) {
            console.log(" Input too long (max 10,000 characters)");
            return null;
        }

        // All validations passed
        console.log(" Validation passed");

        try {
            const response = await client.chat.completions.create({
                model,
                messages: [{ role: "user", content: userInput }],
                max_tokens: 10000
            });
            return response;
        } catch (error) {
            console.log(` Error: ${error.message}`);
            return null;
        }
    }

    // Test validation
    console.log("\nTest 1: Invalid model");
    await validateAndCall("invalid-model", "Hello");

    console.log("\nTest 2: Empty input");
    await validateAndCall("gpt-5-nano", "");

    console.log("\nTest 3: Valid request");
    const result = await validateAndCall("gpt-5-nano", "Say hi");
    if (result) {
        console.log(`Response: ${result.choices[0].message.content}`);
    }
}

// Run the examples
(async () => {
    try {
        // Example 1: Authentication errors
        // await handleAuthenticationError();  // Uncomment to test

        // Example 2: Model not found
        // await handleModelNotFound();  // Uncomment to test

        // Example 3: Rate limiting
        // await handleRateLimit();  // Uncomment to test

        // Example 4: Insufficient balance
        await handleInsufficientBalance();

        // Example 5: Retry with backoff
        await retryWithExponentialBackoff();

        // Example 6: Timeout handling
        // await handleTimeout();  // Uncomment to test

        // Example 7: Comprehensive handler
        await comprehensiveErrorHandler();

        // Example 8: Input validation
        await validateBeforeRequest();

        console.log("\n" + "=".repeat(60));
        console.log("Error handling examples completed!");
        console.log("=".repeat(60));

    } catch (error) {
        console.error(`\nUnexpected error in main: ${error.message}`);
    }
})();
