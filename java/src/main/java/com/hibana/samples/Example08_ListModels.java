package com.hibana.samples;

import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.models.models.Model;
import com.openai.models.models.ModelListPage;

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

    private static final String API_KEY = System.getenv().getOrDefault("HIBANA_API_KEY", "YOUR_API_KEY");
    private static final String BASE_URL = "https://api-ai.hibanacloud.ir/v1";

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
                "deepseek-v4-flash",
                "gemini-3.7-flash"
        };

        System.out.println("\nInspecting key models:\n");

        // Look each model up in the list (Hibana serves GET /v1/models, not /v1/models/{id})
        List<Model> available = client.models().list().data();

        for (String modelId : keyModels) {
            Model model = available.stream()
                    .filter(m -> m.id().equals(modelId))
                    .findFirst()
                    .orElse(null);

            if (model == null) {
                System.out.println(modelId + " is not available to this API key");
                System.out.println();
                continue;
            }

            System.out.println("-".repeat(60));
            System.out.println("Model: " + model.id());
            System.out.println("Object: " + model._object_());
            System.out.println("Created: " + model.created());
            System.out.println("Owned by: " + model.ownedBy());
            System.out.println("-".repeat(60));
            System.out.println();
        }

        System.out.println("=".repeat(60));
        System.out.println("Use these model IDs in your chat completions!");
        System.out.println("=".repeat(60));
    }
}
