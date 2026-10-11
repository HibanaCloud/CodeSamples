# Hibana API - JavaScript/Node.js Samples

JavaScript/Node.js code samples demonstrating how to use the Hibana AI API with the OpenAI Node.js SDK.

## Prerequisites

- Node.js 18.0 or higher
- npm (Node package manager)

## Installation

1. Install dependencies:

```bash
npm install
```

This will install:
- `openai` - Official OpenAI Node.js SDK
- `axios` - For custom API endpoints (balance checking)

## Configuration

All samples use the following configuration:

```javascript
import OpenAI from 'openai';

const client = new OpenAI({
    apiKey: "YOUR_API_KEY",  // Replace with your Hibana API key
    baseURL: "https://api-ai.hibanacloud.ir/v1"
});
```

**Important:** Replace `"YOUR_API_KEY"` with your actual Hibana API key before running the samples.

## Available Samples

### Basic Examples

| File | Description | Model |
|------|-------------|-------|
| `01_simple_chat.js` | Basic chat completion | gpt-5-nano |
| `02_chat_with_system_prompt.js` | Custom system instructions | claude-haiku-4-5, gpt-5-nano |
| `03_multi_turn_conversation.js` | Multi-message conversations | deepseek-v4-flash |

### Advanced Features

| File | Description | Model |
|------|-------------|-------|
| `04_streaming_response.js` | Real-time streaming responses | gpt-5-nano, gemini-3.7-flash |
| `05_json_mode.js` | Structured JSON output | gpt-5-nano |
| `06_responses_api_simple.js` | Simplified Responses API | All 4 providers |

### Additional Capabilities

| File | Description | Model |
|------|-------------|-------|
| `07_image_generation.js` | Image generation (size = aspect ratio, URL saved to a file) | gemini-3.1-flash-lite-image |
| `08_list_models.js` | List available models | N/A |
| `09_check_balance.js` | Check wallet balance | N/A |

### Best Practices

| File | Description | Model |
|------|-------------|-------|
| `10_error_handling.js` | Error handling & retry logic | gpt-5-nano |
| `11_multiple_providers.js` | Compare all 4 providers | gpt-5-nano, claude-haiku-4-5, deepseek-v4-flash, gemini-3.7-flash |
| `12_embeddings_search.js` | Semantic search in Persian with embeddings | bge-m3 |
| `13_cutoff_fallback.js` | Fall back to a domestic model during an internet cutoff | gpt-5-nano → gpt-oss |

## Running the Samples

Run any sample directly with Node.js:

```bash
node 01_simple_chat.js
```

Or from the CodeSamples root:

```bash
node javascript/01_simple_chat.js
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

```javascript
{
    temperature: 0.7,      // Balanced randomness
    max_tokens: 10000,     // Maximum response length
}
```

Variations:
- **Coding tasks**: `temperature: 0.3` (more deterministic)
- **Creative writing**: `temperature: 1.0` (more random)
- **DeepSeek conversations**: `max_tokens: 8192`

## ES Modules

This project uses ES modules (`"type": "module"` in package.json). Use `import` instead of `require`:

```javascript
//  Correct (ES modules)
import OpenAI from 'openai';

// L Incorrect (CommonJS)
const OpenAI = require('openai');
```

## Troubleshooting

### Cannot find package 'openai'

Install the required dependencies:
```bash
npm install
```

### SyntaxError: Cannot use import statement

Ensure you're using Node.js 18+ and that `package.json` has `"type": "module"`.

### Authentication Error

Ensure you've replaced `"YOUR_API_KEY"` with your actual Hibana API key from the dashboard.

### Rate Limit Errors

See `10_error_handling.js` for examples of handling rate limits with exponential backoff.

## Documentation

- [Back to main README](../README.md)
- [Python samples](../python/README.md)
- Hibana Dashboard: https://hibanacloud.ir
