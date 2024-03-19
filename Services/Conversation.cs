using Azure;
using Azure.AI.OpenAI;

namespace blackbeard.Services
{
  public class Conversation
  {
    private OpenAIClient client;
    private List<ChatRequestMessage> messagesSoFar;
    private string modelName;

    public Conversation(OpenAIClient client, string modelName)
    {
      this.client = client;
      this.modelName = modelName;
      messagesSoFar = CreateInitialMessages();
    }

    public async Task SendPrompt(string prompt, Func<string, bool, Task> processResponsePart)
    {
      messagesSoFar.Add(new ChatRequestUserMessage(prompt));

      StreamingResponse<StreamingChatCompletionsUpdate> response = await client.GetChatCompletionsStreamingAsync(new ChatCompletionsOptions(modelName, messagesSoFar));
      string message = "";
      bool firstMessagePart = true;
      await foreach (var x in response)
      {
        if (x.ContentUpdate == null)
          break;

        await processResponsePart(x.ContentUpdate, firstMessagePart);
        message += x.ContentUpdate;
        firstMessagePart = false;
      }
      messagesSoFar.Add(new ChatRequestAssistantMessage(message));
    }

    private List<ChatRequestMessage> CreateInitialMessages()
    {
      return new List<ChatRequestMessage>()
      {
        new ChatRequestSystemMessage("You are a helpful assistant. You will talk like a pirate and your name is Blackbeard.")
      };
    }
  }
}
