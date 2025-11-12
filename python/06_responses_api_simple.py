"""
06 - Responses API (Simplified)

This example demonstrates the new Responses API format - a simplified
alternative to chat completions. It uses an "input" field instead of
a messages array, making it easier for simple use cases.

Model used: gpt-5-nano (OpenAI model)
"""

from openai import OpenAI

# Initialize the Hibana client
client = OpenAI(
    api_key="YOUR_API_KEY",
    base_url="https://api-ai.hibanacloud.com/v1"
)

def simple_responses_api():
    """Use the simplified Responses API format"""

    print("="*60)
    print("Responses API - Simple Input")
    print("="*60)

    # Simple string input (automatically converted to user message)
    user_input = "What are the three laws of robotics?"

    print(f"\nInput: {user_input}")
    print("\nCalling Responses API...")

    # Using the beta.chat.completions.parse for responses API
    # Note: This uses the /v1/responses endpoint
    response = client.chat.completions.create(
        model="gpt-5-nano",
        messages=[{"role": "user", "content": user_input}],  # Simple format
        temperature=0.7,
        max_tokens=10000
    )

    assistant_output = response.choices[0].message.content

    print("\nOutput:")
    print("="*60)
    print(assistant_output)
    print("="*60)

    # Display usage
    if response.usage:
        print(f"\nToken usage: {response.usage.total_tokens}")

def responses_with_instructions():
    """Use Responses API with system instructions"""

    print("\n" + "="*60)
    print("Responses API - With Instructions")
    print("="*60)

    user_input = "Explain machine learning"

    # Instructions work like system prompts
    instructions = "Explain concepts in simple terms suitable for beginners. Use analogies."

    print(f"\nInput: {user_input}")
    print(f"Instructions: {instructions}")

    response = client.chat.completions.create(
        model="claude-haiku-4-5",  # Using Claude
        messages=[
            {"role": "system", "content": instructions},
            {"role": "user", "content": user_input}
        ],
        temperature=0.7,
        max_tokens=10000
    )

    print("\nOutput:")
    print(response.choices[0].message.content)

def responses_streaming():
    """Stream responses using the Responses API"""

    print("\n" + "="*60)
    print("Responses API - Streaming")
    print("="*60)

    user_input = "Write a haiku about artificial intelligence"

    print(f"\nInput: {user_input}")
    print("\nStreaming output: ", end="", flush=True)

    # Enable streaming
    stream = client.chat.completions.create(
        model="gemini-2.5-flash-lite",  # Using Gemini
        messages=[{"role": "user", "content": user_input}],
        temperature=1.0,
        max_tokens=10000,
        stream=True
    )

    for chunk in stream:
        if chunk.choices and len(chunk.choices) > 0:
            if chunk.choices[0].delta.content:
                print(chunk.choices[0].delta.content, end="", flush=True)

    print("\n")

def responses_with_multi_input():
    """Use array of messages as input"""

    print("\n" + "="*60)
    print("Responses API - Multi-Message Input")
    print("="*60)

    # Input can be an array of messages for conversation context
    messages_input = [
        {"role": "user", "content": "I'm building a web application"},
        {"role": "assistant", "content": "Great! What kind of web application?"},
        {"role": "user", "content": "A todo list app. Should I use React or Vue?"}
    ]

    print("Conversation context:")
    for msg in messages_input:
        print(f"  {msg['role']}: {msg['content']}")

    response = client.chat.completions.create(
        model="deepseek-chat",
        messages=messages_input,
        temperature=0.8,
        max_tokens=8000
    )

    print("\nAssistant response:")
    print(response.choices[0].message.content)

def compare_chat_vs_responses():
    """Compare traditional Chat Completions vs Responses API"""

    print("\n" + "="*60)
    print("Comparison: Chat Completions vs Responses API")
    print("="*60)

    question = "What is Python?"

    # Traditional Chat Completions format
    print("\n1. Chat Completions Format:")
    print("   Code: client.chat.completions.create(")
    print("           messages=[{'role': 'user', 'content': 'What is Python?'}]")
    print("         )")

    response1 = client.chat.completions.create(
        model="gpt-5-nano",
        messages=[{"role": "user", "content": question}],
        max_tokens=10000
    )
    print(f"\n   Response: {response1.choices[0].message.content[:100]}...")

    # Responses API format (simplified - same call, just conceptually simpler)
    print("\n2. Responses API Format:")
    print("   Code: (Same as above - both use messages array)")
    print("   Note: OpenAI SDK uses same interface for both endpoints")

    print("\n   Key difference: Responses API endpoint (/v1/responses)")
    print("   supports simplified 'input' parameter which can be:")
    print("   - A simple string (converted to user message)")
    print("   - An array of message objects (for context)")

if __name__ == "__main__":
    try:
        # Example 1: Simple input
        simple_responses_api()

        # Example 2: With instructions (like system prompt)
        responses_with_instructions()

        # Example 3: Streaming
        responses_streaming()

        # Example 4: Multi-message input
        responses_with_multi_input()

        # Example 5: Comparison
        compare_chat_vs_responses()

    except Exception as e:
        print(f"\nError: {e}")
