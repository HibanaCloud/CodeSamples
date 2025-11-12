"""
02 - Chat with System Prompt

This example demonstrates how to use system prompts to control
the AI's behavior, tone, and expertise. System prompts set the
context and guidelines for how the assistant should respond.

Model used: claude-haiku-4-5 (Claude Haiku - Fast Anthropic model)
"""

from openai import OpenAI

# Initialize the Hibana client
client = OpenAI(
    api_key="YOUR_API_KEY",
    base_url="https://api-ai.hibanacloud.com/v1"
)

def chat_with_system_prompt():
    """Demonstrate using system prompts to control AI behavior"""

    # Define a custom system prompt
    system_prompt = """You are a English-speaking AI assistant specializing in
explaining technical concepts in simple terms. Always respond in English
and use analogies from everyday life to explain complex topics. Be friendly and
encouraging."""

    user_question = "What is machine learning and how does it work?"

    print("Sending message to claude-haiku-4-5 with custom system prompt...")
    print(f"\nSystem Prompt: {system_prompt[:100]}...")
    print(f"User Question: {user_question}")

    # Create chat completion with system prompt
    response = client.chat.completions.create(
        model="claude-haiku-4-5",  # Anthropic's Claude Haiku model
        messages=[
            {
                "role": "system",  # System message defines AI behavior
                "content": system_prompt
            },
            {
                "role": "user",
                "content": user_question
            }
        ],
        temperature=0.7,  # Slightly lower for more consistent responses
        max_tokens=10000
    )

    # Extract and display response
    assistant_message = response.choices[0].message.content

    print("\n" + "="*60)
    print("Response from claude-haiku-4-5:")
    print("="*60)
    print(assistant_message)
    print("="*60)

    # Display usage statistics
    if response.usage:
        print(f"\nToken Usage:")
        print(f"  Total tokens: {response.usage.total_tokens}")
        if hasattr(response.usage, 'cost_rial'):
            print(f"  Cost: {response.usage.cost_rial} Rials")

def compare_with_without_system():
    """Compare responses with and without system prompt"""

    user_message = "Explain quantum computing"

    print("\n" + "="*60)
    print("Comparison: With vs Without System Prompt")
    print("="*60)

    # Response WITHOUT system prompt
    print("\n1. WITHOUT system prompt:")
    response1 = client.chat.completions.create(
        model="gpt-5-nano",
        messages=[{"role": "user", "content": user_message}],
        max_tokens=10000
    )
    print(response1.choices[0].message.content)

    # Response WITH system prompt
    print("\n2. WITH system prompt (explain like I'm 10 years old):")
    response2 = client.chat.completions.create(
        model="gpt-5-nano",
        messages=[
            {
                "role": "system",
                "content": "Explain everything as if talking to a 10-year-old child. Use simple words and fun examples."
            },
            {
                "role": "user",
                "content": user_message
            }
        ],
        max_tokens=10000
    )
    print(response2.choices[0].message.content)

if __name__ == "__main__":
    try:
        # Example 1: System prompt for Persian responses
        chat_with_system_prompt()

        # Example 2: Comparison with/without system prompt
        compare_with_without_system()

    except Exception as e:
        print(f"Error: {e}")
