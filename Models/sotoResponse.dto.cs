// Models/SotoResponse.cs
namespace SotoGeneratorAPI.Models
{
    public class TargetOutcomeResponse
    {
        public string OutcomeName { get; set; }
        public string OutcomeMeasure { get; set; }
    }

    public class SotoResponse
    {
        public required string Reference { get; set; }
        public required string Customer { get; set; }
        public required string CustomerCompanyNumber { get; set; }
        public required string CustomerRepresentative { get; set; }
        public required string CustomerEmail { get; set; }
        public string Supplier { get; set; } = "IJYI Ltd";
        public string SupplierCompanyNumber { get; set; } = "08844194";
        public required string SupplierRepresentative { get; set; }
        public required string SupplierEmail { get; set; }
        public string GovernedBy { get; set; } = "Terms202305.28.1 (attached)";
        public required string Problem { get; set; }
        public required List<string> CustomerResponsibilities { get; set; }
        public required List<TargetOutcomeResponse> TargetOutcomes { get; set; }
    }
}