package com.hibana.samples;

import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.core.http.StreamResponse;
import com.openai.errors.*;
import com.openai.models.ChatCompletion;
import com.openai.models.ChatCompletionCreateParams;
import com.openai.models.ChatCompletionMessage;

/**
 * 10 - Error Handling
 *
 * This example demonstrates proper error handling when using the Hibana API.
 * It covers common errors, retry strategies, and best practices for building
 * robust applications.
 */
public class Example10_ErrorHandling {

    private static final String API_KEY = "YOUR_API_KEY";
    private static final String BASE_URL = "https://api-ai.hibanacloud.com/v1";

    public static void main(String[] args) {
        try {
            // handleAuthenticationError();  // Uncomment to test
            // handleModelNotFound();  // Uncomment to test
            // handleRateLimit();  // Uncomment to test
            handleInsufficientBalance();
            System.out.println("\n");
            retryWithExponentialBackoff();
            System.out.println("\n");
            comprehensiveErrorHandler();
            System.out.println("\n");
            validateBeforeRequest();
        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void handleAuthenticationError() {
        /**
         * Handle authentication errors (invalid API key)
         */

        System.out.println("=".repeat(60));
        System.out.println("Error Handling - Authentication");
        System.out.println("=".repeat(60));

        // Create client with invalid API key
        OpenAIClient badClient = OpenAIOkHttpClient.builder()
                .apiKey("INVALID_KEY_123")
                .baseUrl(BASE_URL)
                .build();

        try {
            ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                    .model("gpt-5-nano")
                    .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                            ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                    .content("Hello")
                                    .build()
                    ))
                    .build();

            badClient.chat().completions().create(params);

        } catch (AuthenticationError e) {
            System.out.println("\n✗ Authentication Error Caught!");
            System.out.println("Error message: " + e.getMessage());
            System.out.println("\nSolution:");
            System.out.println("  1. Check that your API key is correct");
            System.out.println("  2. Verify the key is active in your dashboard");
            System.out.println("  3. Ensure proper Bearer token format");
        }
    }

    private static void handleModelNotFound() {
        /**
         * Handle errors when requesting unavailable models
         */

        System.out.println("=".repeat(60));
        System.out.println("Error Handling - Model Not Found");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        try {
            ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                    .model("non-existent-model-xyz")
                    .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                            ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                    .content("Hello")
                                    .build()
                    ))
                    .build();

