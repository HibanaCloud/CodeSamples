"""
11 - Multiple Providers Comparison

This example demonstrates how to use all four LLM providers available
through Hibana API: OpenAI, Anthropic, DeepSeek, and Google Gemini.
Compare responses, performance, and features across providers.
"""

from openai import OpenAI
import time

# Initialize the Hibana client
client = OpenAI(
    api_key="YOUR_API_KEY",
    base_url="https://api-ai.hibanacloud.com/v1"
)

# Define models for each provider
MODELS = {
    "OpenAI": "gpt-5-nano",
    "Anthropic": "claude-haiku-4-5",
    "DeepSeek": "deepseek-chat",
    "Google": "gemini-2.5-flash-lite"
}

def compare_basic_responses():
    """Compare responses from all four providers"""

    print("="*60)
    print("Comparing All Providers")
    print("="*60)

    question = "What is the future of artificial intelligence? Answer in 2 sentences."

    print(f"\nQuestion: {question}\n")

    for provider, model in MODELS.items():
        print(f"\n{provider} ({model}):")
        print("-" * 60)

        try:
            start_time = time.time()

            response = client.chat.completions.create(
                model=model,
                messages=[{"role": "user", "content": question}],
                temperature=0.7,
                max_tokens=8000
            )

            elapsed = time.time() - start_time

            answer = response.choices[0].message.content
            tokens = response.usage.total_tokens if response.usage else 0

            print(f"Response: {answer}")
            print(f"\nTime: {elapsed:.2f}s | Tokens: {tokens}")

        except Exception as e:
            print(f"Error: {e}")

def compare_coding_tasks():
    """Compare coding assistance from different providers"""

    print("\n" + "="*60)
    print("Coding Task Comparison")
    print("="*60)

    coding_question = "Write a Python function to check if a string is a palindrome. Include comments."

    print(f"\nTask: {coding_question}\n")

    for provider, model in MODELS.items():
        print(f"\n{provider} ({model}):")
        print("-" * 60)

        try:
            response = client.chat.completions.create(
                model=model,
                messages=[
                    {
                        "role": "system",
                        "content": "You are a Python programming expert."
                    },
                    {
                        "role": "user",
                        "content": coding_question
                    }
                ],
                temperature=0.3,  # Lower temp for coding
                max_tokens=8000
            )

            print(response.choices[0].message.content)

        except Exception as e:
            print(f"Error: {e}")

def compare_creative_writing():
    """Compare creative writing capabilities"""

    print("\n" + "="*60)
    print("Creative Writing Comparison")
    print("="*60)

    prompt = "Write a two-line poem about technology and humanity."

    print(f"\nPrompt: {prompt}\n")

    for provider, model in MODELS.items():
        print(f"\n{provider} ({model}):")
        print("-" * 60)

        try:
            response = client.chat.completions.create(
                model=model,
                messages=[{"role": "user", "content": prompt}],
                temperature=1.0,  # Higher temp for creativity
                max_tokens=8000
            )

            print(response.choices[0].message.content)

        except Exception as e:
            print(f"Error: {e}")

def compare_multilingual():
    """Compare multilingual capabilities (Persian)"""

    print("\n" + "="*60)
    print("Multilingual Capability (Persian)")
    print("="*60)

    persian_question = "D7A' /1 ̩ ,EDG �H*'G *H6�- (/G �G GH4 E5FH9� ��3*"

    print(f"\nQuestion (Persian): {persian_question}\n")

    for provider, model in MODELS.items():
        print(f"\n{provider} ({model}):")
        print("-" * 60)

        try:
            response = client.chat.completions.create(
                model=model,
                messages=[{"role": "user", "content": persian_question}],
                temperature=0.7,
                max_tokens=8000
            )

            print(response.choices[0].message.content)

        except Exception as e:
            print(f"Error: {e}")

def benchmark_performance():
    """Benchmark response time and token usage"""

    print("\n" + "="*60)
    print("Performance Benchmark")
    print("="*60)

    test_message = "Explain quantum computing in one sentence."

    results = []

    print(f"\nTest message: {test_message}\n")
    print("Running benchmark...\n")

    for provider, model in MODELS.items():
        try:
            # Warm-up request (not counted)
            client.chat.completions.create(
                model=model,
                messages=[{"role": "user", "content": "Hi"}],
                max_tokens=8000
            )

            # Actual benchmark
            start = time.time()

            response = client.chat.completions.create(
                model=model,
                messages=[{"role": "user", "content": test_message}],
                temperature=0.5,
                max_tokens=8000
            )

            elapsed = time.time() - start

            results.append({
                "provider": provider,
                "model": model,
                "time": elapsed,
                "tokens": response.usage.total_tokens if response.usage else 0,
                "response_length": len(response.choices[0].message.content)
            })

        except Exception as e:
            print(f"{provider}: Error - {e}")

    # Display results
    print("\nBenchmark Results:")
    print("-" * 80)
    print(f"{'Provider':<15} {'Model':<25} {'Time (s)':<12} {'Tokens':<10} {'Length':<10}")
    print("-" * 80)

    for result in sorted(results, key=lambda x: x['time']):
        print(f"{result['provider']:<15} {result['model']:<25} "
              f"{result['time']:<12.3f} {result['tokens']:<10} {result['response_length']:<10}")

    print("-" * 80)

    # Find fastest and slowest
    if results:
        fastest = min(results, key=lambda x: x['time'])
        print(f"\nFastest: {fastest['provider']} ({fastest['time']:.3f}s)")

