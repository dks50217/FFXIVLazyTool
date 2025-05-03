using Azure;
using Azure.AI.Inference;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Service
{
    public interface IAzureInferenceChatService
    {
        Task<string> GetChatCompletionAsync(IEnumerable<ChatRequestMessage> messages);
    }

    public class AzureInferenceChatService : IAzureInferenceChatService
    {
        private readonly ChatCompletionsClient _client;
        private readonly string _model;
        private readonly ILogger<AzureInferenceChatService> _logger;
        private readonly IConfiguration _configuration;

        public AzureInferenceChatService(IConfiguration configuration, ILogger<AzureInferenceChatService> logger)
        {
            _configuration = configuration;

            var endpoint = new Uri(_configuration["AzureInference:Endpoint"]);
            var apiKey = _configuration["AzureInference:ApiKey"];
            _model = _configuration["AzureInference:Model"] ?? "openai/gpt-4.1";

            _client = new ChatCompletionsClient(
                endpoint,
                new AzureKeyCredential(apiKey),
                new AzureAIInferenceClientOptions());

            _logger = logger;
        }

        public async Task<string> GetChatCompletionAsync(IEnumerable<ChatRequestMessage> messages)
        {
            try
            {
                var requestOptions = new ChatCompletionsOptions
                {
                    Temperature = 1f,
                    NucleusSamplingFactor = 1f,
                    Model = _model
                };

                foreach (var message in messages)
                {
                    requestOptions.Messages.Add(message);
                }

                Response<ChatCompletions> response = await _client.CompleteAsync(requestOptions);
                return response.Value.Content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get chat completion");
                return "An error occurred while getting the chat completion.";
            }
        }
    }
}
