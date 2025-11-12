"""
07 - Image Generation

This example demonstrates how to generate images using DALL-E models
through the Hibana API. The API is fully compatible with OpenAI's
image generation endpoints.

Model used: dall-e-3 (OpenAI's latest image generation model)
"""

from openai import OpenAI
import json

# Initialize the Hibana client
client = OpenAI(
    api_key="YOUR_API_KEY",
    base_url="https://api-ai.hibanacloud.com/v1"
)

def simple_image_generation():
    """Generate a simple image with DALL-E 3"""

    print("="*60)
    print("Image Generation - DALL-E 3")
    print("="*60)

    prompt = "A serene landscape of the Alborz mountains in Iran at sunset, with snow-capped peaks and golden light"

    print(f"\nPrompt: {prompt}")
    print("\nGenerating image with DALL-E 3...")

    response = client.images.generate(
        model="dall-e-3",
        prompt=prompt,
        size="1024x1024",  # DALL-E 3 sizes: 1024x1024, 1024x1792, 1792x1024
        quality="standard",  # "standard" or "hd"
        n=1  # Number of images (DALL-E 3 only supports 1)
    )

    # Get the image URL
    image_url = response.data[0].url

    print("\n" + "="*60)
    print("Image generated successfully!")
    print("="*60)
    print(f"\nImage URL: {image_url}")
    print(f"\nRevised Prompt: {response.data[0].revised_prompt}")

    print("\nYou can:")
    print("  1. Open the URL in a browser")
    print("  2. Download the image programmatically")
    print("  3. Use it in your application")

def generate_with_different_sizes():
    """Generate images with different aspect ratios"""

    print("\n" + "="*60)
    print("Image Generation - Different Sizes")
    print("="*60)

    prompt = "A minimalist logo for a tech startup, featuring a stylized AI brain"

    sizes = ["1024x1024", "1024x1792", "1792x1024"]

    print(f"\nPrompt: {prompt}")
    print("\nGenerating images in different sizes...\n")

    for size in sizes:
        print(f"Generating {size}...")

        response = client.images.generate(
            model="dall-e-3",
            prompt=prompt,
            size=size,
            quality="standard"
        )

        print(f"  URL: {response.data[0].url}")

def generate_hd_quality():
    """Generate high-quality image with DALL-E 3"""

    print("\n" + "="*60)
    print("Image Generation - HD Quality")
    print("="*60)

    prompt = "A photorealistic portrait of a Persian cat wearing a tiny crown, studio lighting, highly detailed"

    print(f"\nPrompt: {prompt}")
    print("\nGenerating HD quality image (takes longer, costs more)...")

    response = client.images.generate(
        model="dall-e-3",
        prompt=prompt,
        size="1024x1024",
        quality="hd",  # Higher quality, more detailed
        style="natural"  # "vivid" or "natural"
    )

    print("\nHD Image generated!")
    print(f"URL: {response.data[0].url}")

def generate_with_style():
    """Compare vivid vs natural styles"""

    print("\n" + "="*60)
    print("Image Generation - Style Comparison")
    print("="*60)

    prompt = "A futuristic city with flying cars and neon lights"

    styles = ["vivid", "natural"]

    print(f"\nPrompt: {prompt}")
    print("\nGenerating with different styles...\n")

    for style in styles:
        print(f"Style: {style}")

        response = client.images.generate(
            model="dall-e-3",
            prompt=prompt,
            size="1024x1024",
            quality="standard",
            style=style
        )

        print(f"  URL: {response.data[0].url}")
        print(f"  Revised: {response.data[0].revised_prompt[:80]}...")
        print()

def download_image():
    """Generate and download an image to local file"""

    print("\n" + "="*60)
    print("Image Generation - Download to File")
    print("="*60)

    import requests
    from datetime import datetime

    prompt = "A beautiful Persian garden with traditional architecture and fountains"

    print(f"\nPrompt: {prompt}")
    print("\nGenerating and downloading image...")

    # Generate image
    response = client.images.generate(
        model="dall-e-3",
        prompt=prompt,
        size="1024x1024",
        quality="standard"
    )

    image_url = response.data[0].url

    # Download the image
    image_response = requests.get(image_url)

    if image_response.status_code == 200:
        # Save to file with timestamp
        timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
        filename = f"generated_image_{timestamp}.png"

        with open(filename, 'wb') as f:
            f.write(image_response.content)

        print(f"\nImage downloaded successfully!")
        print(f"Saved as: {filename}")
        print(f"File size: {len(image_response.content)} bytes")
    else:
        print(f"Failed to download image: {image_response.status_code}")

def batch_image_generation():
    """Generate multiple images with different prompts"""

    print("\n" + "="*60)
    print("Image Generation - Batch Processing")
    print("="*60)

    prompts = [
        "A coffee cup on a wooden table, morning light",
        "Abstract geometric patterns in blue and gold",
        "A cozy reading nook with bookshelves"
    ]

    print("\nGenerating multiple images...\n")

    results = []

    for i, prompt in enumerate(prompts, 1):
        print(f"{i}. Generating: {prompt}")

        try:
            response = client.images.generate(
                model="dall-e-3",
                prompt=prompt,
                size="1024x1024",
                quality="standard"
            )

            results.append({
                "prompt": prompt,
                "url": response.data[0].url,
                "revised_prompt": response.data[0].revised_prompt
            })

            print(f"    Success")

        except Exception as e:
            print(f"    Error: {e}")

    print("\n" + "="*60)
    print("Batch Results:")
    print("="*60)
    print(json.dumps(results, indent=2, ensure_ascii=False))

if __name__ == "__main__":
    try:
        # Example 1: Simple generation
        simple_image_generation()

        # Example 2: Different sizes
        generate_with_different_sizes()

        # Example 3: HD quality
        generate_hd_quality()

        # Example 4: Style comparison
        generate_with_style()

        # Example 5: Download image (commented to avoid file creation)
        # Uncomment to actually download:
        # download_image()

        # Example 6: Batch generation
        # batch_image_generation()

        print("\n" + "="*60)
        print("Image generation examples completed!")
        print("="*60)

    except Exception as e:
        print(f"\nError: {e}")
