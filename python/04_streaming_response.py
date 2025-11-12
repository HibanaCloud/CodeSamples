"""
04 - Streaming Response

This example demonstrates real-time streaming of AI responses using
Server-Sent Events (SSE). Instead of waiting for the complete response,
tokens are received and displayed progressively as they're generated.

Model used: gpt-5-nano (Fast OpenAI model)
"""

from openai import OpenAI
import sys

# Initialize the Hibana client
client = OpenAI(
    api_key="YOUR_API_KEY",
    base_url="https://api-ai.hibanacloud.com/v1"
)

def streaming_chat():
    """Demonstrate streaming responses with real-time output"""

    print("="*60)
    print("Streaming Response Demo")
    print("="*60)

    user_message = "Write a short story about a robot learning to paint. Make it about 150 words."

    print(f"\nUser: {user_message}")
    print("\nAssistant (streaming): ", end="", flush=True)

    # Create streaming chat completion
    stream = client.chat.completions.create(
        model="gpt-5-nano",
        messages=[
            {
                "role": "user",
                "content": user_message
            }
        ],
        temperature=1.0,
        max_tokens=10000,
        stream=True  # Enable streaming
    )

    # Collect the full response for later display
    full_response = ""

    # Process the stream
    for chunk in stream:
        # Check if chunk has choices and content
        if chunk.choices and len(chunk.choices) > 0:
            if chunk.choices[0].delta.content is not None:
                content = chunk.choices[0].delta.content
                full_response += content

                # Print the content immediately (streaming effect)
                print(content, end="", flush=True)

    print("\n\n" + "="*60)
    print("Streaming complete!")
    print("="*60)
    print(f"\nTotal characters received: {len(full_response)}")

def streaming_with_metadata():
    """Streaming with detailed chunk inspection"""

    print("\n" + "="*60)
    print("Streaming with Metadata Inspection")
    print("="*60)

    user_message = "Explain quantum entanglement in 2 sentences."

    print(f"\nUser: {user_message}")
    print("\nStreaming response...\n")

    stream = client.chat.completions.create(
        model="gemini-2.5-flash-lite",  # Using Gemini model
        messages=[{"role": "user", "content": user_message}],
        temperature=0.7,
        max_tokens=10000,
        stream=True
    )

    full_text = ""
    chunk_count = 0

    for chunk in stream:
        chunk_count += 1

        # Inspect chunk structure
        if chunk.choices and len(chunk.choices) > 0:
            delta = chunk.choices[0].delta

            # Check for content
            if delta.content:
                full_text += delta.content
                print(delta.content, end="", flush=True)

            # Check for finish reason
            if chunk.choices[0].finish_reason:
                print(f"\n\nFinish reason: {chunk.choices[0].finish_reason}")

    print(f"\n\nTotal chunks received: {chunk_count}")
    print(f"Full response length: {len(full_text)} characters")

def compare_streaming_vs_normal():
    """Compare streaming vs non-streaming response times"""

    import time

    print("\n" + "="*60)
    print("Comparison: Streaming vs Non-Streaming")
    print("="*60)

    message = "List 5 benefits of cloud computing."

    # Non-streaming
    print("\n1. NON-STREAMING:")
    start_time = time.time()
    response = client.chat.completions.create(
        model="gpt-5-nano",
        messages=[{"role": "user", "content": message}],
        max_tokens=10000,
        stream=False
    )
    end_time = time.time()

    print(f"   Time to first output: {end_time - start_time:.2f} seconds")
    print(f"   Response: {response.choices[0].message.content[:100]}...")

    # Streaming
    print("\n2. STREAMING:")
    start_time = time.time()
    first_chunk_time = None

    stream = client.chat.completions.create(
        model="gpt-5-nano",
        messages=[{"role": "user", "content": message}],
        max_tokens=10000,
        stream=True
    )

    for i, chunk in enumerate(stream):
        if chunk.choices and len(chunk.choices) > 0:
            if chunk.choices[0].delta.content:
                first_chunk_time = time.time()
                print(f"   Time to first output: {first_chunk_time - start_time:.2f} seconds")
                print(f"   First chunk: {chunk.choices[0].delta.content}")
                break

    print("\n   Streaming provides faster time-to-first-token!")

if __name__ == "__main__":
    try:
        # Example 1: Basic streaming
        streaming_chat()

        # Example 2: Streaming with metadata
        streaming_with_metadata()

        # Example 3: Compare streaming vs non-streaming
        compare_streaming_vs_normal()

    except Exception as e:
        print(f"\nError: {e}")
