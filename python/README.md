# Hibana API - Python Samples

Python code samples demonstrating how to use the Hibana AI API with the OpenAI Python SDK.

## Prerequisites

- Python 3.7 or higher
- pip (Python package manager)

## Installation

1. Install dependencies:

```bash
pip install -r requirements.txt
```

This will install:
- `openai>=1.0.0` - Official OpenAI Python SDK
- `requests` - For custom API endpoints (balance checking)

## Configuration

All samples use the following configuration:

```python
from openai import OpenAI

client = OpenAI(
    api_key="YOUR_API_KEY",  # Replace with your Hibana API key
    base_url="https://api-ai.hibanacloud.ir/v1"
)
```

**Important:** Replace `"YOUR_API_KEY"` with your actual Hibana API key before running the samples.

## Available Samples

### Basic Examples

| File | Description | Model |
|------|-------------|-------|
| `01_simple_chat.py` | Basic chat completion | gpt-5-nano |
| `02_chat_with_system_prompt.py` | Custom system instructions | claude-haiku-4-5, gpt-5-nano |
| `03_multi_turn_conversation.py` | Multi-message conversations | deepseek-v4-flash |

### Advanced Features

| File | Description | Model |
|------|-------------|-------|
| `04_streaming_response.py` | Real-time streaming responses | gpt-5-nano, gemini-3.7-flash |
| `05_json_mode.py` | Structured JSON output | gpt-5-nano |
| `06_responses_api_simple.py` | Simplified Responses API | All 4 providers |

### Additional Capabilities

| File | Description | Model |
|------|-------------|-------|
| `07_image_generation.py` | Image generation (size = aspect ratio, URL saved to a file) | gemini-3.1-flash-lite-image |
| `08_list_models.py` | List available models | N/A |
| `09_check_balance.py` | Check wallet balance | N/A |

### Best Practices

| File | Description | Model |
|------|-------------|-------|
| `10_error_handling.py` | Error handling & retry logic | gpt-5-nano |
| `11_multiple_providers.py` | Compare all 4 providers | gpt-5-nano, claude-haiku-4-5, deepseek-v4-flash, gemini-3.7-flash |
| `12_embeddings_search.py` | Semantic search in Persian with embeddings | bge-m3 |
| `13_cutoff_fallback.py` | Fall back to a domestic model during an internet cutoff | gpt-5-nano → gpt-oss |

## Running the Samples

Run any sample directly with Python:

```bash
python 01_simple_chat.py
```

Or from the CodeSamples root:

```bash
python python/01_simple_chat.py
```

## Supported Models

| Provider | Model ID | Description |
|----------|----------|-------------|
| **OpenAI** | `gpt-5-nano` | Fast and efficient GPT model |
| **Anthropic** | `claude-haiku-4-5` | Claude Haiku - Fast responses |
| **DeepSeek** | `deepseek-v4-flash` | DeepSeek conversational model |
| **Hibana (domestic)** | `gpt-oss` | Runs on Hibana's servers; available during internet cutoffs |
| **Hibana (domestic)** | `bge-m3` | Embeddings, 1024 dimensions |
| **Google** | `gemini-3.7-flash` | Gemini Flash |
| **Google** | `gemini-3.1-flash-lite-image` | Image generation |

## Common Configuration

Most samples use these default values (tested and verified):

```python
{
    "temperature": 0.7,      # Balanced randomness
    "max_tokens": 10000,     # Maximum response length
}
```

Variations:
- **Coding tasks**: `temperature=0.3` (more deterministic)
- **Creative writing**: `temperature=1.0` (more random)
- **DeepSeek conversations**: `max_tokens=8192`

## Troubleshooting

### ModuleNotFoundError: No module named 'openai'

Install the required dependencies:
```bash
pip install -r requirements.txt
```

### Authentication Error

Ensure you've replaced `"YOUR_API_KEY"` with your actual Hibana API key from the dashboard.

### Rate Limit Errors

See `10_error_handling.py` for examples of handling rate limits with exponential backoff.

## Documentation

- [Back to main README](../README.md)
- [JavaScript samples](../javascript/README.md)
- Hibana Dashboard: https://hibanacloud.ir
