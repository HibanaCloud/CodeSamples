package com.hibana.samples;

import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.models.ChatCompletionChunk;
import com.openai.models.ChatCompletionCreateParams;
import com.openai.models.ChatCompletionMessage;

import java.util.stream.Stream;

/**
 * 04 - Streaming Response
 *
 * This example demonstrates real-time streaming of AI responses using
 * Server-Sent Events (SSE). Instead of waiting for the complete response,
 * tokens are received and displayed progressively as they're generated.
 *
 * Model used: gpt-5-nano (Fast OpenAI model)
 */
public class Example04_StreamingResponse {

    private static final String API_KEY = "YOUR_API_KEY";
    private static final String BASE_URL = "https://api-ai.hibanacloud.com/v1";

    public static void main(String[] args) {
        try {
            streamingChat();
            System.out.println("\n");
            streamingWithMetadata();
            System.out.println("\n");
            compareStreamingVsNormal();
        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void streamingChat() {
        /**
         * Demonstrate streaming responses with real-time output
         */

        System.out.println("=".repeat(60));
        System.out.println("Streaming Response Demo");
        System.out.println("=".repeat(60));

        String userMessage = "Write a short story about a robot learning to paint. Make it about 150 words.";

        System.out.println("\nUser: " + userMessage);
        System.out.print("\nAssistant (streaming): ");

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        // Create streaming chat completion
        ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                .model("gpt-5-nano")
                .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                .content(userMessage)
                                .build()
                ))
                .temperature(1.0)
                .maxTokens(10000L)
                .stream(true)  // Enable streaming
                .build();

        Stream<ChatCompletionChunk> stream = client.chat().completions().createStreaming(params);

        // Collect the full response for later display
        StringBuilder fullResponse = new StringBuilder();

        // Process the stream
        stream.forEach(chunk -> {
            // Check if chunk has choices and content - IMPORTANT for avoiding errors
            if (chunk.choices() != null && !chunk.choices().isEmpty()) {
                chunk.choices().get(0).delta().content().ifPresent(content -> {
                    fullResponse.append(content);
                    // Print the content immediately (streaming effect)
                    System.out.print(content);
                    System.out.flush();
                });
            }
        });

        System.out.println("\n\n" + "=".repeat(60));
        System.out.println("Streaming complete!");
        System.out.println("=".repeat(60));
        System.out.println("\nTotal characters received: " + fullResponse.length());
    }

    private static void streamingWithMetadata() {
        /**
         * Streaming with detailed chunk inspection
         */

        System.out.println("=".repeat(60));
        System.out.println("Streaming with Metadata Inspection");
        System.out.println("=".repeat(60));

        String userMessage = "Explain quantum entanglement in 2 sentences.";

        System.out.println("\nUser: " + userMessage);
        System.out.println("\nStreaming response...\n");

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                .model("gemini-2.5-flash-lite")  // Using Gemini model
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

        StringBuilder fullText = new StringBuilder();
        final int[] chunkCount = {0};

        stream.forEach(chunk -> {
            chunkCount[0]++;

            // Inspect chunk structure
            if (chunk.choices() != null && !chunk.choices().isEmpty()) {
                var delta = chunk.choices().get(0).delta();

                // Check for content
                delta.content().ifPresent(content -> {
                    fullText.append(content);
                    System.out.print(content);
                    System.out.flush();
                });

                // Check for finish reason
                chunk.choices().get(0).finishReason().ifPresent(finishReason -> {
                    System.out.println("\n\nFinish reason: " + finishReason);
                });
            }
        });

        System.out.println("\n\nTotal chunks received: " + chunkCount[0]);
        System.out.println("Full response length: " + fullText.length() + " characters");
    }

    private static void compareStreamingVsNormal() {
        /**
         * Compare streaming vs non-streaming response times
         */

        System.out.println("=".repeat(60));
        System.out.println("Comparison: Streaming vs Non-Streaming");
        System.out.println("=".repeat(60));

        String message = "List 5 benefits of cloud computing.";

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        // Non-streaming
        System.out.println("\n1. NON-STREAMING:");
        long startTime = System.currentTimeMillis();

        ChatCompletionCreateParams params1 = ChatCompletionCreateParams.builder()
                .model("gpt-5-nano")
                .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                .content(message)
                                .build()
                ))
                .maxTokens(10000L)
                .stream(false)
                .build();

        var response = client.chat().completions().create(params1);
        long endTime = System.currentTimeMillis();

        System.out.printf("   Time to first output: %.2f seconds%n", (endTime - startTime) / 1000.0);
        System.out.println("   Response: " + response.choices().get(0).message().content().orElse("").substring(0, Math.min(100, response.choices().get(0).message().content().orElse("").length())) + "...");

        // Streaming
        System.out.println("\n2. STREAMING:");
        long startTimeStream = System.currentTimeMillis();
        final long[] firstChunkTime = {0};

        ChatCompletionCreateParams params2 = ChatCompletionCreateParams.builder()
                .model("gpt-5-nano")
                .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                .content(message)
                                .build()
                ))
                .maxTokens(10000L)
                .stream(true)
                .build();

        Stream<ChatCompletionChunk> stream = client.chat().completions().createStreaming(params2);

        final boolean[] first = {true};
        stream.forEach(chunk -> {
            if (chunk.choices() != null && !chunk.choices().isEmpty()) {
                chunk.choices().get(0).delta().content().ifPresent(content -> {
                    if (first[0]) {
                        firstChunkTime[0] = System.currentTimeMillis();
                        System.out.printf("   Time to first output: %.2f seconds%n", (firstChunkTime[0] - startTimeStream) / 1000.0);
                        System.out.println("   First chunk: " + content);
                        first[0] = false;
                    }
                });
            }
        });

        System.out.println("\n   Streaming provides faster time-to-first-token!");
    }
}
