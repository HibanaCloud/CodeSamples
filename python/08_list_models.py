"""
08 - List Available Models

This example demonstrates how to retrieve all available models
from the Hibana API. This is useful for discovering what models
are enabled for your account.

Endpoint: GET /v1/models
"""

from openai import OpenAI
from datetime import datetime

# Initialize the Hibana client
client = OpenAI(
    api_key="YOUR_API_KEY",
    base_url="https://api-ai.hibanacloud.com/v1"
)

def list_all_models():
    """List all available models"""

    print("="*60)
    print("Listing All Available Models")
    print("="*60)

    # Get list of models
    models = client.models.list()

    print(f"\nTotal models available: {len(models.data)}\n")

    # Display each model
    for i, model in enumerate(models.data, 1):
        print(f"{i}. {model.id}")
        print(f"   Owner: {model.owned_by}")
        if hasattr(model, 'created'):
            created_date = datetime.fromtimestamp(model.created)
            print(f"   Created: {created_date.strftime('%Y-%m-%d')}")
        print()

def group_models_by_provider():
    """Group models by their provider (OpenAI, Anthropic, etc.)"""

    print("="*60)
    print("Models Grouped by Provider")
    print("="*60)

    models = client.models.list()

    # Group models by owner/provider
    grouped = {}
    for model in models.data:
        provider = model.owned_by
        if provider not in grouped:
            grouped[provider] = []
        grouped[provider].append(model.id)

    # Display grouped models
    for provider, model_list in grouped.items():
        print(f"\n{provider.upper()}")
        print("-" * 40)
        for model_id in model_list:
            print(f"  {model_id}")

    print(f"\nTotal providers: {len(grouped)}")

def filter_models_by_type():
    """Filter and categorize models by their type"""

    print("\n" + "="*60)
    print("Models by Type")
    print("="*60)

    models = client.models.list()

    # Categorize models
    chat_models = []
    image_models = []
    other_models = []

    for model in models.data:
        model_id = model.id.lower()

        if 'dall-e' in model_id or 'dalle' in model_id:
            image_models.append(model.id)
        elif any(x in model_id for x in ['gpt', 'claude', 'deepseek', 'gemini']):
            chat_models.append(model.id)
        else:
            other_models.append(model.id)

    # Display categorized models
    print("\nCHAT/TEXT MODELS:")
    print("-" * 40)
    for model in chat_models:
        print(f"   {model}")

    print("\n\nIMAGE GENERATION MODELS:")
    print("-" * 40)
    for model in image_models:
        print(f"   {model}")

    if other_models:
        print("\n\nOTHER MODELS:")
        print("-" * 40)
        for model in other_models:
            print(f"   {model}")

def get_specific_model_info():
    """Get information about a specific model"""

    print("\n" + "="*60)
    print("Specific Model Information")
    print("="*60)

    model_id = "gpt-5-nano"

    print(f"\nQuerying model: {model_id}")

    try:
        # Retrieve specific model
        model = client.models.retrieve(model_id)

        print("\nModel Details:")
        print("-" * 40)
        print(f"ID: {model.id}")
        print(f"Object: {model.object}")
        print(f"Owned by: {model.owned_by}")

        if hasattr(model, 'created'):
            created_date = datetime.fromtimestamp(model.created)
            print(f"Created: {created_date.strftime('%Y-%m-%d %H:%M:%S')}")

        if hasattr(model, 'permission'):
            print(f"Permissions: {len(model.permission)} permission(s)")

    except Exception as e:
        print(f"Error retrieving model: {e}")

def check_model_availability():
    """Check if specific models are available"""

    print("\n" + "="*60)
    print("Model Availability Check")
    print("="*60)

    # Models to check
    models_to_check = [
        "gpt-5-nano",
        "claude-haiku-4-5",
        "deepseek-chat",
        "gemini-2.5-flash-lite",
        "dall-e-3"
    ]

    print("\nChecking availability...\n")

    # Get all available models
    available_models = client.models.list()
    available_ids = [m.id for m in available_models.data]

    # Check each model
    for model_id in models_to_check:
        is_available = model_id in available_ids
        status = " Available" if is_available else " Not Available"
        print(f"{status.ljust(20)} {model_id}")

def display_models_table():
    """Display models in a formatted table"""

    print("\n" + "="*60)
    print("Models Table")
    print("="*60)

    models = client.models.list()

    # Table header
    print(f"\n{'Model ID':<40} {'Provider':<15} {'Type':<10}")
    print("-" * 70)

    # Table rows
    for model in models.data:
        model_id = model.id
        provider = model.owned_by

        # Determine type
        if 'dall-e' in model_id.lower():
            model_type = "Image"
        elif any(x in model_id.lower() for x in ['gpt', 'claude', 'deepseek', 'gemini']):
            model_type = "Chat"
        else:
            model_type = "Other"

        print(f"{model_id:<40} {provider:<15} {model_type:<10}")

    print("\n")

if __name__ == "__main__":
    try:
        # Example 1: List all models
        list_all_models()

        # Example 2: Group by provider
        group_models_by_provider()

        # Example 3: Filter by type
        filter_models_by_type()

        # Example 4: Get specific model info
        get_specific_model_info()

        # Example 5: Check availability
        check_model_availability()

        # Example 6: Display as table
        display_models_table()

    except Exception as e:
        print(f"\nError: {e}")
