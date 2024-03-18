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

    public async Task<string> SendPrompt(string prompt)
    {
      messagesSoFar.Add(new ChatRequestUserMessage(prompt));

      Response<ChatCompletions> response = await client.GetChatCompletionsAsync(new ChatCompletionsOptions(modelName, messagesSoFar));
      ChatResponseMessage responseMessage = response.Value.Choices[0].Message;
      messagesSoFar.Add(new ChatRequestAssistantMessage(responseMessage.Content));

      return responseMessage.Content;
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
