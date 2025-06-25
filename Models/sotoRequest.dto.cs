using System.Collections.Generic;

namespace SotoGeneratorAPI.Models
{
    public class SotoRequest
    {
        public required string Reference { get; set; } // optional or auto-generated
        public required string Customer { get; set; }
        public required string CustomerCompanyNumber { get; set; }
        public required string CustomerRepresentative { get; set; }
        public required string CustomerEmail { get; set; }

        public required string SupplierRepresentative { get; set; }
        public required string SupplierEmail { get; set; }

        public required string Problem { get; set; }
    }
}