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
                    new { role = "system", content = "You are a business and marketing expert." },
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
                    SupportTerm = request.SupportTerm,
                    SupportContract = GenerateSupportContractText(request.SupportTerm),
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

        private string GenerateSupportContractText(int supportTerm)
        {
            var dayWord = supportTerm == 28 ? "Twenty-Eight (28)" : "Thirty (30)";
            var dayNum = supportTerm;
            
            return $@"Support Service Fees & Costs

All prices below are exclusive of VAT.
Cleared payment is due within {dayWord} days from the date of invoice.

The Support Service will commence when the System Builder is made available to end-users as a live or beta service. This may be at the end of the development project or during if deemed appropriate by the customer.

The Support service will be invoiced at £4,400 per calendar month. A 50% discount applies to any full calendar month which has at least one full project development sprint within it. A 36-month term applies, and the costs are fixed for the term.

Support comprises fixed and variable costs as described below.

Fixed Support Costs

Description: Support Service
Cost: £1,200
Notes: Per Month. Base Support Service, Tooling, Knowledge Base, Business Hours Support and Management.

Description: Support Hours
Cost: £800
Notes: Per Month. 24 hours of Business Hours Support per Quarter, pro rata to 8 hours per month.

Description: Security Test and Report
Cost: Not Included
Notes: Per Month (£1,000 per Quarter) Annual security test of the Service.

Description: Cost Optimisation Assessment and Report
Cost: Not Included
Notes: Per Month (£1,000 per Quarter) Annual cost optimisation assessment of the Cloud Platform.

Description: Total Fixed Cost
Cost: £2,000
Per Month";
        }
    }
}
