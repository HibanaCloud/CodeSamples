package com.hibana.samples;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;

import java.io.IOException;
import java.text.NumberFormat;
import java.util.Locale;

/**
 * 09 - Check Balance
 *
 * This example demonstrates how to check your wallet balance using
 * the Hibana API. This endpoint is not part of the standard OpenAI API,
 * so we use OkHttp to call it directly.
 *
 * Endpoint: GET /v1/user/balance
 */
public class Example09_CheckBalance {

    private static final String API_KEY = "YOUR_API_KEY";
    private static final String BASE_URL = "https://api-ai.hibanacloud.com/v1";
    private static final Gson gson = new Gson();
    private static final OkHttpClient httpClient = new OkHttpClient();

    public static void main(String[] args) {
        try {
            checkBalance();
            System.out.println("\n");
            checkBalanceWithDetails();
            System.out.println("\n");
            monitorBalanceBeforeRequest();
            System.out.println("\n");
            balanceCheckWithUsageHistory();
        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static void checkBalance() throws IOException {
        /**
         * Check current wallet balance
         */

        System.out.println("=".repeat(60));
        System.out.println("Check Wallet Balance");
        System.out.println("=".repeat(60));

        String url = BASE_URL + "/user/balance";

        Request request = new Request.Builder()
                .url(url)
                .addHeader("Authorization", "Bearer " + API_KEY)
                .addHeader("Content-Type", "application/json")
                .get()
                .build();

        System.out.println("\nFetching balance...");

        try (Response response = httpClient.newCall(request).execute()) {
            if (response.isSuccessful() && response.body() != null) {
                String responseBody = response.body().string();
                JsonObject data = gson.fromJson(responseBody, JsonObject.class);

                System.out.println("\n" + "=".repeat(60));
                System.out.println("Balance Information");
                System.out.println("=".repeat(60));

                long balance = data.has("balance") ? data.get("balance").getAsLong() : 0;
                String currency = data.has("currency") ? data.get("currency").getAsString() : "IRR";

                NumberFormat formatter = NumberFormat.getInstance(Locale.US);
                System.out.println("\nCurrent Balance: " + formatter.format(balance) + " " + currency);

                if (currency.equals("IRR")) {
                    System.out.println("Formatted: " + formatter.format(balance) + " Rials");
                }

                // Estimate usage capability
                long avgCostPerRequest = 100;  // Example: 100 Rials per request
                long estimatedRequests = balance / avgCostPerRequest;

                System.out.println("\nEstimated remaining requests: ~" + formatter.format(estimatedRequests));
                System.out.println("(Based on average cost, actual may vary)");
            } else {
                System.out.println("\nError: " + response.code());
                if (response.body() != null) {
                    System.out.println("Message: " + response.body().string());
                }
            }
        }
    }

    private static void checkBalanceWithDetails() throws IOException {
        /**
         * Check balance with additional error handling and details
         */

        System.out.println("=".repeat(60));
        System.out.println("Detailed Balance Check");
        System.out.println("=".repeat(60));

        String url = BASE_URL + "/user/balance";

        Request request = new Request.Builder()
                .url(url)
                .addHeader("Authorization", "Bearer " + API_KEY)
                .addHeader("Content-Type", "application/json")
                .get()
                .build();

        try (Response response = httpClient.newCall(request).execute()) {
            System.out.println("\nStatus Code: " + response.code());

            if (response.isSuccessful() && response.body() != null) {
                String responseBody = response.body().string();
                JsonObject data = gson.fromJson(responseBody, JsonObject.class);

                System.out.println("\nFull Response:");
                System.out.println("-".repeat(60));

                // Display all fields in response
                data.entrySet().forEach(entry -> {
                    System.out.println(entry.getKey() + ": " + entry.getValue());
                });

                System.out.println("-".repeat(60));

                // Check balance status
                long balance = data.has("balance") ? data.get("balance").getAsLong() : 0;

                String status;
                if (balance > 10000) {
                    status = "✓ Healthy balance";
                } else if (balance > 1000) {
                    status = "⚠ Low balance";
                } else {
                    status = "✗ Critical - Please recharge";
                }

                System.out.println("\nStatus: " + status);
            }
        } catch (IOException e) {
            System.out.println("\n✗ Error: " + e.getMessage());
        }
    }

    private static void monitorBalanceBeforeRequest() throws IOException {
        /**
         * Example: Check balance before making an API call
         */

        System.out.println("=".repeat(60));
        System.out.println("Pre-Request Balance Check");
        System.out.println("=".repeat(60));

        String url = BASE_URL + "/user/balance";

        Request request = new Request.Builder()
                .url(url)
                .addHeader("Authorization", "Bearer " + API_KEY)
                .addHeader("Content-Type", "application/json")
                .get()
                .build();

        try (Response response = httpClient.newCall(request).execute()) {
            if (response.isSuccessful() && response.body() != null) {
                String responseBody = response.body().string();
                JsonObject data = gson.fromJson(responseBody, JsonObject.class);

                long balance = data.has("balance") ? data.get("balance").getAsLong() : 0;

                NumberFormat formatter = NumberFormat.getInstance(Locale.US);
                System.out.println("\nCurrent balance: " + formatter.format(balance) + " Rials");

                // Set minimum balance threshold
                long minimumBalance = 1000;  // 1,000 Rials

                if (balance >= minimumBalance) {
                    System.out.println("✓ Sufficient balance to proceed with API call");
                    System.out.println("\nProceed with API request...");
                } else {
                    System.out.println("✗ Insufficient balance");
                    System.out.println("Minimum required: " + formatter.format(minimumBalance) + " Rials");
                    System.out.println("Please recharge your account before making requests.");
                }
            }
        } catch (IOException e) {
            System.out.println("Could not check balance: " + e.getMessage());
        }
    }

    private static void balanceCheckWithUsageHistory() throws IOException {
        /**
         * Check balance and provide usage recommendations
         */

        System.out.println("=".repeat(60));
        System.out.println("Balance with Usage Recommendations");
        System.out.println("=".repeat(60));

        String url = BASE_URL + "/user/balance";

        Request request = new Request.Builder()
                .url(url)
                .addHeader("Authorization", "Bearer " + API_KEY)
                .addHeader("Content-Type", "application/json")
                .get()
                .build();

        try (Response response = httpClient.newCall(request).execute()) {
            if (response.isSuccessful() && response.body() != null) {
                String responseBody = response.body().string();
                JsonObject data = gson.fromJson(responseBody, JsonObject.class);

                long balance = data.has("balance") ? data.get("balance").getAsLong() : 0;

                NumberFormat formatter = NumberFormat.getInstance(Locale.US);
                System.out.println("\nCurrent Balance: " + formatter.format(balance) + " Rials\n");

                // Model cost estimates (example values)
                Map<String, Long> modelCosts = Map.of(
                        "gpt-5-nano", 50L,
                        "claude-haiku-4-5", 75L,
                        "deepseek-chat", 40L,
                        "gemini-2.5-flash-lite", 45L,
                        "dall-e-3", 5000L
                );

                System.out.println("Estimated requests per model:");
                System.out.println("-".repeat(60));

                modelCosts.forEach((model, cost) -> {
                    long requestsPossible = balance / cost;
                    System.out.printf("%-25s ~%6s requests%n", model, formatter.format(requestsPossible));
                });

                System.out.println("-".repeat(60));

                // Recommendations
                System.out.println("\nRecommendations:");
                if (balance < 5000) {
                    System.out.println("  • Consider recharging soon");
                    System.out.println("  • Use cost-efficient models (gpt-5-nano, deepseek-chat)");
                } else if (balance < 20000) {
                    System.out.println("  • Balance is moderate");
                    System.out.println("  • Monitor usage regularly");
                } else {
                    System.out.println("  • Balance is healthy");
                    System.out.println("  • You can use any model freely");
                }
            }
        } catch (IOException e) {
            System.out.println("Error checking balance: " + e.getMessage());
        }
    }

    private static class Map<K, V> extends HashMap<K, V> {
        public static <K, V> Map<K, V> of(K k1, V v1, K k2, V v2, K k3, V v3, K k4, V v4, K k5, V v5) {
            Map<K, V> map = new Map<>();
            map.put(k1, v1);
            map.put(k2, v2);
            map.put(k3, v3);
            map.put(k4, v4);
            map.put(k5, v5);
            return map;
        }
    }
}
