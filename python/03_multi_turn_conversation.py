"""
03 - Multi-Turn Conversation

This example demonstrates how to maintain conversation context
across multiple messages. The AI can reference previous messages
and maintain a coherent conversation flow.

Model used: deepseek-chat (DeepSeek conversational model)
"""

from openai import OpenAI

# Initialize the Hibana client
client = OpenAI(
    api_key="YOUR_API_KEY",
    base_url="https://api-ai.hibanacloud.com/v1"
)

def multi_turn_conversation():
    """Demonstrate a multi-turn conversation with context"""

    # Conversation history - this maintains context across turns
    conversation_history = [
        {
            "role": "system",
            "content": "You are a helpful programming tutor specializing in Python."
        },
        {
            "role": "user",
            "content": "Hi! I'm learning Python. Can you help me?"
        }
    ]

    print("="*60)
    print("Multi-Turn Conversation with deepseek-chat")
    print("="*60)

    # First exchange
    print("\nUser: Hi! I'm learning Python. Can you help me?")

    response1 = client.chat.completions.create(
        model="deepseek-chat",
        messages=conversation_history,
        temperature=0.8,
        max_tokens=8192
    )

    assistant_reply1 = response1.choices[0].message.content
    print(f"\nAssistant: {assistant_reply1}")

    # Add assistant's response to conversation history
    conversation_history.append({
        "role": "assistant",
        "content": assistant_reply1
    })

    # Second turn - ask a follow-up question
    user_message2 = "What's the difference between a list and a tuple?"
    conversation_history.append({
        "role": "user",
        "content": user_message2
    })

    print(f"\nUser: {user_message2}")

    response2 = client.chat.completions.create(
        model="deepseek-chat",
        messages=conversation_history,
        temperature=0.8,
        max_tokens=8192
    )

    assistant_reply2 = response2.choices[0].message.content
    print(f"\nAssistant: {assistant_reply2}")

    # Add to history for potential future turns
    conversation_history.append({
        "role": "assistant",
        "content": assistant_reply2
    })

    # Third turn - reference previous context
    user_message3 = "Can you show me an example of each?"
    conversation_history.append({
        "role": "user",
        "content": user_message3
    })

    print(f"\nUser: {user_message3}")

    response3 = client.chat.completions.create(
        model="deepseek-chat",
        messages=conversation_history,
        temperature=0.8,
        max_tokens=8192
    )

    assistant_reply3 = response3.choices[0].message.content
    print(f"\nAssistant: {assistant_reply3}")

    print("\n" + "="*60)
    print(f"Total messages in conversation: {len(conversation_history) + 1}")
    print("="*60)

    # Display total usage
    if response3.usage:
        print(f"\nLast request token usage:")
        print(f"  Total tokens: {response3.usage.total_tokens}")

def interactive_conversation():
    """Interactive conversation loop (commented for documentation)"""

    # Uncomment this function to enable interactive mode
    print("\n" + "="*60)
    print("Interactive mode (demonstration - not running)")
    print("="*60)
    print("""
    # To create an interactive chatbot, use this pattern:

    conversation = [{"role": "system", "content": "You are a helpful assistant."}]

    while True:
        user_input = input("You: ")
        if user_input.lower() in ['exit', 'quit', 'bye']:
            break

        conversation.append({"role": "user", "content": user_input})

        response = client.chat.completions.create(
            model="deepseek-chat",
            messages=conversation,
            temperature=0.8
        )

        assistant_message = response.choices[0].message.content
        print(f"Assistant: {assistant_message}")

        conversation.append({"role": "assistant", "content": assistant_message})
    """)

if __name__ == "__main__":
    try:
        # Run multi-turn conversation example
        multi_turn_conversation()

        # Show interactive conversation pattern
        interactive_conversation()

    except Exception as e:
        print(f"Error: {e}")