def provider_specific_features():
    """Demonstrate provider-specific features"""

    print("\n" + "="*60)
    print("Provider-Specific Features")
    print("="*60)

    # OpenAI: JSON mode
    print("\n1. OpenAI - JSON Mode:")
    print("-" * 60)
    try:
        response = client.chat.completions.create(
            model="gpt-5-nano",
            messages=[
                {
                    "role": "system",
                    "content": "Return responses as JSON."
                },
                {
                    "role": "user",
                    "content": "Create a JSON object with name and age for a fictional person."
                }
            ],
            response_format={"type": "json_object"}
        )
        print(response.choices[0].message.content)
    except Exception as e:
        print(f"Error: {e}")

    # Anthropic: Long context
    print("\n\n2. Anthropic Claude - Long Context:")
    print("-" * 60)
    try:
        long_text = "AI " * 500  # Simulate longer context
        response = client.chat.completions.create(
            model="claude-haiku-4-5",
            messages=[
                {
                    "role": "user",
                    "content": f"Count how many times 'AI' appears in this text: {long_text}"
                }
            ],
            max_tokens=8000
        )
        print(response.choices[0].message.content)
    except Exception as e:
        print(f"Error: {e}")

    # DeepSeek: Cost-effective
    print("\n\n3. DeepSeek - Cost-Effective Model:")
    print("-" * 60)
    try:
        response = client.chat.completions.create(
            model="deepseek-chat",
            messages=[
                {
                    "role": "user",
                    "content": "What makes DeepSeek models cost-effective?"
                }
            ],
            max_tokens=8000
        )
        print(response.choices[0].message.content)
        if response.usage and hasattr(response.usage, 'cost_rial'):
            print(f"\nCost: {response.usage.cost_rial} Rials")
    except Exception as e:
        print(f"Error: {e}")

    # Gemini: Multimodal potential
    print("\n\n4. Google Gemini - Fast & Efficient:")
    print("-" * 60)
    try:
        response = client.chat.completions.create(
            model="gemini-2.5-flash-lite",
            messages=[
                {
                    "role": "user",
                    "content": "List 3 advantages of Gemini models in one sentence each."
                }
            ],
            max_tokens=8000
        )
        print(response.choices[0].message.content)
    except Exception as e:
        print(f"Error: {e}")

def choose_best_model_for_task():
    """Recommendation system for choosing models"""

    print("\n" + "="*60)
    print("Model Selection Guide")
    print("="*60)

    recommendations = {
        "Speed & Cost": {
            "model": "gpt-5-nano or deepseek-chat",
            "reason": "Fast responses and low cost"
        },
        "Coding": {
            "model": "deepseek-chat or gpt-5-nano",
            "reason": "Strong programming capabilities"
        },
        "Creative Writing": {
            "model": "claude-haiku-4-5 or gpt-5-nano",
            "reason": "Excellent creative output"
        },
        "Long Context": {
            "model": "claude-haiku-4-5",
            "reason": "Large context window"
        },
        "Persian/Multilingual": {
            "model": "gpt-5-nano or gemini-2.5-flash-lite",
            "reason": "Good multilingual support"
        },
        "JSON Output": {
            "model": "gpt-5-nano",
            "reason": "Native JSON mode support"
        }
    }

    print("\nTask-Based Model Recommendations:\n")

    for task, info in recommendations.items():
        print(f"{task}:")
        print(f"  � {info['model']}")
        print(f"  Reason: {info['reason']}")
        print()

if __name__ == "__main__":
    try:
        # Example 1: Basic comparison
        compare_basic_responses()

        # Example 2: Coding tasks
        # compare_coding_tasks()  # Uncomment to run

        # Example 3: Creative writing
        # compare_creative_writing()  # Uncomment to run

        # Example 4: Multilingual
        # compare_multilingual()  # Uncomment to run

        # Example 5: Performance benchmark
        benchmark_performance()

        # Example 6: Provider-specific features
        provider_specific_features()

        # Example 7: Model selection guide
        choose_best_model_for_task()

        print("\n" + "="*60)
        print("Multiple provider comparison completed!")
        print("="*60)

    except Exception as e:
        print(f"\nError: {e}")
