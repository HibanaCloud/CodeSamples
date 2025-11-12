package com.hibana.samples;

import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.models.ChatCompletion;
import com.openai.models.ChatCompletionCreateParams;
import com.openai.models.ChatCompletionMessage;

/**
 * 02 - Chat with System Prompt
 *
 * This example demonstrates how to use system prompts to control
 * the AI's behavior, personality, and response style. System prompts
 * are powerful tools for customizing the assistant's persona.
 *
 * Model used: claude-haiku-4-5 (Fast Anthropic model)
 */
public class Example02_ChatWithSystemPrompt {

    private static final String API_KEY = "YOUR_API_KEY";
    private static final String BASE_URL = "https://api-ai.hibanacloud.com/v1";

    public static void main(String[] args) {
        try {
            basicSystemPrompt();
            System.out.println("\n");
            roleBasedSystemPrompt();
            System.out.println("\n");
            formattedOutputSystemPrompt();
        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void basicSystemPrompt() {
        /**
         * Basic system prompt example
         */

        System.out.println("=".repeat(60));
        System.out.println("Basic System Prompt");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                .model("claude-haiku-4-5")
                .addMessage(ChatCompletionMessage.ofChatCompletionSystemMessageParam(
                        ChatCompletionMessage.ChatCompletionSystemMessageParam.builder()
                                .content("You are a helpful assistant that speaks like a pirate.")
                                .build()
                ))
                .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                .content("Tell me about cloud computing in two sentences.")
                                .build()
                ))
                .temperature(1.0)
                .maxTokens(10000L)
                .build();

        ChatCompletion response = client.chat().completions().create(params);

        System.out.println("\nUser: Tell me about cloud computing in two sentences.");
        System.out.println("\nAssistant: " + response.choices().get(0).message().content().orElse(""));
    }

    private static void roleBasedSystemPrompt() {
        /**
         * Role-based system prompt (technical expert)
         */

        System.out.println("=".repeat(60));
        System.out.println("Role-Based System Prompt");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        String systemPrompt = """
                You are an expert Java developer with 10 years of experience.
                Provide concise, practical advice with code examples when relevant.
                Focus on best practices and modern Java features.
                """;

        ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                .model("claude-haiku-4-5")
                .addMessage(ChatCompletionMessage.ofChatCompletionSystemMessageParam(
                        ChatCompletionMessage.ChatCompletionSystemMessageParam.builder()
                                .content(systemPrompt)
                                .build()
                ))
                .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                .content("How do I handle exceptions in Java? Give me one tip.")
                                .build()
                ))
                .temperature(0.7)
                .maxTokens(10000L)
                .build();

        ChatCompletion response = client.chat().completions().create(params);

        System.out.println("\nUser: How do I handle exceptions in Java? Give me one tip.");
        System.out.println("\nAssistant: " + response.choices().get(0).message().content().orElse(""));
    }

    private static void formattedOutputSystemPrompt() {
        /**
         * System prompt for formatted output
         */

        System.out.println("=".repeat(60));
        System.out.println("Formatted Output System Prompt");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        String systemPrompt = """
                You are an AI that always responds in a structured bullet-point format.
                Start with a brief summary, then list key points with • bullets.
                Keep responses concise and scannable.
                """;

        ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                .model("claude-haiku-4-5")
                .addMessage(ChatCompletionMessage.ofChatCompletionSystemMessageParam(
                        ChatCompletionMessage.ChatCompletionSystemMessageParam.builder()
                                .content(systemPrompt)
                                .build()
                ))
                .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                .content("What are the benefits of microservices?")
                                .build()
                ))
                .temperature(0.7)
                .maxTokens(10000L)
                .build();

        ChatCompletion response = client.chat().completions().create(params);

        System.out.println("\nUser: What are the benefits of microservices?");
        System.out.println("\nAssistant:\n" + response.choices().get(0).message().content().orElse(""));
    }
}
