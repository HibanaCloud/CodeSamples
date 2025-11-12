/**
 * 05 - JSON Mode
 *
 * This example demonstrates how to get structured JSON responses from the AI.
 * JSON mode ensures the model outputs valid JSON that can be easily parsed
 * and used in your applications.
 *
 * Model used: gpt-5-nano (OpenAI model with JSON support)
 */

import OpenAI from 'openai';

// Initialize the Hibana client
const client = new OpenAI({
    apiKey: "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.com/v1"
});

async function basicJsonMode() {
    /**
     * Get a structured JSON response
     */

    console.log("=".repeat(60));
    console.log("JSON Mode - Basic Example");
    console.log("=".repeat(60));

    // Request must explicitly mention JSON in the prompt
    const userMessage = `Create a JSON object for a book with the following fields:
    - title
    - author
    - year
    - genre
    - summary (short)

    Make up a fictional book.`;

    console.log(`\nUser: ${userMessage.substring(0, 80)}...`);
    console.log("\nRequesting JSON response from gpt-5-nano...");

    const response = await client.chat.completions.create({
        model: "gpt-5-nano",
        messages: [
            {
                role: "system",
                content: "You are a helpful assistant that outputs data in JSON format."
            },
            {
                role: "user",
                content: userMessage
            }
        ],
        temperature: 0.8,
        max_tokens: 10000,
        response_format: { type: "json_object" }  // Enable JSON mode
    });

    // Get the response
    const jsonResponse = response.choices[0].message.content;

    console.log("\nRaw JSON Response:");
    console.log("=".repeat(60));
    console.log(jsonResponse);
    console.log("=".repeat(60));

    // Parse and pretty-print the JSON
    try {
        const parsedJson = JSON.parse(jsonResponse);
        console.log("\nParsed JSON (pretty-printed):");
        console.log(JSON.stringify(parsedJson, null, 2));

        // Access specific fields
        console.log("\nAccessing specific fields:");
        console.log(`  Title: ${parsedJson.title || 'N/A'}`);
        console.log(`  Author: ${parsedJson.author || 'N/A'}`);
        console.log(`  Year: ${parsedJson.year || 'N/A'}`);
    } catch (error) {
        console.log(`Error parsing JSON: ${error.message}`);
    }
}

async function jsonForDataExtraction() {
    /**
     * Use JSON mode to extract structured data from text
     */

    console.log("\n" + "=".repeat(60));
    console.log("JSON Mode - Data Extraction");
    console.log("=".repeat(60));

    const textToAnalyze = `
    John Smith works as a Senior Software Engineer at TechCorp Inc.
    He can be reached at john.smith@techcorp.com or by phone at +1-555-0123.
    His office is located in San Francisco, California.
    `;

    const prompt = `Extract contact information from the following text and return it as JSON:

Text: ${textToAnalyze}

Return JSON with these fields: name, job_title, company, email, phone, location`;

    console.log(`\nExtracting structured data from text...`);

    const response = await client.chat.completions.create({
        model: "gpt-5-nano",
        messages: [
            {
                role: "system",
                content: "You extract information from text and return it as JSON."
            },
            {
                role: "user",
                content: prompt
            }
        ],
        temperature: 0.3,  // Lower temperature for more consistent extraction
        response_format: { type: "json_object" }
    });

    const jsonData = JSON.parse(response.choices[0].message.content);

    console.log("\nExtracted Data:");
    console.log(JSON.stringify(jsonData, null, 2));
}

async function jsonArrayResponse() {
    /**
     * Get a JSON array as response
     */

    console.log("\n" + "=".repeat(60));
    console.log("JSON Mode - Array Response");
    console.log("=".repeat(60));

    const prompt = `Generate a list of 5 programming languages with their primary use cases.
    Return as JSON array where each item has 'language' and 'use_case' fields.`;

    console.log("\nRequesting array of programming languages...");

    const response = await client.chat.completions.create({
        model: "gpt-5-nano",
        messages: [
            {
                role: "user",
                content: prompt
            }
        ],
        temperature: 0.7,
        response_format: { type: "json_object" }
    });

    const jsonResult = JSON.parse(response.choices[0].message.content);

    console.log("\nProgramming Languages:");
    console.log(JSON.stringify(jsonResult, null, 2));

    // Process the array
    if (jsonResult.languages) {
        console.log("\nFormatted output:");
        jsonResult.languages.forEach((lang, i) => {
            console.log(`${i + 1}. ${lang.language}: ${lang.use_case}`);
        });
    }
}

async function jsonComplexStructure() {
    /**
     * Create a complex nested JSON structure
     */

    console.log("\n" + "=".repeat(60));
    console.log("JSON Mode - Complex Nested Structure");
    console.log("=".repeat(60));

    const prompt = `Create a JSON object representing a university course with:
    - course_id
    - course_name
    - instructor (object with name and email)
    - schedule (array of class sessions with day, time, room)
    - students (array of 3 students with name and student_id)
    - grading (object with assignments percentage, exams percentage, participation percentage)
    `;

    console.log("\nGenerating complex JSON structure...");

    const response = await client.chat.completions.create({
        model: "gpt-5-nano",
        messages: [
            {
                role: "system",
                content: "Generate realistic but fictional data in JSON format."
            },
            {
                role: "user",
                content: prompt
            }
        ],
        temperature: 0.8,
        max_tokens: 10000,
        response_format: { type: "json_object" }
    });

    const complexJson = JSON.parse(response.choices[0].message.content);

    console.log("\nComplex JSON Structure:");
    console.log(JSON.stringify(complexJson, null, 2));
}

// Run the examples
(async () => {
    try {
        // Example 1: Basic JSON mode
        await basicJsonMode();

        // Example 2: Data extraction to JSON
        await jsonForDataExtraction();

        // Example 3: JSON array response
        await jsonArrayResponse();

        // Example 4: Complex nested structure
        await jsonComplexStructure();

    } catch (error) {
        console.error(`\nError: ${error.message}`);
    }
})();
