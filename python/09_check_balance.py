"""
09 - Check Balance

This example demonstrates how to check your wallet balance using
the Hibana API. This endpoint is not part of the standard OpenAI API,
so we use the requests library to call it directly.

Endpoint: GET /v1/user/balance
"""

import requests

# API Configuration
API_KEY = "YOUR_API_KEY"
BASE_URL = "https://api-ai.hibanacloud.com/v1"

def check_balance():
    """Check current wallet balance"""

    print("="*60)
    print("Check Wallet Balance")
    print("="*60)

    # Endpoint URL
    url = f"{BASE_URL}/user/balance"

    # Headers with API key
    headers = {
        "Authorization": f"Bearer {API_KEY}",
        "Content-Type": "application/json"
    }

    print("\nFetching balance...")

    try:
        # Make GET request
        response = requests.get(url, headers=headers)

        # Check if request was successful
        if response.status_code == 200:
            data = response.json()

            print("\n" + "="*60)
            print("Balance Information")
            print("="*60)

            # Display balance information
            balance = data.get('balance', 0)
            currency = data.get('currency', 'IRR')

            print(f"\nCurrent Balance: {balance:,} {currency}")

            # Format with thousands separator
            if currency == 'IRR':
                print(f"Formatted: {balance:,} Rials")

            # Estimate usage capability (example calculation)
            # Note: Actual cost depends on model and usage
            avg_cost_per_request = 100  # Example: 100 Rials per request
            estimated_requests = balance // avg_cost_per_request

            print(f"\nEstimated remaining requests: ~{estimated_requests:,}")
            print("(Based on average cost, actual may vary)")

        else:
            print(f"\nError: {response.status_code}")
            print(f"Message: {response.text}")

    except requests.exceptions.RequestException as e:
        print(f"\nRequest error: {e}")
    except Exception as e:
        print(f"\nError: {e}")

def check_balance_with_details():
    """Check balance with additional error handling and details"""

    print("\n" + "="*60)
    print("Detailed Balance Check")
    print("="*60)

    url = f"{BASE_URL}/user/balance"
    headers = {
        "Authorization": f"Bearer {API_KEY}",
        "Content-Type": "application/json"
    }

    try:
        response = requests.get(url, headers=headers, timeout=10)

        print(f"\nStatus Code: {response.status_code}")

        if response.status_code == 200:
            data = response.json()

            print("\nFull Response:")
            print("-" * 60)

            # Display all fields in response
            for key, value in data.items():
                print(f"{key}: {value}")

            print("-" * 60)

            # Check balance status
            balance = data.get('balance', 0)

            if balance > 10000:
                status = " Healthy balance"
            elif balance > 1000:
                status = "� Low balance"
            else:
                status = " Critical - Please recharge"

            print(f"\nStatus: {status}")

        elif response.status_code == 401:
            print("\n Authentication Error")
            print("Your API key may be invalid or expired.")

        elif response.status_code == 403:
            print("\n Access Forbidden")
            print("Your account may not have permission to access this endpoint.")

        else:
            print(f"\n Unexpected error: {response.status_code}")
            print(f"Response: {response.text}")

    except requests.exceptions.Timeout:
        print("\n Request timeout - Server took too long to respond")

    except requests.exceptions.ConnectionError:
        print("\n Connection error - Could not reach the server")

    except Exception as e:
        print(f"\n Error: {e}")

def monitor_balance_before_request():
    """Example: Check balance before making an API call"""

    print("\n" + "="*60)
    print("Pre-Request Balance Check")
    print("="*60)

    url = f"{BASE_URL}/user/balance"
    headers = {
        "Authorization": f"Bearer {API_KEY}",
        "Content-Type": "application/json"
    }

    # Check balance
    response = requests.get(url, headers=headers)

    if response.status_code == 200:
        data = response.json()
        balance = data.get('balance', 0)

        print(f"\nCurrent balance: {balance:,} Rials")

        # Set minimum balance threshold
        minimum_balance = 1000  # 1,000 Rials

        if balance >= minimum_balance:
            print(" Sufficient balance to proceed with API call")

            # Here you would make your actual API call
            print("\nProceed with API request...")

        else:
            print(" Insufficient balance")
            print(f"Minimum required: {minimum_balance:,} Rials")
            print("Please recharge your account before making requests.")

    else:
        print(f"Could not check balance: {response.status_code}")

def balance_check_with_usage_history():
    """Check balance and provide usage recommendations"""

    print("\n" + "="*60)
    print("Balance with Usage Recommendations")
    print("="*60)

    url = f"{BASE_URL}/user/balance"
    headers = {
        "Authorization": f"Bearer {API_KEY}",
        "Content-Type": "application/json"
    }

    response = requests.get(url, headers=headers)

    if response.status_code == 200:
        data = response.json()
        balance = data.get('balance', 0)

        print(f"\nCurrent Balance: {balance:,} Rials\n")

        # Model cost estimates (example values)
        model_costs = {
            "gpt-5-nano": 50,
            "claude-haiku-4-5": 75,
            "deepseek-chat": 40,
            "gemini-2.5-flash-lite": 45,
            "dall-e-3": 5000
        }

        print("Estimated requests per model:")
        print("-" * 60)

        for model, cost in model_costs.items():
            requests_possible = balance // cost
            print(f"{model:<25} ~{requests_possible:>6} requests")

        print("-" * 60)

        # Recommendations
        print("\nRecommendations:")
        if balance < 5000:
            print("  Consider recharging soon")
            print("  Use cost-efficient models (gpt-5-nano, deepseek-chat)")
        elif balance < 20000:
            print("  Balance is moderate")
            print("  Monitor usage regularly")
        else:
            print("  Balance is healthy")
            print("  You can use any model freely")

    else:
        print(f"Error checking balance: {response.status_code}")

if __name__ == "__main__":
    try:
        # Example 1: Simple balance check
        check_balance()

        # Example 2: Detailed balance check
        check_balance_with_details()

        # Example 3: Pre-request balance check
        monitor_balance_before_request()

        # Example 4: Balance with usage recommendations
        balance_check_with_usage_history()

    except Exception as e:
        print(f"\nError: {e}")
