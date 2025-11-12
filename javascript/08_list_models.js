/**
 * 08 - List Available Models
 *
 * This example demonstrates how to retrieve all available models
 * from the Hibana API. This is useful for discovering what models
 * are enabled for your account.
 *
 * Endpoint: GET /v1/models
 */

import OpenAI from 'openai';

// Initialize the Hibana client
const client = new OpenAI({
    apiKey: "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.com/v1"
});

async function listAllModels() {
    /**
     * List all available models
     */

    console.log("=".repeat(60));
    console.log("Listing All Available Models");
    console.log("=".repeat(60));

    // Get list of models
    const models = await client.models.list();

    console.log(`\nTotal models available: ${models.data.length}\n`);

    // Display each model
    models.data.forEach((model, i) => {
        console.log(`${i + 1}. ${model.id}`);
        console.log(`   Owner: ${model.owned_by}`);
        if (model.created) {
            const createdDate = new Date(model.created * 1000);
            console.log(`   Created: ${createdDate.toISOString().split('T')[0]}`);
        }
        console.log();
    });
}

async function groupModelsByProvider() {
    /**
     * Group models by their provider (OpenAI, Anthropic, etc.)
     */

    console.log("=".repeat(60));
    console.log("Models Grouped by Provider");
    console.log("=".repeat(60));

    const models = await client.models.list();

    // Group models by owner/provider
    const grouped = {};
    models.data.forEach(model => {
        const provider = model.owned_by;
        if (!grouped[provider]) {
            grouped[provider] = [];
        }
        grouped[provider].push(model.id);
    });

    // Display grouped models
    for (const [provider, modelList] of Object.entries(grouped)) {
        console.log(`\n${provider.toUpperCase()}`);
        console.log("-".repeat(40));
        modelList.forEach(modelId => {
            console.log(`  " ${modelId}`);
        });
    }

    console.log(`\nTotal providers: ${Object.keys(grouped).length}`);
}

async function filterModelsByType() {
    /**
     * Filter and categorize models by their type
     */

    console.log("\n" + "=".repeat(60));
    console.log("Models by Type");
    console.log("=".repeat(60));

    const models = await client.models.list();

    // Categorize models
    const chatModels = [];
    const imageModels = [];
    const otherModels = [];

    models.data.forEach(model => {
        const modelId = model.id.toLowerCase();

        if (modelId.includes('dall-e') || modelId.includes('dalle')) {
            imageModels.push(model.id);
        } else if (['gpt', 'claude', 'deepseek', 'gemini'].some(x => modelId.includes(x))) {
            chatModels.push(model.id);
        } else {
            otherModels.push(model.id);
        }
    });

    // Display categorized models
    console.log("\nCHAT/TEXT MODELS:");
    console.log("-".repeat(40));
    chatModels.forEach(model => {
        console.log(`  " ${model}`);
    });

    console.log("\n\nIMAGE GENERATION MODELS:");
    console.log("-".repeat(40));
    imageModels.forEach(model => {
        console.log(`  " ${model}`);
    });

    if (otherModels.length > 0) {
        console.log("\n\nOTHER MODELS:");
        console.log("-".repeat(40));
        otherModels.forEach(model => {
            console.log(`  " ${model}`);
        });
    }
}

async function getSpecificModelInfo() {
    /**
     * Get information about a specific model
     */

    console.log("\n" + "=".repeat(60));
    console.log("Specific Model Information");
    console.log("=".repeat(60));

    const modelId = "gpt-5-nano";

    console.log(`\nQuerying model: ${modelId}`);

    try {
        // Retrieve specific model
        const model = await client.models.retrieve(modelId);

        console.log("\nModel Details:");
        console.log("-".repeat(40));
        console.log(`ID: ${model.id}`);
        console.log(`Object: ${model.object}`);
        console.log(`Owned by: ${model.owned_by}`);

        if (model.created) {
            const createdDate = new Date(model.created * 1000);
            console.log(`Created: ${createdDate.toISOString()}`);
        }

        if (model.permission) {
            console.log(`Permissions: ${model.permission.length} permission(s)`);
        }

    } catch (error) {
        console.log(`Error retrieving model: ${error.message}`);
    }
}

async function checkModelAvailability() {
    /**
     * Check if specific models are available
     */

    console.log("\n" + "=".repeat(60));
    console.log("Model Availability Check");
    console.log("=".repeat(60));

    // Models to check
    const modelsToCheck = [
        "gpt-5-nano",
        "claude-haiku-4-5",
        "deepseek-chat",
        "gemini-2.5-flash-lite",
        "dall-e-3"
    ];

    console.log("\nChecking availability...\n");

    // Get all available models
    const availableModels = await client.models.list();
    const availableIds = availableModels.data.map(m => m.id);

    // Check each model
    modelsToCheck.forEach(modelId => {
        const isAvailable = availableIds.includes(modelId);
        const status = isAvailable ? " Available" : " Not Available";
        console.log(`${status.padEnd(20)} ${modelId}`);
    });
}

async function displayModelsTable() {
    /**
     * Display models in a formatted table
     */

    console.log("\n" + "=".repeat(60));
    console.log("Models Table");
    console.log("=".repeat(60));

    const models = await client.models.list();

    // Table header
    console.log(`\n${"Model ID".padEnd(40)} ${"Provider".padEnd(15)} ${"Type".padEnd(10)}`);
    console.log("-".repeat(70));

    // Table rows
    models.data.forEach(model => {
        const modelId = model.id;
        const provider = model.owned_by;

        // Determine type
        let modelType;
        if (modelId.toLowerCase().includes('dall-e')) {
            modelType = "Image";
        } else if (['gpt', 'claude', 'deepseek', 'gemini'].some(x => modelId.toLowerCase().includes(x))) {
            modelType = "Chat";
        } else {
            modelType = "Other";
        }

        console.log(`${modelId.padEnd(40)} ${provider.padEnd(15)} ${modelType.padEnd(10)}`);
    });

    console.log();
}

// Run the examples
(async () => {
    try {
        // Example 1: List all models
        await listAllModels();

        // Example 2: Group by provider
        await groupModelsByProvider();

        // Example 3: Filter by type
        await filterModelsByType();

        // Example 4: Get specific model info
        await getSpecificModelInfo();

        // Example 5: Check availability
        await checkModelAvailability();

        // Example 6: Display as table
        await displayModelsTable();

    } catch (error) {
        console.error(`\nError: ${error.message}`);
    }
})();
