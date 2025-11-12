package com.hibana.samples;

import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.models.ChatCompletion;
import com.openai.models.ChatCompletionChunk;
import com.openai.models.ChatCompletionCreateParams;
import com.openai.models.ChatCompletionMessage;

import java.util.stream.Stream;

/**
 * 06 - Responses API (Simple)
 *
 * This example demonstrates basic usage of the chat completions API
 * with both streaming and non-streaming modes. Shows how to handle
 * responses and access metadata.
 *
 * Model used: gemini-2.5-flash-lite (Fast Google model)
 */
public class Example06_ResponsesApiSimple {

    private static final String API_KEY = "YOUR_API_KEY";
    private static final String BASE_URL = "https://api-ai.hibanacloud.com/v1";

    public static void main(String[] args) {
        try {
            responsesBasic();
            System.out.println("\n");
            responsesWithMetadata();
            System.out.println("\n");
            responsesStreaming();
        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void responsesBasic() {
        /**
         * Basic non-streaming response
         */

        System.out.println("=".repeat(60));
        System.out.println("Basic Response API");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                .model("gemini-2.5-flash-lite")
                .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                .content("What is the capital of France? Answer in one word.")
                                .build()
                ))
                .temperature(0.3)
                .maxTokens(10000L)
                .build();

        ChatCompletion response = client.chat().completions().create(params);

        System.out.println("\nUser: What is the capital of France?");
        System.out.println("Assistant: " + response.choices().get(0).message().content().orElse(""));
        System.out.println("\nFinish reason: " + response.choices().get(0).finishReason());
    }

    private static void responsesWithMetadata() {
        /**
         * Response with metadata inspection
         */

        System.out.println("=".repeat(60));
        System.out.println("Response with Metadata");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        String userMessage = "Write a haiku about programming.";

        ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                .model("gemini-2.5-flash-lite")
                .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                .content(userMessage)
                                .build()
                ))
                .temperature(1.0)
                .maxTokens(10000L)
                .build();

        ChatCompletion response = client.chat().completions().create(params);

        System.out.println("\nUser: " + userMessage);
        System.out.println("\n" + "-".repeat(60));
        System.out.println("Response Metadata:");
        System.out.println("-".repeat(60));
        System.out.println("Model: " + response.model());
        System.out.println("ID: " + response.id());
        System.out.println("Object: " + response.object());

        response.usage().ifPresent(usage -> {
            System.out.println("\nToken Usage:");
            System.out.println("  Prompt tokens: " + usage.promptTokens());
            System.out.println("  Completion tokens: " + usage.completionTokens());
            System.out.println("  Total tokens: " + usage.totalTokens());
        });

        System.out.println("\n" + "-".repeat(60));
        System.out.println("Assistant Response:");
        System.out.println("-".repeat(60));
        System.out.println(response.choices().get(0).message().content().orElse(""));
    }

    private static void responsesStreaming() {
        /**
         * Streaming response with chunk inspection
         */

        System.out.println("=".repeat(60));
        System.out.println("Streaming Response");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        String userMessage = "Count from 1 to 10 with a brief comment for each number.";

        System.out.println("\nUser: " + userMessage);
        System.out.println("\nAssistant (streaming):");
        System.out.println("-".repeat(60));

        ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                .model("gemini-2.5-flash-lite")
                .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                .content(userMessage)
                                .build()
                ))
                .temperature(0.7)
                .maxTokens(10000L)
                .stream(true)
                .build();

        Stream<ChatCompletionChunk> stream = client.chat().completions().createStreaming(params);

        final int[] chunkCount = {0};
        StringBuilder fullResponse = new StringBuilder();

        stream.forEach(chunk -> {
            chunkCount[0]++;

            // Check if chunk has choices and content - IMPORTANT for avoiding errors
            if (chunk.choices() != null && !chunk.choices().isEmpty()) {
                chunk.choices().get(0).delta().content().ifPresent(content -> {
                    fullResponse.append(content);
                    System.out.print(content);
                    System.out.flush();
                });

                chunk.choices().get(0).finishReason().ifPresent(finishReason -> {
                    System.out.println("\n\n" + "-".repeat(60));
                    System.out.println("Stream complete!");
                    System.out.println("Finish reason: " + finishReason);
                    System.out.println("Total chunks: " + chunkCount[0]);
                    System.out.println("Total characters: " + fullResponse.length());
                    System.out.println("-".repeat(60));
                });
            }
        });
    }
}
