# blackbeard-ai
A tiny chatbot to experiment with conversing with OpenAI

## Instructions
Build the UI with webpack by running the following:

```
npm run build
```

Then run the ASP.NET Core application (which serves the UI via HTTP on port 5128).  You will also need to set the `OPENAI_API_KEY` environment variable to an appropriate OpenAI API key so that the chatbot can connect to the OpenAI API.

```
OPENAI_API_KEY=<some key> dotnet run
```

Then send a message to the chatbot via the UI at http://localhost:5128.