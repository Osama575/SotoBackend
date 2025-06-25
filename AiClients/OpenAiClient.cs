// using System;
// using System.Net.Http;
// using System.Net.Http.Headers;
// using System.Text;
// using System.Text.Json;
// using System.Threading.Tasks;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
// using SotoGeneratorAPI.Models;

// namespace SotoGeneratorAPI.Clients
// {
//     public class OpenAiClient : IOpenAiClient
//     {
//         private readonly HttpClient _httpClient;
//         private readonly OpenAiOptions _options;
//         private readonly ILogger<OpenAiClient> _logger;

//         public OpenAiClient(HttpClient httpClient, IOptions<OpenAiOptions> options, ILogger<OpenAiClient> logger)
//         {
//             _httpClient = httpClient;
//             _options = options.Value;
//             _logger = logger;
//             _httpClient.BaseAddress = new Uri(_options.BaseUrl);
//             _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
//         }

//         public async Task<string> GetCompletionAsync(string prompt)
//         {
//             var payload = new
//             {
//                 model = _options.Model,
//                 messages = new[]
//                 {
//                     new { role = "system", content = "You are a helpful assistant." },
//                     new { role = "user", content = prompt }
//                 }
//             };
//             try
//             {
//                 var response = await _httpClient.PostAsync(string.Empty,
//                     new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
//                 response.EnsureSuccessStatusCode();

//                 var json = await response.Content.ReadAsStringAsync();
//                 var completion = JsonSerializer.Deserialize<ChatCompletionResponse>(json);
//                 return completion?.Choices?[0]?.Message?.Content ?? string.Empty;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "OpenAI request failed");
//                 throw;
//             }
//         }
//     }
// }
