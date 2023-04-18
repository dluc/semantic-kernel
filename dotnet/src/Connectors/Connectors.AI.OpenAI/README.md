The library contains a set of classes to integrate Semantic Kernel with OpenAI and Azure OpenAI.

For most part the code relies on [Azure OpenAI SDK](AzureSdk), which supports both AI services,
however, some services like DALL-E2 are integrated differently using a [custom client](CustomClient).

The library includes also a set of tokenizers, ported from Python [tiktoken](https://github.com/openai/tiktoken),
supporting tokenization for GPT2, GPT3 and GPT4 models.

The code is organized by features, hiding implementation details like whether Azure SDK or a custom
client is used under the cover.

* [ChatCompletion](ChatCompletion) folder: implementations of SK **IChatCompletion**. The clients
  implement also **ITextCompletion**, so you can use the chat models also for semantic functions
  and prompt engineering.
* [ImageGeneration](ImageGeneration) folder: **IImageGeneration** implementation using OpenAI DALL-E2.
* [TextCompletion](TextCompletion) folder: implementations of SK **ITextCompletion**.
* [TextEmbedding](TextEmbedding) folder:  implementations of SK **IEmbeddingGeneration<string, float>**.
* [Tokenizers](Tokenizers) folder: GPT tokenizers, compatible with OpenAI tiktoken.
