using System.ComponentModel.DataAnnotations;

namespace SotoGeneratorAPI.Data.Entities
{
    public class SotoEntity
    {
        [Key]
        public required string Reference { get; set; }
        public required string Customer { get; set; }
        public required string CustomerCompanyNumber { get; set; }
        public required string CustomerRepresentative { get; set; }
        public required string CustomerEmail { get; set; }
        public required string SupplierRepresentative { get; set; }
        public required string SupplierEmail { get; set; }
        public required string GovernedBy { get; set; }
        public required string Problem { get; set; }

        public required string CustomerResponsibilitiesJson { get; set; }
        public required string TargetOutcomesJson { get; set; }
    }
}
