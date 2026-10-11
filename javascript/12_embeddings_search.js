/**
 * 12 - Embeddings: Semantic Search in Persian
 *
 * This example turns a few Persian documents into embedding vectors with
 * Hibana's /v1/embeddings endpoint, then finds the document closest in
 * meaning to a question (cosine similarity). It is the retrieval step of
 * RAG; a full walkthrough is at https://hibanacloud.ir/blog/rag-persian-bge-m3
 *
 * Model used: bge-m3 (1024-dimension vectors, multilingual, runs on
 * Hibana's own servers)
 */

import OpenAI from 'openai';

// Reads your Hibana API key from the HIBANA_API_KEY environment variable
const client = new OpenAI({
    apiKey: process.env.HIBANA_API_KEY ?? "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.ir/v1"
});

const MODEL = "bge-m3";

const DOCUMENTS = [
    "برای شارژ کیف پول از درگاه پرداخت بانکی استفاده کنید.",
    "کلید API را می‌توانید از داشبورد بسازید و هر وقت خواستید باطل کنید.",
    "مدل‌های داخلی هنگام قطعی اینترنت بین‌الملل هم در دسترس می‌مانند.",
    "برای دریافت پاسخ کلمه‌به‌کلمه، پارامتر stream را روشن کنید.",
];

async function embed(texts) {
    const response = await client.embeddings.create({ model: MODEL, input: texts });
    return response.data.map((item) => item.embedding);
}

function cosine(a, b) {
    let dot = 0, normA = 0, normB = 0;
    for (let i = 0; i < a.length; i++) {
        dot += a[i] * b[i];
        normA += a[i] * a[i];
        normB += b[i] * b[i];
    }
    return dot / (Math.sqrt(normA) * Math.sqrt(normB));
}

async function search(question) {
    console.log("=".repeat(60));
    console.log(`Semantic Search - ${MODEL}`);
    console.log("=".repeat(60));

    const docVectors = await embed(DOCUMENTS);
    console.log(`\nEmbedded ${docVectors.length} documents (${docVectors[0].length} dimensions each)`);

    const [questionVector] = await embed([question]);
    const ranked = DOCUMENTS
        .map((document, i) => ({ document, score: cosine(questionVector, docVectors[i]) }))
        .sort((a, b) => b.score - a.score);

    console.log(`\nQuestion: ${question}\n`);
    for (const { document, score } of ranked) {
        console.log(`  ${score.toFixed(3)}  ${document}`);
    }
    console.log(`\nBest match: ${ranked[0].document}`);
}

search("اگر اینترنت خارجی قطع شود چه اتفاقی می‌افتد؟").catch((error) => {
    console.error("Error:", error.message);
    process.exitCode = 1;
});
