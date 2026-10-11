"""
12 - Embeddings: Semantic Search in Persian

This example turns a few Persian documents into embedding vectors with
Hibana's /v1/embeddings endpoint, then finds the document closest in
meaning to a question (cosine similarity). It is the retrieval step of
RAG; a full walkthrough is at https://hibanacloud.ir/blog/rag-persian-bge-m3

Model used: bge-m3 (1024-dimension vectors, multilingual, runs on
Hibana's own servers)
"""

import math
import os
from openai import OpenAI

# Reads your Hibana API key from the HIBANA_API_KEY environment variable
client = OpenAI(
    api_key=os.environ.get("HIBANA_API_KEY", "YOUR_API_KEY"),
    base_url="https://api-ai.hibanacloud.ir/v1"
)

MODEL = "bge-m3"

DOCUMENTS = [
    "برای شارژ کیف پول از درگاه پرداخت بانکی استفاده کنید.",
    "کلید API را می‌توانید از داشبورد بسازید و هر وقت خواستید باطل کنید.",
    "مدل‌های داخلی هنگام قطعی اینترنت بین‌الملل هم در دسترس می‌مانند.",
    "برای دریافت پاسخ کلمه‌به‌کلمه، پارامتر stream را روشن کنید.",
]


def embed(texts):
    """Return one embedding vector per input text"""
    response = client.embeddings.create(model=MODEL, input=texts)
    return [item.embedding for item in response.data]


def cosine(a, b):
    dot = sum(x * y for x, y in zip(a, b))
    return dot / (math.sqrt(sum(x * x for x in a)) * math.sqrt(sum(y * y for y in b)))


def search(question):
    print("=" * 60)
    print(f"Semantic Search - {MODEL}")
    print("=" * 60)

    doc_vectors = embed(DOCUMENTS)
    print(f"\nEmbedded {len(doc_vectors)} documents ({len(doc_vectors[0])} dimensions each)")

    question_vector = embed([question])[0]
    ranked = sorted(
        zip(DOCUMENTS, (cosine(question_vector, v) for v in doc_vectors)),
        key=lambda pair: pair[1],
        reverse=True,
    )

    print(f"\nQuestion: {question}\n")
    for document, score in ranked:
        print(f"  {score:.3f}  {document}")
    print(f"\nBest match: {ranked[0][0]}")


if __name__ == "__main__":
    search("اگر اینترنت خارجی قطع شود چه اتفاقی می‌افتد؟")
