"""
05 - JSON Mode

This example demonstrates how to get structured JSON responses from the AI.
JSON mode ensures the model outputs valid JSON that can be easily parsed
and used in your applications.

Model used: gpt-5-nano (OpenAI model with JSON support)
"""

from openai import OpenAI
import json

# Initialize the Hibana client
client = OpenAI(
    api_key="YOUR_API_KEY",
    base_url="https://api-ai.hibanacloud.com/v1"
)

def basic_json_mode():
    """Get a structured JSON response"""

    print("="*60)
    print("JSON Mode - Basic Example")
    print("="*60)

    # Request must explicitly mention JSON in the prompt
    user_message = """Create a JSON object for a book with the following fields:
    - title
    - author
    - year
    - genre
    - summary (short)

    Make up a fictional book."""

    print(f"\nUser: {user_message[:80]}...")
    print("\nRequesting JSON response from gpt-5-nano...")

    response = client.chat.completions.create(
        model="gpt-5-nano",
        messages=[
            {
                "role": "system",
                "content": "You are a helpful assistant that outputs data in JSON format."
            },
            {
                "role": "user",
                "content": user_message
            }
        ],
        temperature=0.8,
        max_tokens=10000,
        response_format={"type": "json_object"}  # Enable JSON mode
    )

    # Get the response
    json_response = response.choices[0].message.content

    print("\nRaw JSON Response:")
    print("="*60)
    print(json_response)
    print("="*60)

    # Parse and pretty-print the JSON
    try:
        parsed_json = json.loads(json_response)
        print("\nParsed JSON (pretty-printed):")
        print(json.dumps(parsed_json, indent=2, ensure_ascii=False))

        # Access specific fields
        print("\nAccessing specific fields:")
        print(f"  Title: {parsed_json.get('title', 'N/A')}")
        print(f"  Author: {parsed_json.get('author', 'N/A')}")
        print(f"  Year: {parsed_json.get('year', 'N/A')}")
    except json.JSONDecodeError as e:
        print(f"Error parsing JSON: {e}")

def json_for_data_extraction():
    """Use JSON mode to extract structured data from text"""

    print("\n" + "="*60)
    print("JSON Mode - Data Extraction")
    print("="*60)

    text_to_analyze = """
    John Smith works as a Senior Software Engineer at TechCorp Inc.
    He can be reached at john.smith@techcorp.com or by phone at +1-555-0123.
    His office is located in San Francisco, California.
    """

    prompt = f"""Extract contact information from the following text and return it as JSON:

Text: {text_to_analyze}

Return JSON with these fields: name, job_title, company, email, phone, location"""

    print(f"\nExtracting structured data from text...")

    response = client.chat.completions.create(
        model="gpt-5-nano",
        messages=[
            {
                "role": "system",
                "content": "You extract information from text and return it as JSON."
            },
            {
                "role": "user",
                "content": prompt
            }
        ],
        temperature=0.3,  # Lower temperature for more consistent extraction
        response_format={"type": "json_object"}
    )

    json_data = json.loads(response.choices[0].message.content)

    print("\nExtracted Data:")
    print(json.dumps(json_data, indent=2))

def json_array_response():
    """Get a JSON array as response"""

    print("\n" + "="*60)
    print("JSON Mode - Array Response")
    print("="*60)

    prompt = """Generate a list of 5 programming languages with their primary use cases.
    Return as JSON array where each item has 'language' and 'use_case' fields."""

    print("\nRequesting array of programming languages...")

    response = client.chat.completions.create(
        model="gpt-5-nano",
        messages=[
            {
                "role": "user",
                "content": prompt
            }
        ],
        temperature=0.7,
        response_format={"type": "json_object"}
    )

    json_result = json.loads(response.choices[0].message.content)

    print("\nProgramming Languages:")
    print(json.dumps(json_result, indent=2))

    # Process the array
    if 'languages' in json_result:
        print("\nFormatted output:")
        for i, lang in enumerate(json_result['languages'], 1):
            print(f"{i}. {lang.get('language')}: {lang.get('use_case')}")

def json_complex_structure():
    """Create a complex nested JSON structure"""

    print("\n" + "="*60)
    print("JSON Mode - Complex Nested Structure")
    print("="*60)

    prompt = """Create a JSON object representing a university course with:
    - course_id
    - course_name
    - instructor (object with name and email)
    - schedule (array of class sessions with day, time, room)
    - students (array of 3 students with name and student_id)
    - grading (object with assignments percentage, exams percentage, participation percentage)
    """

    print("\nGenerating complex JSON structure...")

    response = client.chat.completions.create(
        model="gpt-5-nano",
        messages=[
            {
                "role": "system",
                "content": "Generate realistic but fictional data in JSON format."
            },
            {
                "role": "user",
                "content": prompt
            }
        ],
        temperature=0.8,
        max_tokens=10000,
        response_format={"type": "json_object"}
    )

    complex_json = json.loads(response.choices[0].message.content)

    print("\nComplex JSON Structure:")
    print(json.dumps(complex_json, indent=2, ensure_ascii=False))

if __name__ == "__main__":
    try:
        # Example 1: Basic JSON mode
        # basic_json_mode()

        # Example 2: Data extraction to JSON
        # json_for_data_extraction()

        # Example 3: JSON array response
        json_array_response()

        # Example 4: Complex nested structure
        # json_complex_structure()

    except Exception as e:
        print(f"\nError: {e}")
