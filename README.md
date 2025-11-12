# Hibana API - Code Samples

Code samples demonstrating how to use the Hibana AI API in multiple programming languages. Hibana provides a unified OpenAI-compatible API gateway for multiple LLM providers including OpenAI, Anthropic, DeepSeek, and Google Gemini.

## Features

- **OpenAI-Compatible**: Use official OpenAI SDKs
- **Multiple Providers**: Access GPT, Claude, DeepSeek, and Gemini models through one API
- **Streaming Support**: Real-time streaming responses
- **Image Generation**: DALL-E integration
- **Cost-Effective**: Pay-as-you-go pricing in Iranian Rials

## Available Languages

### 🐍 [Python Samples](python/)

Complete set of Python examples using the OpenAI Python SDK.

```bash
cd python
pip install -r requirements.txt
python 01_simple_chat.py
```

**[View Python README →](python/README.md)**

### 📦 [JavaScript/Node.js Samples](javascript/)

Complete set of JavaScript examples using the OpenAI Node.js SDK.

```bash
cd javascript
npm install
node 01_simple_chat.js
```

**[View JavaScript README →](javascript/README.md)**

### ☕ [Java Samples](java/)

Complete set of Java examples using the OpenAI Java SDK.

```bash
cd java
./gradlew build
./gradlew run --args="Example01_SimpleChat"
```

**[View Java README →](java/README.md)**

## Quick Start

### API Configuration

All samples use the following configuration:

- **Base URL**: `https://api-ai.hibanacloud.com/v1`
- **Authentication**: Bearer token (API Key)
- **API Key**: Replace `YOUR_API_KEY` with your actual key from the Hibana dashboard

### Python Example

```python
from openai import OpenAI

client = OpenAI(
    api_key="YOUR_API_KEY",
    base_url="https://api-ai.hibanacloud.com/v1"
)

response = client.chat.completions.create(
    model="gpt-5-nano",
    messages=[{"role": "user", "content": "Hello!"}]
)

print(response.choices[0].message.content)
```

### JavaScript Example

```javascript
import OpenAI from 'openai';

const client = new OpenAI({
    apiKey: "YOUR_API_KEY",
    baseURL: "https://api-ai.hibanacloud.com/v1"
});

const response = await client.chat.completions.create({
    model: "gpt-5-nano",
    messages: [{ role: "user", content: "Hello!" }]
});

console.log(response.choices[0].message.content);
```

### Java Example

```java
import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.models.*;

OpenAIClient client = OpenAIOkHttpClient.builder()
    .apiKey("YOUR_API_KEY")
    .baseUrl("https://api-ai.hibanacloud.com/v1")
    .build();

ChatCompletionCreateParams params = ChatCompletionCreateParams.builder()
    .model("gpt-5-nano")
    .addMessage(ChatCompletionMessage.ofChatCompletionUserMessageParam(
        ChatCompletionMessage.ChatCompletionUserMessageParam.builder()
            .content("Hello!")
            .build()
    ))
    .build();

ChatCompletion response = client.chat().completions().create(params);
System.out.println(response.choices().get(0).message().content().orElse(""));
```

## Supported Models

| Provider | Model ID | Description |
|----------|----------|-------------|
| **OpenAI** | `gpt-5-nano` | Fast and efficient GPT model |
| **Anthropic** | `claude-haiku-4-5` | Claude Haiku - Fast responses |
| **DeepSeek** | `deepseek-chat` | DeepSeek conversational model |
| **Google** | `gemini-2.5-flash-lite` | Gemini Flash - Lightweight |
| **OpenAI** | `dall-e-3` | Image generation |

## Sample Categories

All samples are available in Python, JavaScript, and Java:

### 📚 Basic Examples
1. **Simple Chat** - Basic chat completion
2. **System Prompts** - Control AI behavior with system messages
3. **Multi-turn Conversations** - Maintain context across messages

### ⚡ Advanced Features
4. **Streaming** - Real-time streaming responses
5. **JSON Mode** - Structured JSON output
6. **Responses API** - Simplified API format

### 🎨 Additional Capabilities
7. **Image Generation** - Create images with DALL-E
8. **List Models** - Discover available models
9. **Check Balance** - Monitor wallet balance

### ✅ Best Practices
10. **Error Handling** - Proper error handling and retry logic
11. **Multiple Providers** - Compare all 4 providers

## OpenAI SDK Compatibility

Hibana is 100% compatible with OpenAI SDKs. You can use any OpenAI SDK features by simply changing:
- **API Key**: Your Hibana API key
- **Base URL**: `https://api-ai.hibanacloud.com/v1`

This means existing OpenAI code can be migrated to Hibana with minimal changes!

## Getting Your API Key

1. Sign up at [Hibana Cloud Dashboard](https://hibanacloud.com/dashboard)
2. Navigate to API Keys section
3. Create a new API key
4. Replace `YOUR_API_KEY` in the samples with your actual key

## Common Issues

### Authentication Error
- Ensure your API key is correct
- Check that the key is active in your dashboard
- Verify the Bearer token format

### Model Not Found
- Check available models using sample `08_list_models`
- Ensure the model is enabled in your account
- Use exact model names from the supported models table

### Rate Limiting
- Hibana implements rate limiting per model
- See sample `10_error_handling` for retry strategies
- Consider implementing exponential backoff

## Documentation

- **API Documentation**: [https://hibanacloud.com/docs](https://hibanacloud.com/docs)
- **Dashboard**: [https://hibanacloud.com/dashboard](https://hibanacloud.com/dashboard)
- **Support Email**: support@hibanacloud.com

## Contributing

Found an issue or want to add more examples? Feel free to:
- Open an issue
- Submit a pull request
- Contact us with suggestions

## License

These code samples are provided as-is for educational and integration purposes.

---

**Happy coding with Hibana AI!** 🚀
