package com.hibana.samples;

import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.models.ChatCompletion;
import com.openai.models.ChatCompletionCreateParams;
import com.openai.models.ChatCompletionMessage;
import com.openai.models.ResponseFormatJsonObject;

import java.util.*;

/**
 * 11 - Multiple Providers Comparison
 *
 * This example demonstrates how to use all four LLM providers available
 * through Hibana API: OpenAI, Anthropic, DeepSeek, and Google Gemini.
 * Compare responses, performance, and features across providers.
 */
public class Example11_MultipleProviders {

    private static final String API_KEY = "YOUR_API_KEY";
    private static final String BASE_URL = "https://api-ai.hibanacloud.com/v1";

    private static final Map<String, String> MODELS = new LinkedHashMap<>();

    static {
        MODELS.put("OpenAI", "gpt-5-nano");
        MODELS.put("Anthropic", "claude-haiku-4-5");
        MODELS.put("DeepSeek", "deepseek-chat");
        MODELS.put("Google", "gemini-2.5-flash-lite");
    }

    public static void main(String[] args) {
        try {
            compareBasicResponses();
            System.out.println("\n");
            // compareCodingTasks();  // Uncomment to run
            // compareCreativeWriting();  // Uncomment to run
            benchmarkPerformance();
            System.out.println("\n");
            providerSpecificFeatures();
            System.out.println("\n");
            chooseBestModelForTask();

            System.out.println("\n" + "=".repeat(60));
            System.out.println("Multiple provider comparison completed!");
            System.out.println("=".repeat(60));

        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void compareBasicResponses() {
        /**
         * Compare responses from all four providers
         */

        System.out.println("=".repeat(60));
        System.out.println("Comparing All Providers");
        System.out.println("=".repeat(60));

        String question = "What is the future of artificial intelligence? Answer in 2 sentences.";
        System.out.println("\nQuestion: " + question + "\n");

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        MODELS.forEach((provider, model) -> {
            System.out.println("\n" + provider + " (" + model + "):");
            System.out.println("-".repeat(60));

            try {
                long startTime = System.currentTimeMillis();

                ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                        .model(model)
                        .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                                ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                        .content(question)
                                        .build()
                        ))
                        .temperature(0.7)
                        .maxTokens(8000L)
                        .build();

                ChatCompletion response = client.chat().completions().create(params);
                long elapsed = System.currentTimeMillis() - startTime;

                String answer = response.choices().get(0).message().content().orElse("");
                long tokens = response.usage().map(u -> u.totalTokens()).orElse(0L);

                System.out.println("Response: " + answer);
                System.out.printf("\nTime: %.2fs | Tokens: %d%n", elapsed / 1000.0, tokens);

            } catch (Exception e) {
                System.out.println("Error: " + e.getMessage());
            }
        });
    }

