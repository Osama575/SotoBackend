// using SotoGeneratorAPI.Data.Repositories;
using SotoGeneratorAPI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SotoGeneratorAPI.Services
{
    public class SotoGeneratorService
    {
        private static int _refCounter = 75; // TODO: Replace with a more dynamic ID generation strategy
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        // private readonly ISotoRepository _sotoRepo;


        public SotoGeneratorService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _config = config;
            // _sotoRepo = sotoRepo;
        }

        public async Task<SotoResponse> GenerateSotoAsync(SotoRequest request)
        {
            var reference = request.Reference ?? $"S{_refCounter++.ToString("D5")}";

            var prompt = @$"
You are generating a Statement of Target Outcomes (SOTO).
Based on the following problem: '{request.Problem}', provide:
1. A detailed list of 3–5 customer responsibilities
2. 2 outcome objects, each with:
   - OutcomeName
   - OutcomeMeasure

Return this in JSON format:
{{
  ""responsibilities"": [""..."", ""...""],
  ""outcomes"": [
    {{ ""outcomeName"": ""..."", ""outcomeMeasure"": ""..."" }},
    ...
  ]
}}";

            var openAiKey = _config["OpenAI:ApiKey"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAiKey);

            try
            {

                var body = new
                {
                    model = "gpt-4",
                    messages = new[]
                    {
                    new { role = "system", content = "You are a helpful assistant." },
                    new { role = "user", content = prompt }
                }
                };

                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions",
                    new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"));

                var json = await response.Content.ReadAsStringAsync();
                var content = JsonDocument.Parse(json).RootElement   // use try catch to handle errors
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                var parsed = JsonDocument.Parse(content);

                var responsibilities = parsed.RootElement.GetProperty("responsibilities")
                    .EnumerateArray()
                    .Select(x => x.GetString()).ToList();

                var outcomes = parsed.RootElement.GetProperty("outcomes")
                    .EnumerateArray()
                    .Select(o => new TargetOutcomeResponse
                    {
                        OutcomeName = o.GetProperty("outcomeName").GetString(),
                        OutcomeMeasure = o.GetProperty("outcomeMeasure").GetString()
                    }).ToList();

                var soto = new SotoResponse
                {
                    Reference = reference,
                    Customer = request.Customer,
                    CustomerCompanyNumber = request.CustomerCompanyNumber,
                    CustomerRepresentative = request.CustomerRepresentative,
                    CustomerEmail = request.CustomerEmail,
                    SupplierRepresentative = request.SupplierRepresentative,
                    SupplierEmail = request.SupplierEmail,
                    Problem = request.Problem,
                    CustomerResponsibilities = responsibilities,
                    TargetOutcomes = outcomes
                };

                // await _sotoRepo.SaveAsync(soto);

                return soto;
            }
            catch (HttpRequestException ex)
            {
                // Log the error or handle it as needed
                throw new Exception("Error communicating with OpenAI: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                // Handle JSON parsing errors
                throw new Exception("Error parsing JSON response: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                throw new Exception("An unexpected error occurred", ex);
            }
        }
    }
}
