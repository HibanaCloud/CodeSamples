"""
10 - Error Handling

This example demonstrates proper error handling when using the Hibana API.
It covers common errors, retry strategies, and best practices for building
robust applications.
"""

from openai import OpenAI
import openai
import time

# Initialize the Hibana client
client = OpenAI(
    api_key="YOUR_API_KEY",
    base_url="https://api-ai.hibanacloud.com/v1"
)

def handle_authentication_error():
    """Handle authentication errors (invalid API key)"""

    print("="*60)
    print("Error Handling - Authentication")
    print("="*60)

    # Create client with invalid API key
    bad_client = OpenAI(
        api_key="INVALID_KEY_123",
        base_url="https://api-ai.hibanacloud.com/v1"
    )

    try:
        response = bad_client.chat.completions.create(
            model="gpt-5-nano",
            messages=[{"role": "user", "content": "Hello"}]
        )
    except openai.AuthenticationError as e:
        print("\n Authentication Error Caught!")
        print(f"Error message: {e}")
        print("\nSolution:")
        print("  1. Check that your API key is correct")
        print("  2. Verify the key is active in your dashboard")
        print("  3. Ensure proper Bearer token format")

def handle_model_not_found():
    """Handle errors when requesting unavailable models"""

    print("\n" + "="*60)
    print("Error Handling - Model Not Found")
    print("="*60)

    try:
        response = client.chat.completions.create(
            model="non-existent-model-xyz",
            messages=[{"role": "user", "content": "Hello"}]
        )
    except openai.NotFoundError as e:
        print("\n Model Not Found Error!")
        print(f"Error: {e}")
        print("\nSolution:")
        print("  1. Use client.models.list() to see available models")
        print("  2. Check for typos in model name")
        print("  3. Verify model is enabled in your account")

def handle_rate_limit():
    """Handle rate limiting errors"""

    print("\n" + "="*60)
    print("Error Handling - Rate Limiting")
    print("="*60)

    print("\nSimulating rate limit scenario...")

    try:
        # This might trigger rate limiting if called too frequently
        for i in range(5):
            response = client.chat.completions.create(
                model="gpt-5-nano",
                messages=[{"role": "user", "content": f"Request {i+1}"}],
                max_tokens=10000
            )
            print(f"Request {i+1}: Success")
            time.sleep(0.1)  # Small delay

    except openai.RateLimitError as e:
        print(f"\n Rate Limit Error: {e}")
        print("\nSolution:")
        print("  1. Implement exponential backoff")
        print("  2. Add delays between requests")
        print("  3. Monitor rate limits for your tier")
        print("  4. Consider upgrading your plan")

def handle_insufficient_balance():
    """Handle insufficient balance errors"""

    print("\n" + "="*60)
    print("Error Handling - Insufficient Balance")
    print("="*60)

    print("\nChecking for balance errors...")

    try:
        # This would fail if balance is insufficient
        response = client.chat.completions.create(
            model="gpt-5-nano",
            messages=[{"role": "user", "content": "Test"}],
            max_tokens=10000
        )
        print(" Request successful - sufficient balance")

    except openai.APIError as e:
        if "insufficient" in str(e).lower() or "balance" in str(e).lower():
            print(f"\n Insufficient Balance: {e}")
            print("\nSolution:")
            print("  1. Check balance: GET /v1/user/balance")
            print("  2. Recharge your account")
            print("  3. Use lower-cost models")

def retry_with_exponential_backoff():
    """Implement retry logic with exponential backoff"""

    print("\n" + "="*60)
    print("Retry Strategy - Exponential Backoff")
    print("="*60)

    max_retries = 3
    base_delay = 1  # seconds

    for attempt in range(max_retries):
        try:
            print(f"\nAttempt {attempt + 1}/{max_retries}...")

            response = client.chat.completions.create(
                model="gpt-5-nano",
                messages=[{"role": "user", "content": "Hello"}],
                max_tokens=10000
            )

            print(" Success!")
            print(f"Response: {response.choices[0].message.content}")
            break

        except openai.RateLimitError as e:
            if attempt < max_retries - 1:
                delay = base_delay * (2 ** attempt)  # Exponential backoff
                print(f" Rate limited. Retrying in {delay} seconds...")
                time.sleep(delay)
            else:
                print(f" Failed after {max_retries} attempts")
                raise

        except openai.APIError as e:
            print(f" API Error: {e}")
            if attempt < max_retries - 1:
                delay = base_delay * (2 ** attempt)
                print(f"Retrying in {delay} seconds...")
                time.sleep(delay)
            else:
                raise

