package com.hibana.samples;

import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.models.ChatCompletion;
import com.openai.models.ChatCompletionCreateParams;
import com.openai.models.ChatCompletionMessage;

import java.util.ArrayList;
import java.util.List;

/**
 * 03 - Multi-Turn Conversation
 *
 * This example demonstrates how to maintain context across multiple
 * conversation turns. Each message in the conversation is sent to the
 * API, allowing the model to remember previous exchanges.
 *
 * Model used: deepseek-chat (Cost-effective DeepSeek model)
 */
public class Example03_MultiTurnConversation {

    private static final String API_KEY = "YOUR_API_KEY";
    private static final String BASE_URL = "https://api-ai.hibanacloud.com/v1";

    public static void main(String[] args) {
        try {
            multiTurnConversation();
            System.out.println("\n");
            conversationWithContext();
        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void multiTurnConversation() {
        /**
         * Basic multi-turn conversation
         */

        System.out.println("=".repeat(60));
        System.out.println("Multi-Turn Conversation Demo");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        // Conversation history
        List<ChatCompletionMessage> messages = new ArrayList<>();

        // Turn 1
        messages.add(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                        .content("Hi! My name is Alex.")
                        .build()
        ));

        ChatCompletionCreateParams params1 = ChatCompletionCreateParams.builder()
                .model("deepseek-chat")
                .addAllMessages(messages)
                .temperature(1.0)
                .maxTokens(8192L)
                .build();

        ChatCompletion response1 = client.chat().completions().create(params1);
        String assistantReply1 = response1.choices().get(0).message().content().orElse("");

        messages.add(ChatCompletionMessage.ofChatCompletionAssistantMessageParam(
                ChatCompletionMessage.ChatCompletionAssistantMessageParam.builder()
                        .content(assistantReply1)
                        .build()
        ));

        System.out.println("\nTurn 1:");
        System.out.println("User: Hi! My name is Alex.");
        System.out.println("Assistant: " + assistantReply1);

        // Turn 2
        messages.add(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                        .content("What's my name?")
                        .build()
        ));

        ChatCompletionCreateParams params2 = ChatCompletionCreateParams.builder()
                .model("deepseek-chat")
                .addAllMessages(messages)
                .temperature(1.0)
                .maxTokens(8192L)
                .build();

        ChatCompletion response2 = client.chat().completions().create(params2);
        String assistantReply2 = response2.choices().get(0).message().content().orElse("");

        System.out.println("\nTurn 2:");
        System.out.println("User: What's my name?");
        System.out.println("Assistant: " + assistantReply2);
    }

    private static void conversationWithContext() {
        /**
         * Longer conversation with context
         */

        System.out.println("=".repeat(60));
        System.out.println("Conversation with Context");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        List<ChatCompletionMessage> messages = new ArrayList<>();

        // System message for context
        messages.add(ChatCompletionMessage.ofChatCompletionSystemMessageParam(
                ChatCompletionMessage.ChatCompletionSystemMessageParam.builder()
                        .content("You are a helpful coding tutor.")
                        .build()
        ));

        // Define conversation turns
        String[][] turns = {
                {"User", "I want to learn about sorting algorithms."},
                {"User", "Start with bubble sort. Explain it in simple terms."},
                {"User", "What's the time complexity?"}
        };

        for (String[] turn : turns) {
            messages.add(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                    ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                            .content(turn[1])
                            .build()
            ));

            ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                    .model("deepseek-chat")
                    .addAllMessages(messages)
                    .temperature(0.7)
                    .maxTokens(8192L)
                    .build();

            ChatCompletion response = client.chat().completions().create(params);
            String assistantReply = response.choices().get(0).message().content().orElse("");

            messages.add(ChatCompletionMessage.ofChatCompletionAssistantMessageParam(
                    ChatCompletionMessage.ChatCompletionAssistantMessageParam.builder()
                            .content(assistantReply)
                            .build()
            ));

            System.out.println("\n" + turn[0] + ": " + turn[1]);
            System.out.println("Assistant: " + assistantReply);
        }

        System.out.println("\n" + "=".repeat(60));
        System.out.println("Conversation complete! Context was maintained across all turns.");
        System.out.println("=".repeat(60));
    }
}
