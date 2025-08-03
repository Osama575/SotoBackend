using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SotoGeneratorAPI.Models
{
    public class SotoRequest
    {
        public required string Reference { get; set; }
        public required string Customer { get; set; }
        public required string CustomerCompanyNumber { get; set; }
        public required string CustomerRepresentative { get; set; }
        public required string CustomerEmail { get; set; }

        public required string SupplierRepresentative { get; set; }
        public required string SupplierEmail { get; set; }

        public required string Problem { get; set; }
        [Range(28, 30, ErrorMessage = "SupportTerm must be either 28 or 30 days")]
        public int SupportTerm { get; set; } = 30; 
    }
}