package com.hibana.samples;

import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.models.ChatCompletion;
import com.openai.models.ChatCompletionCreateParams;
import com.openai.models.ChatCompletionMessage;

/**
 * 01 - Simple Chat Completion
 *
 * This example demonstrates the most basic usage of Hibana API
 * using the OpenAI Java SDK. It shows how to:
 * - Initialize the client with Hibana's base URL
 * - Send a simple chat completion request
 * - Receive and display the response
 *
 * Model used: gpt-5-nano (Fast and efficient GPT model)
 */
public class Example01_SimpleChat {

    // API Configuration
    private static final String API_KEY = "YOUR_API_KEY";
    private static final String BASE_URL = "https://api-ai.hibanacloud.com/v1";

    public static void main(String[] args) {
        try {
            simpleChat();
        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void simpleChat() {
        /**
         * Send a simple chat message and get a response
         */

        System.out.println("Sending message to gpt-5-nano...");

        // Initialize the Hibana client
        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        // Create a chat completion
        ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                .model("gpt-5-nano")  // Fast OpenAI model
                .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                .content("Hello! Please introduce yourself in one sentence.")
                                .build()
                ))
                .temperature(1.0)  // Controls randomness (0.0 = deterministic, 2.0 = very random)
                .maxTokens(10000L)    // Maximum tokens in the response
                .build();

        ChatCompletion response = client.chat().completions().create(params);

        // Extract the response
        String assistantMessage = response.choices().get(0).message().content().orElse("");
        String finishReason = response.choices().get(0).finishReason().toString();

        // Display the response
        System.out.println("\n" + "=".repeat(60));
        System.out.println("Response from gpt-5-nano:");
        System.out.println("=".repeat(60));
        System.out.println(assistantMessage);
        System.out.println("=".repeat(60));
        System.out.println("\nFinish reason: " + finishReason);

        // Display token usage
        response.usage().ifPresent(usage -> {
            System.out.println("\nToken Usage:");
            System.out.println("  Prompt tokens: " + usage.promptTokens());
            System.out.println("  Completion tokens: " + usage.completionTokens());
            System.out.println("  Total tokens: " + usage.totalTokens());
        });
    }
}
