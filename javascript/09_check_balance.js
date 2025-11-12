/**
 * 09 - Check Balance
 *
 * This example demonstrates how to check your wallet balance using
 * the Hibana API. This endpoint is not part of the standard OpenAI API,
 * so we use axios to call it directly.
 *
 * Endpoint: GET /v1/user/balance
 */

import axios from 'axios';

// API Configuration
const API_KEY = "YOUR_API_KEY";
const BASE_URL = "https://api-ai.hibanacloud.com/v1";

async function checkBalance() {
    /**
     * Check current wallet balance
     */

    console.log("=".repeat(60));
    console.log("Check Wallet Balance");
    console.log("=".repeat(60));

    // Endpoint URL
    const url = `${BASE_URL}/user/balance`;

    // Headers with API key
    const headers = {
        "Authorization": `Bearer ${API_KEY}`,
        "Content-Type": "application/json"
    };

    console.log("\nFetching balance...");

    try {
        // Make GET request
        const response = await axios.get(url, { headers });

        // Check if request was successful
        if (response.status === 200) {
            const data = response.data;

            console.log("\n" + "=".repeat(60));
            console.log("Balance Information");
            console.log("=".repeat(60));

            // Display balance information
            const balance = data.balance || 0;
            const currency = data.currency || 'IRR';

            console.log(`\nCurrent Balance: ${balance.toLocaleString()} ${currency}`);

            // Format with thousands separator
            if (currency === 'IRR') {
                console.log(`Formatted: ${balance.toLocaleString()} Rials`);
            }

            // Estimate usage capability (example calculation)
            // Note: Actual cost depends on model and usage
            const avgCostPerRequest = 100;  // Example: 100 Rials per request
            const estimatedRequests = Math.floor(balance / avgCostPerRequest);

            console.log(`\nEstimated remaining requests: ~${estimatedRequests.toLocaleString()}`);
            console.log("(Based on average cost, actual may vary)");
        }

    } catch (error) {
        if (error.response) {
            console.log(`\nError: ${error.response.status}`);
            console.log(`Message: ${error.response.data}`);
        } else {
            console.log(`\nRequest error: ${error.message}`);
        }
    }
}

async function checkBalanceWithDetails() {
    /**
     * Check balance with additional error handling and details
     */

    console.log("\n" + "=".repeat(60));
    console.log("Detailed Balance Check");
    console.log("=".repeat(60));

    const url = `${BASE_URL}/user/balance`;
    const headers = {
        "Authorization": `Bearer ${API_KEY}`,
        "Content-Type": "application/json"
    };

    try {
        const response = await axios.get(url, { headers, timeout: 10000 });

        console.log(`\nStatus Code: ${response.status}`);

        if (response.status === 200) {
            const data = response.data;

            console.log("\nFull Response:");
            console.log("-".repeat(60));

            // Display all fields in response
            for (const [key, value] of Object.entries(data)) {
                console.log(`${key}: ${value}`);
            }

            console.log("-".repeat(60));

            // Check balance status
            const balance = data.balance || 0;

            let status;
            if (balance > 10000) {
                status = " Healthy balance";
            } else if (balance > 1000) {
                status = "� Low balance";
            } else {
                status = " Critical - Please recharge";
            }

            console.log(`\nStatus: ${status}`);
        }

    } catch (error) {
        if (error.response) {
            if (error.response.status === 401) {
                console.log("\n Authentication Error");
                console.log("Your API key may be invalid or expired.");
            } else if (error.response.status === 403) {
                console.log("\n Access Forbidden");
                console.log("Your account may not have permission to access this endpoint.");
            } else {
                console.log(`\n Unexpected error: ${error.response.status}`);
                console.log(`Response: ${error.response.data}`);
            }
        } else if (error.code === 'ECONNABORTED') {
            console.log("\n Request timeout - Server took too long to respond");
        } else if (error.code === 'ECONNREFUSED') {
            console.log("\n Connection error - Could not reach the server");
        } else {
            console.log(`\n Error: ${error.message}`);
        }
    }
}

async function monitorBalanceBeforeRequest() {
    /**
     * Example: Check balance before making an API call
     */

    console.log("\n" + "=".repeat(60));
    console.log("Pre-Request Balance Check");
    console.log("=".repeat(60));

    const url = `${BASE_URL}/user/balance`;
    const headers = {
        "Authorization": `Bearer ${API_KEY}`,
        "Content-Type": "application/json"
    };

    try {
        // Check balance
        const response = await axios.get(url, { headers });

        if (response.status === 200) {
            const data = response.data;
            const balance = data.balance || 0;

            console.log(`\nCurrent balance: ${balance.toLocaleString()} Rials`);

            // Set minimum balance threshold
            const minimumBalance = 1000;  // 1,000 Rials

            if (balance >= minimumBalance) {
                console.log(" Sufficient balance to proceed with API call");

                // Here you would make your actual API call
                console.log("\nProceed with API request...");

            } else {
                console.log(" Insufficient balance");
                console.log(`Minimum required: ${minimumBalance.toLocaleString()} Rials`);
                console.log("Please recharge your account before making requests.");
            }
        }

    } catch (error) {
        console.log(`Could not check balance: ${error.response?.status || error.message}`);
    }
}

async function balanceCheckWithUsageHistory() {
    /**
     * Check balance and provide usage recommendations
     */

    console.log("\n" + "=".repeat(60));
    console.log("Balance with Usage Recommendations");
    console.log("=".repeat(60));

    const url = `${BASE_URL}/user/balance`;
    const headers = {
        "Authorization": `Bearer ${API_KEY}`,
        "Content-Type": "application/json"
    };

    try {
        const response = await axios.get(url, { headers });

        if (response.status === 200) {
            const data = response.data;
            const balance = data.balance || 0;

            console.log(`\nCurrent Balance: ${balance.toLocaleString()} Rials\n`);

            // Model cost estimates (example values)
            const modelCosts = {
                "gpt-5-nano": 50,
                "claude-haiku-4-5": 75,
                "deepseek-chat": 40,
                "gemini-2.5-flash-lite": 45,
                "dall-e-3": 5000
            };

            console.log("Estimated requests per model:");
            console.log("-".repeat(60));

            for (const [model, cost] of Object.entries(modelCosts)) {
                const requestsPossible = Math.floor(balance / cost);
                console.log(`${model.padEnd(25)} ~${requestsPossible.toString().padStart(6)} requests`);
            }

            console.log("-".repeat(60));

            // Recommendations
            console.log("\nRecommendations:");
            if (balance < 5000) {
                console.log("  • Consider recharging soon");
                console.log("  • Use cost-efficient models (gpt-5-nano, deepseek-chat)");
            } else if (balance < 20000) {
                console.log("  • Balance is moderate");
                console.log("  • Monitor usage regularly");
            } else {
                console.log("  • Balance is healthy");
                console.log("  • You can use any model freely");
            }
        }

    } catch (error) {
        console.log(`Error checking balance: ${error.response?.status || error.message}`);
    }
}

// Run the examples
(async () => {
    try {
        // Example 1: Simple balance check
        await checkBalance();

        // Example 2: Detailed balance check
        await checkBalanceWithDetails();

        // Example 3: Pre-request balance check
        await monitorBalanceBeforeRequest();

        // Example 4: Balance with usage recommendations
        await balanceCheckWithUsageHistory();

    } catch (error) {
        console.error(`\nError: ${error.message}`);
    }
})();
