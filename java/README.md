# Hibana API Java Samples

This folder contains Java code samples demonstrating how to use the Hibana AI API with the official OpenAI Java SDK.

## Requirements

- Java 17 or later
- Gradle 7.6 or later (no Gradle wrapper is included; use your own `gradle`)
- OpenAI Java SDK `com.openai:openai-java:4.7.1` (downloaded by Gradle)

## Installation

1. **Clone or download this repository**

2. **Build the project**:
   ```bash
   cd java
   gradle build
   ```

3. **Set your API key**: every example reads it from the `HIBANA_API_KEY` environment variable:
   ```bash
   export HIBANA_API_KEY="your-key"          # Windows PowerShell: $env:HIBANA_API_KEY="your-key"
   ```

## Running the Examples

Run any example with Gradle by passing its class name as `-PmainClass` (without it, `Example01_SimpleChat` runs):

```bash
gradle run -PmainClass=com.hibana.samples.Example01_SimpleChat
gradle run -PmainClass=com.hibana.samples.Example04_StreamingResponse
```

Or copy the classes and all dependency jars to `build/install` and run with `java` directly:

```bash
gradle installDist
java -cp "build/install/hibana-api-java-samples/lib/*" com.hibana.samples.Example01_SimpleChat
```

On Windows:
```bash
gradle installDist
java -cp "build\install\hibana-api-java-samples\lib\*" com.hibana.samples.Example01_SimpleChat
```

`Example07_ImageGeneration` saves the image in the working directory (the `java/` folder when started with `gradle run`).

## Available Examples

### 1. **Example01_SimpleChat.java** - Basic Chat Completion
- Simple chat request and response
- Display token usage and finish reason
- Model: `gpt-5-nano`

### 2. **Example02_ChatWithSystemPrompt.java** - System Prompts
- Control AI behavior with system messages
- Role-based prompts (pirate, expert, etc.)
- Formatted output examples
- Model: `claude-haiku-4-5`

### 3. **Example03_MultiTurnConversation.java** - Multi-Turn Conversations
- Maintain context across multiple messages
- Build conversation history
- Example: Teaching scenario with context
- Model: `deepseek-v4-flash`

### 4. **Example04_StreamingResponse.java** - Streaming Responses
- Real-time token streaming (SSE)
- Streaming vs non-streaming comparison
- Chunk metadata inspection
- Models: `gpt-5-nano`, `gemini-3.7-flash`

### 5. **Example05_JsonMode.java** - JSON Mode
- Request structured JSON responses
- Data extraction from text
- JSON arrays and objects
- Model: `gpt-5-nano`

### 6. **Example06_ResponsesApiSimple.java** - Response API
- Basic and streaming responses
- Access response metadata
- Inspect finish reasons
- Model: `gemini-3.7-flash`

### 7. **Example07_ImageGeneration.java** - Image Generation
- Generate an image from a text prompt
- `size` is an aspect ratio such as `"16:9"`
- The temporary image URL is downloaded and saved to a file
- Model: `gemini-3.1-flash-lite-image`

### 8. **Example08_ListModels.java** - List Available Models
- Retrieve all available models
- Categorize by provider (OpenAI, Anthropic, DeepSeek, Google)
- Display model details

### 9. **Example09_CheckBalance.java** - Check Balance
- Check wallet balance
- Estimate remaining requests
- Usage recommendations
- Custom endpoint (not OpenAI SDK)

### 10. **Example10_ErrorHandling.java** - Error Handling
- Handle authentication errors
- Retry with exponential backoff
- Input validation
- Comprehensive error handling patterns

### 11. **Example11_MultipleProviders.java** - Multiple Providers
- Compare responses from all providers
- Performance benchmarking
- Provider-specific features
- Model selection guide

## Configuration

All examples use the following configuration:

```java
private static final String API_KEY = System.getenv().getOrDefault("HIBANA_API_KEY", "YOUR_API_KEY");
private static final String BASE_URL = "https://api-ai.hibanacloud.ir/v1";
```

### Supported Models

| Provider | Model ID | Description |
|----------|----------|-------------|
| OpenAI | `gpt-5-nano` | Fast and cost-effective GPT model |
| Anthropic | `claude-haiku-4-5` | Fast Claude model with large context |
| DeepSeek | `deepseek-v4-flash` | Cost-effective Chinese LLM |
| Google | `gemini-3.7-flash` | Fast and efficient Gemini model |
| Google | `gemini-3.1-flash-lite-image` | Image generation model |

## Project Structure

```
java/
├── src/
│   └── main/
│       └── java/
│           └── com/
│               └── hibana/
│                   └── samples/
│                       ├── Example01_SimpleChat.java
│                       ├── Example02_ChatWithSystemPrompt.java
│                       ├── Example03_MultiTurnConversation.java
│                       ├── Example04_StreamingResponse.java
│                       ├── Example05_JsonMode.java
│                       ├── Example06_ResponsesApiSimple.java
│                       ├── Example07_ImageGeneration.java
│                       ├── Example08_ListModels.java
│                       ├── Example09_CheckBalance.java
│                       ├── Example10_ErrorHandling.java
│                       └── Example11_MultipleProviders.java
├── build.gradle.kts
├── settings.gradle.kts
└── README.md
```

## Dependencies

This project uses:
- **OpenAI Java SDK** (4.7.1) - Official OpenAI Java library
- **OkHttp** (4.12.0) - HTTP client for custom endpoints
- **Gson** (2.10.1) - JSON parsing

All dependencies are managed by Gradle and will be downloaded automatically.

## Common Parameters

Most examples use these standard parameters:

- **temperature**: Controls randomness
  - `0.3` - More deterministic (good for coding)
  - `0.7` - Balanced
  - `1.0` - More creative

- **maxTokens**: Maximum length of response
  - `8000` - Standard for most providers
  - `8192` - DeepSeek conversations
  - `10000` - OpenAI models

## Troubleshooting

### "Invalid API key" error
- Make sure the `HIBANA_API_KEY` environment variable holds your actual key
- Verify the key is active in your Hibana dashboard

### "Model not found" error
- Check the model name for typos
- Run `Example08_ListModels` to see all available models
- Some models may require special access

### Build errors
- Ensure you have Java 17 or later: `java -version`
- Ensure you have Gradle 7.6 or later: `gradle --version`
- Clean and rebuild: `gradle clean build`

### Streaming errors
- All streaming examples include safety checks for empty chunks
- If you still encounter errors, check your network connection

## API Documentation

For complete API documentation, visit:
- Hibana API Docs: https://hibanacloud.ir/docs
- OpenAI API Reference: https://platform.openai.com/docs/api-reference

## Support

If you encounter issues:
1. Check the error message in the console
2. Verify your API key and balance
3. Run `Example10_ErrorHandling` for comprehensive error patterns
4. Contact Hibana support for API-specific issues

## License

These samples are provided as-is for educational purposes.