def handle_timeout():
    """Handle request timeout errors"""

    print("\n" + "="*60)
    print("Error Handling - Timeouts")
    print("="*60)

    # Set a short timeout for demonstration
    timeout_client = OpenAI(
        api_key="YOUR_API_KEY",
        base_url="https://api-ai.hibanacloud.com/v1",
        timeout=0.001  # Very short timeout (will likely fail)
    )

    try:
        response = timeout_client.chat.completions.create(
            model="gpt-5-nano",
            messages=[{"role": "user", "content": "Hello"}]
        )
    except openai.APITimeoutError as e:
        print(f"\n Timeout Error: {e}")
        print("\nSolution:")
        print("  1. Increase timeout value")
        print("  2. Check network connection")
        print("  3. Try again later if server is slow")

def comprehensive_error_handler():
    """A comprehensive error handling wrapper"""

    print("\n" + "="*60)
    print("Comprehensive Error Handler")
    print("="*60)

    def safe_api_call(model, messages, **kwargs):
        """Wrapper function with comprehensive error handling"""

        try:
            response = client.chat.completions.create(
                model=model,
                messages=messages,
                **kwargs
            )
            return response

        except openai.AuthenticationError as e:
            print(f" Authentication Error: {e}")
            print("Check your API key.")
            return None

        except openai.NotFoundError as e:
            print(f" Not Found: {e}")
            print("Model may not exist or be unavailable.")
            return None

        except openai.RateLimitError as e:
            print(f" Rate Limit: {e}")
            print("Too many requests. Please wait and try again.")
            return None

        except openai.APITimeoutError as e:
            print(f" Timeout: {e}")
            print("Request took too long. Try again.")
            return None

        except openai.BadRequestError as e:
            print(f" Bad Request: {e}")
            print("Check your request parameters.")
            return None

        except openai.APIError as e:
            print(f" API Error: {e}")
            print("An error occurred on the server side.")
            return None

        except Exception as e:
            print(f" Unexpected Error: {e}")
            return None

    # Test the wrapper
    print("\nTesting with valid request:")
    result = safe_api_call(
        model="gpt-5-nano",
        messages=[{"role": "user", "content": "Say hello in 3 words"}],
        max_tokens=10000
    )

    if result:
        print(f" Success: {result.choices[0].message.content}")
    else:
        print("Request failed gracefully")

def validate_before_request():
    """Validate inputs before making API request"""

    print("\n" + "="*60)
    print("Input Validation")
    print("="*60)

    def validate_and_call(model, user_input):
        """Validate inputs before API call"""

        # Validate model name
        valid_models = ["gpt-5-nano", "claude-haiku-4-5", "deepseek-chat", "gemini-2.5-flash-lite"]

        if model not in valid_models:
            print(f" Invalid model: {model}")
            print(f"Valid models: {', '.join(valid_models)}")
            return None

        # Validate user input
        if not user_input or not user_input.strip():
            print(" Empty user input")
            return None

        if len(user_input) > 10000:
            print(" Input too long (max 10,000 characters)")
            return None

        # All validations passed
        print(" Validation passed")

        try:
            response = client.chat.completions.create(
                model=model,
                messages=[{"role": "user", "content": user_input}],
                max_tokens=10000
            )
            return response
        except Exception as e:
            print(f" Error: {e}")
            return None

    # Test validation
    print("\nTest 1: Invalid model")
    validate_and_call("invalid-model", "Hello")

    print("\nTest 2: Empty input")
    validate_and_call("gpt-5-nano", "")

    print("\nTest 3: Valid request")
    result = validate_and_call("gpt-5-nano", "Say hi")
    if result:
        print(f"Response: {result.choices[0].message.content}")

if __name__ == "__main__":
    try:
        # Example 1: Authentication errors
        # handle_authentication_error()  # Uncomment to test

        # Example 2: Model not found
        # handle_model_not_found()  # Uncomment to test

        # Example 3: Rate limiting
        # handle_rate_limit()  # Uncomment to test

        # Example 4: Insufficient balance
        handle_insufficient_balance()

        # Example 5: Retry with backoff
        retry_with_exponential_backoff()

        # Example 6: Timeout handling
        # handle_timeout()  # Uncomment to test

        # Example 7: Comprehensive handler
        comprehensive_error_handler()

        # Example 8: Input validation
        validate_before_request()

        print("\n" + "="*60)
        print("Error handling examples completed!")
        print("="*60)

    except Exception as e:
        print(f"\nUnexpected error in main: {e}")
