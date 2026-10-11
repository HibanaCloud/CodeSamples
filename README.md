# Hibana API · Code Samples · نمونه‌کدهای API هیبانا

**هیبانا — درگاه API هوش مصنوعی سازگار با OpenAI با پرداخت ریالی**
**Hibana — an OpenAI-compatible AI API gateway for Iran, billed in rials.**

[hibanacloud.ir](https://hibanacloud.ir) · [Docs / مستندات](https://hibanacloud.ir/docs) · [Models / مدل‌ها](https://hibanacloud.ir/docs/models) · [Get an API key / دریافت کلید](https://hibanacloud.ir/docs/get-api-key) · [Blog / بلاگ](https://hibanacloud.ir/blog)

---

## فارسی

با یک کلید API به مدل‌های OpenAI (GPT)، آنتروپیک (Claude)، گوگل (Gemini)، دیپ‌سیک و مدل‌های دیگر دسترسی دارید و هزینه از کیف پول ریالی کم می‌شود. هیبانا با کتابخانه‌های رسمی OpenAI سازگار است: کد فعلی‌تان را نگه دارید و فقط آدرس پایه (`base_url`) و کلید را عوض کنید. مدل‌هایی مثل `gpt-oss` و `bge-m3` روی سرورهای خود هیبانا اجرا می‌شوند و هنگام قطعی اینترنت بین‌الملل هم در دسترس‌اند.

**شروع سریع**

۱. از [داشبورد هیبانا](https://hibanacloud.ir/docs/get-api-key) یک کلید API بسازید.
۲. کلید را در متغیر محیطی `HIBANA_API_KEY` بگذارید. همهٔ نمونه‌ها کلید را از همین‌جا می‌خوانند.
۳. یکی از نمونه‌ها را اجرا کنید (دستورها در بخش زبان‌ها در ادامه).

```bash
export HIBANA_API_KEY="کلید شما"        # Windows PowerShell: $env:HIBANA_API_KEY="کلید شما"
cd python && pip install -r requirements.txt && python 01_simple_chat.py
```

راهنمای اتصال هیبانا به Cline، Cursor، Kilo Code، VS Code، n8n و AnythingLLM در [INTEGRATIONS.md](INTEGRATIONS.md) است.

---

## English

One API key gives you OpenAI (GPT), Anthropic (Claude), Google (Gemini), DeepSeek and other models, paid from a wallet in Iranian rials. Hibana works with the official OpenAI SDKs: keep your code and change only the base URL and the key. Models such as `gpt-oss` and `bge-m3` run on Hibana's own servers in Iran and stay available during international internet cutoffs.

**Quick start**

1. Create an API key ([how](https://hibanacloud.ir/docs/get-api-key)).
2. Put it in the `HIBANA_API_KEY` environment variable. Every sample reads the key from there.
3. Run a sample:

```bash
export HIBANA_API_KEY="your-key"        # Windows PowerShell: $env:HIBANA_API_KEY="your-key"
cd python && pip install -r requirements.txt && python 01_simple_chat.py
```

Settings for Cline, Cursor, Kilo Code, VS Code, n8n and AnythingLLM: [INTEGRATIONS.md](INTEGRATIONS.md).

### The only change to existing OpenAI code

```python
import os
from openai import OpenAI

client = OpenAI(
    api_key=os.environ["HIBANA_API_KEY"],
    base_url="https://api-ai.hibanacloud.ir/v1",
)

response = client.chat.completions.create(
    model="claude-haiku-4-5",          # or gpt-5-nano, gemini-3.7-flash, deepseek-v4-flash, ...
    messages=[{"role": "user", "content": "Hello!"}],
)
print(response.choices[0].message.content)
```

```javascript
import OpenAI from 'openai';

const client = new OpenAI({
    apiKey: process.env.HIBANA_API_KEY,
    baseURL: "https://api-ai.hibanacloud.ir/v1",
});

const response = await client.chat.completions.create({
    model: "claude-haiku-4-5",
    messages: [{ role: "user", content: "Hello!" }],
});
console.log(response.choices[0].message.content);
```

```csharp
using System.ClientModel;
using OpenAI;
using OpenAI.Chat;

var client = new OpenAIClient(
    new ApiKeyCredential(Environment.GetEnvironmentVariable("HIBANA_API_KEY")!),
    new OpenAIClientOptions { Endpoint = new Uri("https://api-ai.hibanacloud.ir/v1") });

ChatClient chat = client.GetChatClient("claude-haiku-4-5");
ChatCompletion completion = await chat.CompleteChatAsync(new UserChatMessage("Hello!"));
Console.WriteLine(completion.Content[0].Text);
```

---

## Samples · نمونه‌ها

| # | Sample | Python | JavaScript | C# | Java |
| --- | --- | :-: | :-: | :-: | :-: |
| 01 | Simple chat | ✓ | ✓ | ✓ | ✓ |
| 02 | System prompt | ✓ | ✓ | ✓ | ✓ |
| 03 | Multi-turn conversation | ✓ | ✓ | ✓ | ✓ |
| 04 | Streaming | ✓ | ✓ | ✓ | ✓ |
| 05 | JSON mode | ✓ | ✓ | ✓ | ✓ |
| 06 | Responses API | ✓ | ✓ | ✓ | ✓ |
| 07 | Image generation (`gemini-3.1-flash-lite-image`) | ✓ | ✓ | ✓ | ✓ |
| 08 | List models | ✓ | ✓ | ✓ | ✓ |
| 09 | Wallet balance | ✓ | ✓ | ✓ | ✓ |
| 10 | Error handling | ✓ | ✓ | ✓ | ✓ |
| 11 | Several providers side by side | ✓ | ✓ | ✓ | ✓ |
| 12 | Semantic search in Persian with embeddings (`bge-m3`) | ✓ | ✓ | | |
| 13 | Fallback to a domestic model during an internet cutoff (`gpt-oss`) | ✓ | ✓ | | |

### Python
```bash
cd python
pip install -r requirements.txt
python 01_simple_chat.py
```
[python/README.md](python/README.md)

### JavaScript / Node.js (18+)
```bash
cd javascript
npm install
node 01_simple_chat.js
```
[javascript/README.md](javascript/README.md)

### C# / .NET 9
```bash
cd csharp
dotnet run -p:StartupObject=Hibana.Samples.Example01_SimpleChat
```
[csharp/README.md](csharp/README.md)

### Java (17+)
See [java/README.md](java/README.md).

---

## Notes

- **Models**: use exact model IDs from [hibanacloud.ir/docs/models](https://hibanacloud.ir/docs/models) or sample `08`. The list, prices and context sizes are live there.
- **Images**: `size` is an aspect ratio (`"1:1"`, `"16:9"`, …) and the returned URL expires after about an hour. Download it (sample `07`).
- **Errors**: `401` bad key · `402` wallet balance too low · `429` rate limit. See sample `10` and [hibanacloud.ir/docs/errors](https://hibanacloud.ir/docs/errors).
- **Support**: support@hibanacloud.ir

## License

See [LICENSE](LICENSE).