            client.chat().completions().create(params);

        } catch (NotFoundError | BadRequestError e) {
            System.out.println("\n✗ Model Not Found Error!");
            System.out.println("Error: " + e.getMessage());
            System.out.println("\nSolution:");
            System.out.println("  1. Use client.models().list() to see available models");
            System.out.println("  2. Check for typos in model name");
            System.out.println("  3. Verify model is enabled in your account");
        }
    }

    private static void handleRateLimit() {
        /**
         * Handle rate limiting errors
         */

        System.out.println("=".repeat(60));
        System.out.println("Error Handling - Rate Limiting");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        System.out.println("\nSimulating rate limit scenario...");

        try {
            for (int i = 0; i < 5; i++) {
                ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                        .model("gpt-5-nano")
                        .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                                ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                        .content("Request " + (i + 1))
                                        .build()
                        ))
                        .maxTokens(10000L)
                        .build();

                client.chat().completions().create(params);
                System.out.println("Request " + (i + 1) + ": Success");
                Thread.sleep(100);  // Small delay
            }

        } catch (RateLimitError e) {
            System.out.println("\n✗ Rate Limit Error: " + e.getMessage());
            System.out.println("\nSolution:");
            System.out.println("  1. Implement exponential backoff");
            System.out.println("  2. Add delays between requests");
            System.out.println("  3. Monitor rate limits for your tier");
            System.out.println("  4. Consider upgrading your plan");
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
    }

    private static void handleInsufficientBalance() {
        /**
         * Handle insufficient balance errors
         */

        System.out.println("=".repeat(60));
        System.out.println("Error Handling - Insufficient Balance");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        System.out.println("\nChecking for balance errors...");

        try {
            ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                    .model("gpt-5-nano")
                    .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                            ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                    .content("Test")
                                    .build()
                    ))
                    .maxTokens(10000L)
                    .build();

            client.chat().completions().create(params);
            System.out.println("✓ Request successful - sufficient balance");

        } catch (OpenAIException e) {
            if (e.getMessage().toLowerCase().contains("insufficient") ||
                    e.getMessage().toLowerCase().contains("balance")) {
                System.out.println("\n✗ Insufficient Balance: " + e.getMessage());
                System.out.println("\nSolution:");
                System.out.println("  1. Check balance: GET /v1/user/balance");
                System.out.println("  2. Recharge your account");
                System.out.println("  3. Use lower-cost models");
            }
        }
    }

    private static void retryWithExponentialBackoff() {
        /**
         * Implement retry logic with exponential backoff
         */

        System.out.println("=".repeat(60));
        System.out.println("Retry Strategy - Exponential Backoff");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        int maxRetries = 3;
        long baseDelay = 1000;  // milliseconds

        for (int attempt = 0; attempt < maxRetries; attempt++) {
            try {
                System.out.println("\nAttempt " + (attempt + 1) + "/" + maxRetries + "...");

                ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                        .model("gpt-5-nano")
                        .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                                ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                        .content("Hello")
                                        .build()
                        ))
                        .maxTokens(10000L)
                        .build();

                ChatCompletion response = client.chat().completions().create(params);

                System.out.println("✓ Success!");
                System.out.println("Response: " + response.choices().get(0).message().content().orElse(""));
                break;

            } catch (RateLimitError e) {
                if (attempt < maxRetries - 1) {
                    long delay = baseDelay * (long) Math.pow(2, attempt);
                    System.out.println("✗ Rate limited. Retrying in " + (delay / 1000) + " seconds...");
                    try {
                        Thread.sleep(delay);
                    } catch (InterruptedException ie) {
                        Thread.currentThread().interrupt();
                    }
                } else {
                    System.out.println("✗ Failed after " + maxRetries + " attempts");
                    throw e;
                }
            } catch (OpenAIException e) {
                if (attempt < maxRetries - 1) {
                    long delay = baseDelay * (long) Math.pow(2, attempt);
                    System.out.println("✗ API Error: " + e.getMessage());
                    System.out.println("Retrying in " + (delay / 1000) + " seconds...");
                    try {
                        Thread.sleep(delay);
                    } catch (InterruptedException ie) {
                        Thread.currentThread().interrupt();
                    }
                } else {
                    System.out.println("✗ Failed after " + maxRetries + " attempts");
                    throw e;
                }
            }
        }
    }

    private static void comprehensiveErrorHandler() {
        /**
         * A comprehensive error handling wrapper
         */

        System.out.println("=".repeat(60));
        System.out.println("Comprehensive Error Handler");
        System.out.println("=".repeat(60));

        System.out.println("\nTesting with valid request:");
        ChatCompletion result = safeApiCall("gpt-5-nano", "Say hello in 3 words");

        if (result != null) {
            System.out.println("✓ Success: " + result.choices().get(0).message().content().orElse(""));
        } else {
            System.out.println("Request failed gracefully");
        }
    }

    private static ChatCompletion safeApiCall(String model, String userMessage) {
        /**
         * Wrapper function with comprehensive error handling
         */

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        try {
            ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                    .model(model)
                    .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                            ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                    .content(userMessage)
                                    .build()
                    ))
                    .maxTokens(10000L)
                    .build();

            return client.chat().completions().create(params);

        } catch (AuthenticationError e) {
            System.out.println("✗ Authentication Error: " + e.getMessage());
            System.out.println("Check your API key.");
            return null;
        } catch (NotFoundError e) {
            System.out.println("✗ Not Found: " + e.getMessage());
            System.out.println("Model may not exist or be unavailable.");
            return null;
        } catch (RateLimitError e) {
            System.out.println("✗ Rate Limit: " + e.getMessage());
            System.out.println("Too many requests. Please wait and try again.");
            return null;
        } catch (BadRequestError e) {
            System.out.println("✗ Bad Request: " + e.getMessage());
            System.out.println("Check your request parameters.");
            return null;
        } catch (InternalServerError e) {
            System.out.println("✗ API Error: " + e.getMessage());
            System.out.println("An error occurred on the server side.");
            return null;
        } catch (OpenAIException e) {
            System.out.println("✗ Unexpected Error: " + e.getMessage());
            return null;
        }
    }

    private static void validateBeforeRequest() {
        /**
         * Validate inputs before making API request
         */

        System.out.println("=".repeat(60));
        System.out.println("Input Validation");
        System.out.println("=".repeat(60));

        // Test validation
        System.out.println("\nTest 1: Invalid model");
        validateAndCall("invalid-model", "Hello");

        System.out.println("\nTest 2: Empty input");
        validateAndCall("gpt-5-nano", "");

        System.out.println("\nTest 3: Valid request");
        ChatCompletion result = validateAndCall("gpt-5-nano", "Say hi");
        if (result != null) {
            System.out.println("Response: " + result.choices().get(0).message().content().orElse(""));
        }
    }

    private static ChatCompletion validateAndCall(String model, String userInput) {
        /**
         * Validate inputs before API call
         */

        // Validate model name
        String[] validModels = {"gpt-5-nano", "claude-haiku-4-5", "deepseek-chat", "gemini-2.5-flash-lite"};
        boolean validModel = false;
        for (String valid : validModels) {
            if (valid.equals(model)) {
                validModel = true;
                break;
            }
        }

        if (!validModel) {
            System.out.println("✗ Invalid model: " + model);
            System.out.println("Valid models: " + String.join(", ", validModels));
            return null;
        }

        // Validate user input
        if (userInput == null || userInput.trim().isEmpty()) {
            System.out.println("✗ Empty user input");
            return null;
        }

        if (userInput.length() > 10000) {
            System.out.println("✗ Input too long (max 10,000 characters)");
            return null;
        }

        // All validations passed
        System.out.println("✓ Validation passed");

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        try {
            ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                    .model(model)
                    .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                            ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                    .content(userInput)
                                    .build()
                    ))
                    .maxTokens(10000L)
                    .build();

            return client.chat().completions().create(params);
        } catch (OpenAIException e) {
            System.out.println("✗ Error: " + e.getMessage());
            return null;
        }
    }
}
