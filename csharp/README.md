# Hibana API - C# Samples

C# code samples demonstrating how to use the Hibana AI API with the OpenAI .NET SDK.

## Prerequisites

- .NET 9.0 SDK or higher
- NuGet package manager

## Installation

1. Restore NuGet packages:

```bash
dotnet restore
```

This will install:
- `OpenAI` version 2.1.0 - Official OpenAI .NET SDK

Alternatively, you can install the package manually:

```bash
dotnet add package OpenAI --version 2.1.0
```

## Configuration

All samples use the following configuration (updated for OpenAI SDK 2.1.0):

```csharp
using System;
using System.ClientModel;
using OpenAI;
using OpenAI.Chat;

var client = new OpenAIClient(
    new ApiKeyCredential("YOUR_API_KEY"),
    new OpenAIClientOptions
    {
        Endpoint = new Uri("https://api-ai.hibanacloud.com/v1")
    }
);
```

**Important:**
- Replace `"YOUR_API_KEY"` with your actual Hibana API key before running the samples
- OpenAI SDK 2.1.0 requires `ApiKeyCredential` wrapper (not a plain string)

## Available Samples

### Basic Examples

| File | Description | Model |
|------|-------------|-------|
| `Example01_SimpleChat.cs` | Basic chat completion | gpt-5-nano |
| `Example02_ChatWithSystemPrompt.cs` | Custom system instructions | claude-haiku-4-5, gpt-5-nano |
| `Example03_MultiTurnConversation.cs` | Multi-message conversations | deepseek-chat |

### Advanced Features

| File | Description | Model |
|------|-------------|-------|
| `Example04_StreamingResponse.cs` | Real-time streaming responses | gpt-5-nano, gemini-2.5-flash-lite |
| `Example05_JsonMode.cs` | Structured JSON output | gpt-5-nano |
| `Example06_ResponsesApiSimple.cs` | Simplified Responses API | All 4 providers |

### Additional Capabilities

| File | Description | Model |
|------|-------------|-------|
| `Example07_ImageGeneration.cs` | DALL-E image generation | dall-e-3 |
| `Example08_ListModels.cs` | List available models | N/A |
| `Example09_CheckBalance.cs` | Check wallet balance | N/A |

### Best Practices

| File | Description | Model |
|------|-------------|-------|
| `Example10_ErrorHandling.cs` | Error handling & retry logic | gpt-5-nano |
| `Example11_MultipleProviders.cs` | Compare all 4 providers | gpt-5-nano, claude-haiku-4-5, deepseek-chat, gemini-2.5-flash-lite |

## Running the Samples

### Running Individual Examples

To run a specific example, edit `CodeSamples.csproj` and change the `StartupObject`:

```xml
<StartupObject>Hibana.Samples.Example01_SimpleChat</StartupObject>
```

Then run:

```bash
dotnet run
```

**Example**: To run Example05_JsonMode:
1. Open `CodeSamples.csproj`
2. Change line 10 to: `<StartupObject>Hibana.Samples.Example05_JsonMode</StartupObject>`
3. Run: `dotnet run`

### Building the Project

To build all examples:

```bash
dotnet build
```

Build output will show 0 errors and a few warnings (which are non-critical).

### Running from Visual Studio

1. Open `CodeSamples.csproj` in Visual Studio 2022 or later
2. Edit the `StartupObject` in the project file
3. Press F5 or click "Start"

## Supported Models

### Chat/Text Models (16 total)

| Provider | Model ID | Best For |
|----------|----------|----------|
| **OpenAI** | `gpt-5` | High quality responses |
| | `gpt-5-mini` | Balanced performance |
| | `gpt-5-nano` | Fast and efficient (recommended for testing) |
| | `gpt-4.1` | Advanced reasoning |
| | `gpt-4.1 mini` | Efficient GPT-4 |
| | `gpt-4o-mini` | Optimized GPT-4 |
| | `gpt-o4-mini` | Lightweight GPT-4 |
| **Anthropic** | `claude-sonnet-4-5` | Balanced Claude model |
| | `claude-opus-4-1` | Highest quality Claude |
| | `claude-haiku-4-5` | Fast Claude responses |
| **DeepSeek** | `deepseek-chat` | Cost-effective conversations |
| | `deepseek-reasoner` | Advanced reasoning |
| **Google** | `gemini-2.5-pro` | Highest quality Gemini |
| | `gemini-2.5-flash` | Fast Gemini |
| | `gemini-2.5-flash-lite` | Lightweight Gemini |

