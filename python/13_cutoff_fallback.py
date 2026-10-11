"""
13 - Staying Online During an Internet Cutoff

Foreign models (GPT, Claude, Gemini, ...) depend on Iran's international
internet link. Hibana also runs models on its own servers inside Iran,
which keep answering when that link is cut. This example tries a foreign
model first and falls back to a domestic one if the call fails, with the
same API key and the same code - only the model name changes.

More: https://hibanacloud.ir/blog/ai-api-without-vpn-international-cutoff

Models used: gpt-5-nano (primary), gpt-oss (domestic fallback)
"""

import os
from openai import OpenAI, APIConnectionError, APIStatusError

# Reads your Hibana API key from the HIBANA_API_KEY environment variable
client = OpenAI(
    api_key=os.environ.get("HIBANA_API_KEY", "YOUR_API_KEY"),
    base_url="https://api-ai.hibanacloud.ir/v1",
    max_retries=0,
    timeout=60,
)

PRIMARY_MODEL = "gpt-5-nano"
FALLBACK_MODEL = "gpt-oss"  # runs on Hibana's own servers


def ask(question):
    """Ask the primary model; on a provider or network failure, ask the fallback"""
    for model in (PRIMARY_MODEL, FALLBACK_MODEL):
        try:
            response = client.chat.completions.create(
                model=model,
                messages=[{"role": "user", "content": question}],
            )
            return model, response.choices[0].message.content
        except (APIConnectionError, APIStatusError) as error:
            # 4xx errors such as a bad key or an empty wallet will fail on any
            # model, so only switch on connection problems and 5xx errors.
            if isinstance(error, APIStatusError) and error.status_code < 500:
                raise
            print(f"{model} failed ({error.__class__.__name__}), trying {FALLBACK_MODEL}...")
    raise RuntimeError("Both models failed")


if __name__ == "__main__":
    print("=" * 60)
    print("Cutoff-ready chat: primary model with a domestic fallback")
    print("=" * 60)

    model, answer = ask("In one sentence, what is an API?")
    print(f"\nAnswered by: {model}")
    print(f"\n{answer}")
