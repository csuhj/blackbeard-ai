using blackbeard.Hubs;
using blackbeard.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddSingleton<ConversationHubMediator>();
builder.Services.AddSingleton<ConversationService>();

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("", context =>
{
    context.Response.Redirect("./index.html", permanent: false);
    return Task.CompletedTask;
});

app.MapHub<ConversationHub>("/hub");

string? openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
if (string.IsNullOrEmpty(openAiApiKey))
{
  Console.WriteLine($"Please specify an OpenAI API key in an environment variable called OPENAI_API_KEY");
  return;
}

app.Services.GetRequiredService<ConversationService>().Initialise(openAiApiKey, "gpt-3.5-turbo");
app.Run();
