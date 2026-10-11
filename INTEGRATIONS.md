# Using Hibana in AI tools · اتصال هیبانا به ابزارهای هوش مصنوعی

Any tool that can talk to the OpenAI API can use Hibana. You only need three values:

| Setting | Value |
| --- | --- |
| Base URL | `https://api-ai.hibanacloud.ir/v1` |
| API key | your Hibana key ([how to get one](https://hibanacloud.ir/docs/get-api-key)) |
| Model | any model ID from the [model list](https://hibanacloud.ir/docs/models), e.g. `claude-haiku-4-5`, `gpt-5-nano`, `deepseek-v4-flash` |

هر ابزاری که با API اوپن‌ای‌آی کار می‌کند با هیبانا هم کار می‌کند؛ فقط آدرس پایه، کلید و نام مدل را عوض کنید. راهنمای کامل و تصویری هر ابزار در سایت است:

| Tool | Where the settings are | Full guide (Persian, with screenshots) |
| --- | --- | --- |
| **Cline** (VS Code) | API Provider → `OpenAI Compatible`, then Base URL, API key, Model ID | [hibanacloud.ir/docs/integrations/cline](https://hibanacloud.ir/docs/integrations/cline) |
| **Kilo Code** | API Provider → `OpenAI Compatible`, then Base URL, API key, model | [hibanacloud.ir/docs/integrations/kilo-code](https://hibanacloud.ir/docs/integrations/kilo-code) |
| **Cursor** | Settings → Models → turn on *OpenAI API Key*, turn on *Override* and enter the Base URL, add the model name | [hibanacloud.ir/docs/integrations/cursor](https://hibanacloud.ir/docs/integrations/cursor) |
| **VS Code** (`vscode-openai` extension) | Setup OpenAI Provider → custom, then Base URL and API key | [hibanacloud.ir/docs/integrations/vscode](https://hibanacloud.ir/docs/integrations/vscode) |
| **n8n** | *OpenAI Chat Model* node → Create New Credential → API key + Base URL | [hibanacloud.ir/docs/integrations/n8n](https://hibanacloud.ir/docs/integrations/n8n) |
| **AnythingLLM** | Settings → LLM → LLM Provider, then Base URL and API key | [hibanacloud.ir/docs/integrations/anything-llm](https://hibanacloud.ir/docs/integrations/anything-llm) |

Video walkthroughs of each tool are on Hibana's Aparat channel and are linked from the guides above.
