"""
01 - Simple Chat Completion

This example demonstrates the most basic usage of Hibana API
using the OpenAI Python SDK. It shows how to:
- Initialize the client with Hibana's base URL
- Send a simple chat completion request
- Receive and display the response

Model used: gpt-5-nano (Fast and efficient GPT model)
"""

from openai import OpenAI

# Initialize the Hibana client
# Replace "YOUR_API_KEY" with your actual Hibana API key
client = OpenAI(
    api_key="YOUR_API_KEY",
    base_url="https://api-ai.hibanacloud.com/v1"
)

def simple_chat():
    """Send a simple chat message and get a response"""

    print("Sending message to gpt-5-nano...")

    # Create a chat completion
    response = client.chat.completions.create(
        model="gpt-5-nano",  # Fast OpenAI model
        messages=[
            {
                "role": "user",
                "content": "Hello! Please introduce yourself in one sentence."
            }
        ],
        temperature=1.0,  # Controls randomness (0.0 = deterministic, 2.0 = very random)
        max_tokens=10000    # Maximum tokens in the response
    )

    # Extract the response
    assistant_message = response.choices[0].message.content
    finish_reason = response.choices[0].finish_reason

    # Display the response
    print("\n" + "="*60)
    print("Response from gpt-5-nano:")
    print("="*60)
    print(assistant_message)
    print("="*60)
    print(f"\nFinish reason: {finish_reason}")

    # Display token usage
    if response.usage:
        print(f"\nToken Usage:")
        print(f"  Prompt tokens: {response.usage.prompt_tokens}")
        print(f"  Completion tokens: {response.usage.completion_tokens}")
        print(f"  Total tokens: {response.usage.total_tokens}")
        if hasattr(response.usage, 'cost_rial'):
            print(f"  Cost: {response.usage.cost_rial} Rials")

if __name__ == "__main__":
    try:
        simple_chat()
    except Exception as e:
        print(f"Error: {e}")
