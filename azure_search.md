# C# SK Semantic Memory w/ Azure Search

```csharp
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Memory.AzureSearch;
using Microsoft.SemanticKernel.Memory;

var kernel = Kernel.Builder
    .WithAzureTextEmbeddingGenerationService(
        "text-embedding-ada-002",
        AZURE_OPENAI_ENDPOINT,
        AZURE_OPENAI_API_KEY)
    .WithMemoryStorage(new AzureSearchMemoryStore(
        AZURE_SEARCH_ENDPOINT,
        AZURE_SEARCH_ADMIN_KEY))
    .Build();

var gitHubFiles = new Dictionary<string, string>
{
    ["https://github.com/microsoft/semantic-kernel/blob/main/README.md"]
        = "README: Installation, getting started, and how to contribute",
    ["https://github.com/microsoft/semantic-kernel/blob/main/samples/notebooks/dotnet/02-running-prompts-from-file.ipynb"]
        = "Jupyter notebook describing how to pass prompts from a file to a semantic skill or function",
    ["https://github.com/microsoft/semantic-kernel/blob/main/samples/notebooks/dotnet/00-getting-started.ipynb"]
        = "Jupyter notebook describing how to get started with the Semantic Kernel",
    ["https://github.com/microsoft/semantic-kernel/tree/main/samples/skills/ChatSkill/ChatGPT"]
        = "Sample demonstrating how to create a chat skill interfacing with ChatGPT",
    ["https://github.com/microsoft/semantic-kernel/blob/main/dotnet/src/SemanticKernel/Memory/VolatileMemoryStore.cs"]
        = "C# class that defines a volatile embedding store",
    ["https://github.com/microsoft/semantic-kernel/blob/main/samples/dotnet/KernelHttpServer/README.md"]
        = "README: How to set up a Semantic Kernel Service API using Azure Function Runtime v4",
    ["https://github.com/microsoft/semantic-kernel/blob/main/samples/apps/chat-summary-webapp-react/README.md"]
        = "README: README associated with a sample chat summary react-based webapp",
};

foreach (var entry in gitHubFiles)
{
    await kernel.Memory.SaveReferenceAsync(
        collection: "GitHubFiles",
        externalSourceName: "GitHub",
        externalId: entry.Key,
        description: entry.Value,
        text: entry.Value);
}

IAsyncEnumerable<MemoryQueryResult> memories = kernel.Memory.SearchAsync("GitHubFiles", "How do I get started?", limit: 1);
var memory = await memories.FirstOrDefaultAsync(_ => true);
Console.WriteLine("URL:     : " + memory.Metadata.Id);
Console.WriteLine("Title    : " + memory.Metadata.Description);
Console.WriteLine("Relevance: " + memory.Relevance);

memories = kernel.Memory.SearchAsync("GitHubFiles", "Can I build a chat with SK?", limit: 1);
memory = await memories.FirstOrDefaultAsync(_ => true);
Console.WriteLine("URL:     : " + memory.Metadata.Id);
Console.WriteLine("Title    : " + memory.Metadata.Description);
Console.WriteLine("Relevance: " + memory.Relevance);
```

# Python SK Semantic Memory w/ Azure Search

```python
from semantic_kernel import Kernel
from semantic_kernel.connectors.ai.open_ai import AzureTextCompletion, AzureTextEmbedding
from semantic_kernel.connectors.memory.azure_search import AzureSearchMemoryStore

kernel = Kernel()

kernel.add_text_completion_service("dv", 
    AzureTextCompletion(AZURE_OPENAI_DEPLOYMENT_NAME, AZURE_OPENAI_ENDPOINT, AZURE_OPENAI_API_KEY))
kernel.add_text_embedding_generation_service("ada", 
    AzureTextEmbedding("text-embedding-ada-002", AZURE_OPENAI_ENDPOINT, AZURE_OPENAI_API_KEY))

kernel.register_memory_store(memory_store=AzureSearchMemoryStore(1536))

await kernel.memory.save_information_async("aboutMe", id="info1", text="My name is Andrea")
await kernel.memory.save_information_async("aboutMe", id="info2", text="I currently work as a tour guide")
await kernel.memory.save_information_async("aboutMe", id="info3", text="I've been living in Seattle since 2005")
await kernel.memory.save_information_async("aboutMe", id="info4", text="I visited France and Italy five times since 2015")
await kernel.memory.save_information_async("aboutMe", id="info5", text="My family is from New York")

questions = [
    "what's my name",
    "where do I live?",
    "where's my family from?",
    "where have I traveled?",
    "what do I do for work",
]

for question in questions:
    print(f"Question: {question}")
    result = await kernel.memory.search_async("aboutMe", question)
    print(f"Answer: {result[0].text}\n")
```