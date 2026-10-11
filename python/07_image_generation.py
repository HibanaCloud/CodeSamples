"""
07 - Image Generation

This example generates an image from a text prompt with the Hibana API,
using the OpenAI Python SDK's images.generate().

- `size` is an ASPECT RATIO such as "1:1", "16:9" or "9:16"
  (all ten are listed at https://hibanacloud.ir/docs/dalle).
- The response is a temporary URL that expires after about an hour,
  so the example downloads the image right away.

Model used: gemini-3.1-flash-lite-image
"""

import os
import urllib.request
from openai import OpenAI

# Reads your Hibana API key from the HIBANA_API_KEY environment variable
client = OpenAI(
    api_key=os.environ.get("HIBANA_API_KEY", "YOUR_API_KEY"),
    base_url="https://api-ai.hibanacloud.ir/v1"
)

MODEL = "gemini-3.1-flash-lite-image"


def generate_image(prompt, size, filename):
    """Generate one image and save it next to this script"""

    print("=" * 60)
    print(f"Image Generation - {MODEL} ({size})")
    print("=" * 60)
    print(f"\nPrompt: {prompt}")
    print("\nGenerating...")

    response = client.images.generate(
        model=MODEL,
        prompt=prompt,
        size=size,
    )

    url = response.data[0].url
    print(f"\nTemporary URL (valid for about an hour):\n{url}")

    urllib.request.urlretrieve(url, filename)
    print(f"\nSaved to {filename}")


if __name__ == "__main__":
    generate_image(
        prompt="A serene landscape of the Alborz mountains in Iran at sunset, "
               "with snow-capped peaks and golden light",
        size="16:9",
        filename="alborz_16x9.png",
    )