    private static void compareCodingTasks() {
        /**
         * Compare coding assistance from different providers
         */

        System.out.println("=".repeat(60));
        System.out.println("Coding Task Comparison");
        System.out.println("=".repeat(60));

        String codingQuestion = "Write a Java function to check if a string is a palindrome. Include comments.";
        System.out.println("\nTask: " + codingQuestion + "\n");

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        MODELS.forEach((provider, model) -> {
            System.out.println("\n" + provider + " (" + model + "):");
            System.out.println("-".repeat(60));

            try {
                ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                        .model(model)
                        .addMessage(ChatCompletionMessage.ofChatCompletionSystemMessageParam(
                                ChatCompletionMessage.ChatCompletionSystemMessageParam.builder()
                                        .content("You are a Java programming expert.")
                                        .build()
                        ))
                        .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                                ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                        .content(codingQuestion)
                                        .build()
                        ))
                        .temperature(0.3)  // Lower temp for coding
                        .maxTokens(8000L)
                        .build();

                ChatCompletion response = client.chat().completions().create(params);
                System.out.println(response.choices().get(0).message().content().orElse(""));

            } catch (Exception e) {
                System.out.println("Error: " + e.getMessage());
            }
        });
    }

    private static void compareCreativeWriting() {
        /**
         * Compare creative writing capabilities
         */

        System.out.println("=".repeat(60));
        System.out.println("Creative Writing Comparison");
        System.out.println("=".repeat(60));

        String prompt = "Write a two-line poem about technology and humanity.";
        System.out.println("\nPrompt: " + prompt + "\n");

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        MODELS.forEach((provider, model) -> {
            System.out.println("\n" + provider + " (" + model + "):");
            System.out.println("-".repeat(60));

            try {
                ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                        .model(model)
                        .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                                ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                        .content(prompt)
                                        .build()
                        ))
                        .temperature(1.0)  // Higher temp for creativity
                        .maxTokens(8000L)
                        .build();

                ChatCompletion response = client.chat().completions().create(params);
                System.out.println(response.choices().get(0).message().content().orElse(""));

            } catch (Exception e) {
                System.out.println("Error: " + e.getMessage());
            }
        });
    }

    private static void benchmarkPerformance() {
        /**
         * Benchmark response time and token usage
         */

        System.out.println("=".repeat(60));
        System.out.println("Performance Benchmark");
        System.out.println("=".repeat(60));

        String testMessage = "Explain quantum computing in one sentence.";
        List<BenchmarkResult> results = new ArrayList<>();

        System.out.println("\nTest message: " + testMessage + "\n");
        System.out.println("Running benchmark...\n");

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        MODELS.forEach((provider, model) -> {
            try {
                // Warm-up request (not counted)
                ChatCompletionCreateParams warmupParams = ChatCompletionCreateParams.builder()
                        .model(model)
                        .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                                ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                        .content("Hi")
                                        .build()
                        ))
                        .maxTokens(8000L)
                        .build();
                client.chat().completions().create(warmupParams);

                // Actual benchmark
                long start = System.currentTimeMillis();

                ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                        .model(model)
                        .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                                ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                        .content(testMessage)
                                        .build()
                        ))
                        .temperature(0.5)
                        .maxTokens(8000L)
                        .build();

                ChatCompletion response = client.chat().completions().create(params);
                long elapsed = System.currentTimeMillis() - start;

                results.add(new BenchmarkResult(
                        provider,
                        model,
                        elapsed / 1000.0,
                        response.usage().map(u -> u.totalTokens()).orElse(0L),
                        response.choices().get(0).message().content().orElse("").length()
                ));

            } catch (Exception e) {
                System.out.println(provider + ": Error - " + e.getMessage());
            }
        });

        // Display results
        System.out.println("\nBenchmark Results:");
        System.out.println("-".repeat(80));
        System.out.printf("%-15s %-25s %-12s %-10s %-10s%n", "Provider", "Model", "Time (s)", "Tokens", "Length");
        System.out.println("-".repeat(80));

        results.stream()
                .sorted(Comparator.comparingDouble(r -> r.time))
                .forEach(result -> System.out.printf("%-15s %-25s %-12.3f %-10d %-10d%n",
                        result.provider, result.model, result.time, result.tokens, result.responseLength));

        System.out.println("-".repeat(80));

        if (!results.isEmpty()) {
            BenchmarkResult fastest = results.stream()
                    .min(Comparator.comparingDouble(r -> r.time))
                    .orElse(null);
            if (fastest != null) {
                System.out.printf("\nFastest: %s (%.3fs)%n", fastest.provider, fastest.time);
            }
        }
    }

    private static void providerSpecificFeatures() {
        /**
         * Demonstrate provider-specific features
         */

        System.out.println("=".repeat(60));
        System.out.println("Provider-Specific Features");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        // OpenAI: JSON mode
        System.out.println("\n1. OpenAI - JSON Mode:");
        System.out.println("-".repeat(60));
        try {
            ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                    .model("gpt-5-nano")
                    .addMessage(ChatCompletionMessage.ofChatCompletionSystemMessageParam(
                            ChatCompletionMessage.ChatCompletionSystemMessageParam.builder()
                                    .content("Return responses as JSON.")
                                    .build()
                    ))
                    .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                            ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                    .content("Create a JSON object with name and age for a fictional person.")
                                    .build()
                    ))
                    .responseFormat(ResponseFormatJsonObject.builder().type(ResponseFormatJsonObject.Type.JSON_OBJECT).build())
                    .build();

            ChatCompletion response = client.chat().completions().create(params);
            System.out.println(response.choices().get(0).message().content().orElse(""));
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        }

        // Anthropic: Long context
        System.out.println("\n\n2. Anthropic Claude - Long Context:");
        System.out.println("-".repeat(60));
        try {
            String longText = "AI ".repeat(500);  // Simulate longer context
            ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                    .model("claude-haiku-4-5")
                    .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                            ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                    .content("Count how many times 'AI' appears in this text: " + longText)
                                    .build()
                    ))
                    .maxTokens(8000L)
                    .build();

            ChatCompletion response = client.chat().completions().create(params);
            System.out.println(response.choices().get(0).message().content().orElse(""));
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        }

        // DeepSeek: Cost-effective
        System.out.println("\n\n3. DeepSeek - Cost-Effective Model:");
        System.out.println("-".repeat(60));
        try {
            ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                    .model("deepseek-chat")
                    .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                            ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                    .content("What makes DeepSeek models cost-effective?")
                                    .build()
                    ))
                    .maxTokens(8000L)
                    .build();

            ChatCompletion response = client.chat().completions().create(params);
            System.out.println(response.choices().get(0).message().content().orElse(""));
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        }

        // Gemini: Fast & Efficient
        System.out.println("\n\n4. Google Gemini - Fast & Efficient:");
        System.out.println("-".repeat(60));
        try {
            ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                    .model("gemini-2.5-flash-lite")
                    .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                            ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                    .content("List 3 advantages of Gemini models in one sentence each.")
                                    .build()
                    ))
                    .maxTokens(8000L)
                    .build();

            ChatCompletion response = client.chat().completions().create(params);
            System.out.println(response.choices().get(0).message().content().orElse(""));
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        }
    }

    private static void chooseBestModelForTask() {
        /**
         * Recommendation system for choosing models
         */

        System.out.println("=".repeat(60));
        System.out.println("Model Selection Guide");
        System.out.println("=".repeat(60));

        Map<String, Recommendation> recommendations = new LinkedHashMap<>();
        recommendations.put("Speed & Cost", new Recommendation("gpt-5-nano or deepseek-chat", "Fast responses and low cost"));
        recommendations.put("Coding", new Recommendation("deepseek-chat or gpt-5-nano", "Strong programming capabilities"));
        recommendations.put("Creative Writing", new Recommendation("claude-haiku-4-5 or gpt-5-nano", "Excellent creative output"));
        recommendations.put("Long Context", new Recommendation("claude-haiku-4-5", "Large context window"));
        recommendations.put("Multilingual", new Recommendation("gpt-5-nano or gemini-2.5-flash-lite", "Good multilingual support"));
        recommendations.put("JSON Output", new Recommendation("gpt-5-nano", "Native JSON mode support"));

        System.out.println("\nTask-Based Model Recommendations:\n");

        recommendations.forEach((task, info) -> {
            System.out.println(task + ":");
            System.out.println("  → " + info.model);
            System.out.println("  Reason: " + info.reason);
            System.out.println();
        });
    }

    private static class BenchmarkResult {
        String provider;
        String model;
        double time;
        long tokens;
        int responseLength;

        BenchmarkResult(String provider, String model, double time, long tokens, int responseLength) {
            this.provider = provider;
            this.model = model;
            this.time = time;
            this.tokens = tokens;
            this.responseLength = responseLength;
        }
    }

    private static class Recommendation {
        String model;
        String reason;

        Recommendation(String model, String reason) {
            this.model = model;
            this.reason = reason;
        }
    }
}
