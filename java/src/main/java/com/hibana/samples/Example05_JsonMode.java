package com.hibana.samples;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.models.ChatCompletion;
import com.openai.models.ChatCompletionCreateParams;
import com.openai.models.ChatCompletionMessage;
import com.openai.models.ResponseFormatJsonObject;

/**
 * 05 - JSON Mode
 *
 * This example demonstrates how to request structured JSON responses
 * from the API. JSON mode ensures the model outputs valid JSON,
 * making it perfect for applications that need parseable data.
 *
 * Model used: gpt-5-nano (with JSON mode support)
 */
public class Example05_JsonMode {

    private static final String API_KEY = "YOUR_API_KEY";
    private static final String BASE_URL = "https://api-ai.hibanacloud.com/v1";
    private static final Gson gson = new GsonBuilder().setPrettyPrinting().create();

    public static void main(String[] args) {
        try {
            basicJsonMode();
            System.out.println("\n");
            structuredDataExtraction();
            System.out.println("\n");
            jsonArrayResponse();
        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void basicJsonMode() {
        /**
         * Basic JSON mode example
         */

        System.out.println("=".repeat(60));
        System.out.println("Basic JSON Mode");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                .model("gpt-5-nano")
                .addMessage(ChatCompletionMessage.ofChatCompletionSystemMessageParam(
                        ChatCompletionMessage.ChatCompletionSystemMessageParam.builder()
                                .content("You are a helpful assistant that outputs JSON.")
                                .build()
                ))
                .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                .content("Create a JSON object with information about a fictional person (name, age, city).")
                                .build()
                ))
                .responseFormat(ResponseFormatJsonObject.builder().type(ResponseFormatJsonObject.Type.JSON_OBJECT).build())
                .temperature(1.0)
                .maxTokens(10000L)
                .build();

        ChatCompletion response = client.chat().completions().create(params);
        String jsonResponse = response.choices().get(0).message().content().orElse("");

        System.out.println("\nUser: Create a JSON object with information about a fictional person.");
        System.out.println("\nAssistant (JSON):");
        System.out.println(prettyPrintJson(jsonResponse));
    }

    private static void structuredDataExtraction() {
        /**
         * Extract structured data from text
         */

        System.out.println("=".repeat(60));
        System.out.println("Structured Data Extraction");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        String userMessage = """
                Extract information from this text into JSON format:

                "John Smith ordered 2 laptops and 1 mouse on January 15, 2025.
                Total cost was $2,500. Shipping address: 123 Main St, Boston, MA."

                Include: customer_name, items (array), order_date, total_cost, shipping_address
                """;

        ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                .model("gpt-5-nano")
                .addMessage(ChatCompletionMessage.ofChatCompletionSystemMessageParam(
                        ChatCompletionMessage.ChatCompletionSystemMessageParam.builder()
                                .content("Extract structured data and return as JSON.")
                                .build()
                ))
                .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                .content(userMessage)
                                .build()
                ))
                .responseFormat(ResponseFormatJsonObject.builder().type(ResponseFormatJsonObject.Type.JSON_OBJECT).build())
                .temperature(0.3)
                .maxTokens(10000L)
                .build();

        ChatCompletion response = client.chat().completions().create(params);
        String jsonResponse = response.choices().get(0).message().content().orElse("");

        System.out.println("\nExtracted Data:");
        System.out.println(prettyPrintJson(jsonResponse));
    }

    private static void jsonArrayResponse() {
        /**
         * Generate JSON array response
         */

        System.out.println("=".repeat(60));
        System.out.println("JSON Array Response");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        String userMessage = """
                Create a JSON object with a "languages" array containing 5 programming languages.
                Each language should have: name, year_created, paradigm (e.g., OOP, functional).
                """;

        ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
                .model("gpt-5-nano")
                .addMessage(ChatCompletionMessage.ofChatCompletionSystemMessageParam(
                        ChatCompletionMessage.ChatCompletionSystemMessageParam.builder()
                                .content("Return structured JSON data as requested.")
                                .build()
                ))
                .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
                        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
                                .content(userMessage)
                                .build()
                ))
                .responseFormat(ResponseFormatJsonObject.builder().type(ResponseFormatJsonObject.Type.JSON_OBJECT).build())
                .temperature(0.7)
                .maxTokens(10000L)
                .build();

        ChatCompletion response = client.chat().completions().create(params);
        String jsonResponse = response.choices().get(0).message().content().orElse("");

        System.out.println("\nProgramming Languages:");
        System.out.println(prettyPrintJson(jsonResponse));

        System.out.println("\n" + "=".repeat(60));
        System.out.println("JSON mode ensures valid, parseable output!");
        System.out.println("=".repeat(60));
    }

    private static String prettyPrintJson(String jsonString) {
        try {
            Object json = gson.fromJson(jsonString, Object.class);
            return gson.toJson(json);
        } catch (Exception e) {
            return jsonString;
        }
    }
}