### Image Generation Models

| Provider | Model ID | Sizes Available |
|----------|----------|-----------------|
| **OpenAI** | `dall-e-3` | 1024x1024, 1024x1792, 1792x1024 |

## Common Configuration

Most samples use these default values (tested and verified):

```csharp
new ChatCompletionOptions
{
    Temperature = 0.7f,           // Balanced randomness
    MaxOutputTokenCount = 10000    // Maximum response length
}
```

Variations:
- **Coding tasks**: `Temperature = 0.3f` (more deterministic)
- **Creative writing**: `Temperature = 1.0f` (more random)
- **DeepSeek conversations**: `MaxOutputTokenCount = 8192`

## Project Structure

```
csharp/
├── CodeSamples.csproj          # Project file (.NET 9.0)
├── README.md                    # This file
├── Example01_SimpleChat.cs
├── Example02_ChatWithSystemPrompt.cs
├── Example03_MultiTurnConversation.cs
├── Example04_StreamingResponse.cs
├── Example05_JsonMode.cs
├── Example06_ResponsesApiSimple.cs
├── Example07_ImageGeneration.cs
├── Example08_ListModels.cs
├── Example09_CheckBalance.cs
├── Example10_ErrorHandling.cs
└── Example11_MultipleProviders.cs
```

## SDK Compatibility Notes

### OpenAI .NET SDK 2.1.0 Changes

These samples have been updated and tested with OpenAI .NET SDK 2.1.0. Key changes from older versions:

#### ✅ API Key Handling
**Correct (SDK 2.1.0)**:
```csharp
new OpenAIClient(new ApiKeyCredential(apiKey), options)
```

**Old (Pre-2.1.0)**:
```csharp
new OpenAIClient(apiKey, options)  // ❌ No longer works
```

#### ✅ JSON Mode
**Correct (SDK 2.1.0)**:
```csharp
ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
```

**Old (Pre-2.1.0)**:
```csharp
ResponseFormat = ChatResponseFormat.JsonObject  // ❌ No longer exists
```

#### ✅ Model Listing (Example08)
Due to API limitations in SDK 2.1.0, `Example08_ListModels.cs` uses direct HTTP calls instead of SDK methods. This is intentional and works perfectly.

### All Examples Are Fully Tested ✅

Every example in this repository has been:
- ✅ Fixed for SDK 2.1.0 compatibility
- ✅ Successfully built (0 errors)
- ✅ Tested with live API calls
- ✅ Verified to work correctly

## Troubleshooting

### Could not load file or assembly 'OpenAI'

Install the required NuGet package:
```bash
dotnet add package OpenAI --version 2.1.0
```

### Compilation Error: cannot convert from 'string' to 'System.ClientModel.ApiKeyCredential'

You're using old code syntax. Update to:
```csharp
new OpenAIClient(new ApiKeyCredential("YOUR_API_KEY"), options)
```

### Authentication Error

Ensure you've replaced `"YOUR_API_KEY"` with your actual Hibana API key from the dashboard.

### Rate Limit Errors

See `Example10_ErrorHandling.cs` for examples of handling rate limits with exponential backoff.

### .NET SDK Not Found

Download and install the .NET 9.0 SDK from: https://dotnet.microsoft.com/download

### Build Errors

Try cleaning and rebuilding:
```bash
dotnet clean
dotnet restore
dotnet build
```

### StartupObject Error

If you see "Could not find 'Hibana.Samples.TestSimple'", edit `CodeSamples.csproj` and set a valid example:
```xml
<StartupObject>Hibana.Samples.Example01_SimpleChat</StartupObject>
```

## Key Differences from Python

- **Async/Await**: All API calls are asynchronous using `async`/`await` pattern
- **Naming Convention**: PascalCase instead of snake_case (e.g., `MaxOutputTokenCount` vs `max_tokens`)
- **Strongly Typed**: Uses strongly typed options classes (`ChatCompletionOptions`, `ImageGenerationOptions`)
- **Streaming**: Uses `await foreach` to iterate over streaming updates
- **JSON Handling**: Uses `System.Text.Json` for JSON parsing and serialization
- **HTTP Client**: Uses built-in `HttpClient` for custom endpoints (Example 09)

## Documentation

- [Back to main README](../README.md)
- [Python samples](../python/README.md)
- [JavaScript samples](../javascript/README.md)
- [Java samples](../java/README.md)
- Hibana Dashboard: https://hibanacloud.com
