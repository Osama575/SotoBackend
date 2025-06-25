// namespace SotoGeneratorAPI.Prompt
// {
//     public static class PromptBuilder
//     {
//         public static string BuildSotoPrompt(string problem)
//         {
//             return $@"You are generating a Statement of Target Outcomes (SOTO). 
// Based on the following problem: '{problem}', provide: 
// 1. A detailed list of 3–5 customer responsibilities 
// 2. 2 outcome objects, each with: 
//    - OutcomeName 
//    - OutcomeMeasure 

// Return this in JSON format: 
// {{
//   \"responsibilities\": [\"...\", \"...\"],
//   \"outcomes\": [
//     {{ \"outcomeName\": \"...\", \"outcomeMeasure\": \"...\" }},
//     ...
//   ]
// }}";
//         }
//     }
// }