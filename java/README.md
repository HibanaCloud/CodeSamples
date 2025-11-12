# Hibana API Java Samples

This folder contains Java code samples demonstrating how to use the Hibana AI API with the official OpenAI Java SDK.

## Requirements

- Java 21 or later
- Gradle 8.0+ (wrapper included)

## Installation

1. **Clone or download this repository**

2. **Build the project**:
   ```bash
   cd java
   ./gradlew build
   ```
   On Windows:
   ```bash
   gradlew.bat build
   ```

3. **Update your API key**:
   - Open any example file in `src/main/java/com/hibana/samples/`
   - Replace `"YOUR_API_KEY"` with your actual Hibana API key

## Running the Examples

Run any example using Gradle:

```bash
./gradlew run --args="Example01_SimpleChat"
```

Or compile and run directly:

```bash
./gradlew build
java -cp build/libs/hibana-api-java-samples-1.0.0.jar com.hibana.samples.Example01_SimpleChat
```

On Windows:
```bash
gradlew.bat build
java -cp build\libs\hibana-api-java-samples-1.0.0.jar com.hibana.samples.Example01_SimpleChat
```

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
- Model: `deepseek-chat`

### 4. **Example04_StreamingResponse.java** - Streaming Responses
- Real-time token streaming (SSE)
- Streaming vs non-streaming comparison
- Chunk metadata inspection
- Models: `gpt-5-nano`, `gemini-2.5-flash-lite`

### 5. **Example05_JsonMode.java** - JSON Mode
- Request structured JSON responses
- Data extraction from text
- JSON arrays and objects
- Model: `gpt-5-nano`

### 6. **Example06_ResponsesApiSimple.java** - Response API
- Basic and streaming responses
- Access response metadata
- Inspect finish reasons
- Model: `gemini-2.5-flash-lite`

### 7. **Example07_ImageGeneration.java** - Image Generation
- Generate images from text prompts
- Multiple image variations
- HD quality and style options
- Model: `dall-e-3`

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
private static final String API_KEY = "YOUR_API_KEY";
private static final String BASE_URL = "https://api-ai.hibanacloud.com/v1";
```

### Supported Models

| Provider | Model ID | Description |
|----------|----------|-------------|
| OpenAI | `gpt-5-nano` | Fast and cost-effective GPT model |
| Anthropic | `claude-haiku-4-5` | Fast Claude model with large context |
| DeepSeek | `deepseek-chat` | Cost-effective Chinese LLM |
| Google | `gemini-2.5-flash-lite` | Fast and efficient Gemini model |
| OpenAI | `dall-e-3` | Image generation model |

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
- Make sure you replaced `"YOUR_API_KEY"` with your actual key
- Verify the key is active in your Hibana dashboard

### "Model not found" error
- Check the model name for typos
- Run `Example08_ListModels` to see all available models
- Some models may require special access

### Build errors
- Ensure you have Java 21 or later: `java -version`
- Update Gradle: `./gradlew wrapper --gradle-version=8.5`
- Clean and rebuild: `./gradlew clean build`

### Streaming errors
- All streaming examples include safety checks for empty chunks
- If you still encounter errors, check your network connection

## API Documentation

For complete API documentation, visit:
- Hibana API Docs: https://docs.hibanacloud.com
- OpenAI API Reference: https://platform.openai.com/docs/api-reference

## Support

If you encounter issues:
1. Check the error message in the console
2. Verify your API key and balance
3. Run `Example10_ErrorHandling` for comprehensive error patterns
4. Contact Hibana support for API-specific issues

## License

These samples are provided as-is for educational purposes.
