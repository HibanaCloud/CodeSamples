/**
 * 03 - Multi-Turn Conversation
 *
 * This example demonstrates how to maintain conversation context
 * across multiple messages. The AI can reference previous messages
 * and maintain a coherent conversation flow.
 *
 * Model used: deepseek-chat (DeepSeek conversational model)
 */

import OpenAI from 'openai';

// Initialize the Hibana client
const client = new OpenAI({
    apiKey: "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.com/v1"
});

async function multiTurnConversation() {
    /**
     * Demonstrate a multi-turn conversation with context
     */

    // Conversation history - this maintains context across turns
    const conversationHistory = [
        {
            role: "system",
            content: "You are a helpful programming tutor specializing in Python."
        },
        {
            role: "user",
            content: "Hi! I'm learning Python. Can you help me?"
        }
    ];

    console.log("=".repeat(60));
    console.log("Multi-Turn Conversation with deepseek-chat");
    console.log("=".repeat(60));

    // First exchange
    console.log("\nUser: Hi! I'm learning Python. Can you help me?");

    const response1 = await client.chat.completions.create({
        model: "deepseek-chat",
        messages: conversationHistory,
        temperature: 0.8,
        max_tokens: 8192
    });

    const assistantReply1 = response1.choices[0].message.content;
    console.log(`\nAssistant: ${assistantReply1}`);

    // Add assistant's response to conversation history
    conversationHistory.push({
        role: "assistant",
        content: assistantReply1
    });

    // Second turn - ask a follow-up question
    const userMessage2 = "What's the difference between a list and a tuple?";
    conversationHistory.push({
        role: "user",
        content: userMessage2
    });

    console.log(`\nUser: ${userMessage2}`);

    const response2 = await client.chat.completions.create({
        model: "deepseek-chat",
        messages: conversationHistory,
        temperature: 0.8,
        max_tokens: 8192
    });

    const assistantReply2 = response2.choices[0].message.content;
    console.log(`\nAssistant: ${assistantReply2}`);

    // Add to history for potential future turns
    conversationHistory.push({
        role: "assistant",
        content: assistantReply2
    });

    // Third turn - reference previous context
    const userMessage3 = "Can you show me an example of each?";
    conversationHistory.push({
        role: "user",
        content: userMessage3
    });

    console.log(`\nUser: ${userMessage3}`);

    const response3 = await client.chat.completions.create({
        model: "deepseek-chat",
        messages: conversationHistory,
        temperature: 0.8,
        max_tokens: 8192
    });

    const assistantReply3 = response3.choices[0].message.content;
    console.log(`\nAssistant: ${assistantReply3}`);

    console.log("\n" + "=".repeat(60));
    console.log(`Total messages in conversation: ${conversationHistory.length + 1}`);
    console.log("=".repeat(60));

    // Display total usage
    if (response3.usage) {
        console.log(`\nLast request token usage:`);
        console.log(`  Total tokens: ${response3.usage.total_tokens}`);
    }
}

function interactiveConversation() {
    /**
     * Interactive conversation loop (demonstration - not running)
     */

    console.log("\n" + "=".repeat(60));
    console.log("Interactive mode (demonstration - not running)");
    console.log("=".repeat(60));
    console.log(`
    // To create an interactive chatbot, use this pattern:

    const conversation = [{ role: "system", content: "You are a helpful assistant." }];
    const readline = require('readline').createInterface({
        input: process.stdin,
        output: process.stdout
    });

    function askQuestion() {
        readline.question("You: ", async (input) => {
            if (['exit', 'quit', 'bye'].includes(input.toLowerCase())) {
                readline.close();
                return;
            }

            conversation.push({ role: "user", content: input });

            const response = await client.chat.completions.create({
                model: "deepseek-chat",
                messages: conversation,
                temperature: 0.8
            });

            const assistantMessage = response.choices[0].message.content;
            console.log(\`Assistant: \${assistantMessage}\`);

            conversation.push({ role: "assistant", content: assistantMessage });

            askQuestion();
        });
    }

    askQuestion();
    `);
}

// Run the example
(async () => {
    try {
        // Run multi-turn conversation example
        await multiTurnConversation();

        // Show interactive conversation pattern
        interactiveConversation();

    } catch (error) {
        console.error(`Error: ${error.message}`);
    }
})();
