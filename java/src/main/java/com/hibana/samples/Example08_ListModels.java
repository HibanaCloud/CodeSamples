package com.hibana.samples;

import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.models.Model;
import com.openai.models.ModelListPage;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 08 - List Models
 *
 * This example demonstrates how to retrieve the list of available
 * models from the Hibana API. It shows model information and
 * categorizes models by provider.
 */
public class Example08_ListModels {

    private static final String API_KEY = "YOUR_API_KEY";
    private static final String BASE_URL = "https://api-ai.hibanacloud.com/v1";

    public static void main(String[] args) {
        try {
            listAllModels();
            System.out.println("\n");
            categorizeModelsByProvider();
            System.out.println("\n");
            showModelDetails();
        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void listAllModels() {
        /**
         * List all available models
         */

        System.out.println("=".repeat(60));
        System.out.println("Available Models");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        System.out.println("\nFetching available models...\n");

        ModelListPage modelsPage = client.models().list();
        List<Model> models = modelsPage.data();

        System.out.println("Total models available: " + models.size());
        System.out.println("\n" + "-".repeat(60));

        for (Model model : models) {
            System.out.println("• " + model.id());
        }

        System.out.println("-".repeat(60));
    }

    private static void categorizeModelsByProvider() {
        /**
         * Categorize models by provider
         */

        System.out.println("=".repeat(60));
        System.out.println("Models by Provider");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        ModelListPage modelsPage = client.models().list();
        List<Model> models = modelsPage.data();

        // Categorize models
        Map<String, List<String>> providers = new HashMap<>();
        providers.put("OpenAI", new ArrayList<>());
        providers.put("Anthropic", new ArrayList<>());
        providers.put("DeepSeek", new ArrayList<>());
        providers.put("Google", new ArrayList<>());
        providers.put("Other", new ArrayList<>());

        for (Model model : models) {
            String modelId = model.id();
            if (modelId.contains("gpt") || modelId.contains("dall-e")) {
                providers.get("OpenAI").add(modelId);
            } else if (modelId.contains("claude")) {
                providers.get("Anthropic").add(modelId);
            } else if (modelId.contains("deepseek")) {
                providers.get("DeepSeek").add(modelId);
            } else if (modelId.contains("gemini")) {
                providers.get("Google").add(modelId);
            } else {
                providers.get("Other").add(modelId);
            }
        }

        // Display categorized models
        for (Map.Entry<String, List<String>> entry : providers.entrySet()) {
            if (!entry.getValue().isEmpty()) {
                System.out.println("\n" + entry.getKey() + " Models:");
                System.out.println("-".repeat(40));
                for (String modelId : entry.getValue()) {
                    System.out.println("  • " + modelId);
                }
            }
        }
    }

    private static void showModelDetails() {
        /**
         * Show detailed information for specific models
         */

        System.out.println("=".repeat(60));
        System.out.println("Model Details");
        System.out.println("=".repeat(60));

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .apiKey(API_KEY)
                .baseUrl(BASE_URL)
                .build();

        // Key models to inspect
        String[] keyModels = {
                "gpt-5-nano",
                "claude-haiku-4-5",
                "deepseek-chat",
                "gemini-2.5-flash-lite"
        };

        System.out.println("\nInspecting key models:\n");

        for (String modelId : keyModels) {
            try {
                Model model = client.models().retrieve(modelId);

                System.out.println("-".repeat(60));
                System.out.println("Model: " + model.id());
                System.out.println("Object: " + model.object());
                System.out.println("Created: " + model.created());
                System.out.println("Owned by: " + model.ownedBy());
                System.out.println("-".repeat(60));
                System.out.println();
            } catch (Exception e) {
                System.out.println("Could not retrieve details for: " + modelId);
                System.out.println();
            }
        }

        System.out.println("=".repeat(60));
        System.out.println("Use these model IDs in your chat completions!");
        System.out.println("=".repeat(60));
    }
}
