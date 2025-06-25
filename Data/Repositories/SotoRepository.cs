// using Microsoft.EntityFrameworkCore;
// using System.Text.Json;
// using SotoGeneratorAPI.Data.Entities;
// using SotoGeneratorAPI.Models;

// namespace SotoGeneratorAPI.Data.Repositories
// {
//     public class SotoRepository : ISotoRepository
//     {
//         private readonly AppDbContext _ctx;

//         public SotoRepository(AppDbContext ctx) => _ctx = ctx;

//         public async Task SaveAsync(SotoResponse soto)
//         {
//             var entity = new SotoEntity
//             {
//                 Reference                   = soto.Reference,
//                 Customer                    = soto.Customer,
//                 CustomerCompanyNumber       = soto.CustomerCompanyNumber,
//                 CustomerRepresentative      = soto.CustomerRepresentative,
//                 CustomerEmail               = soto.CustomerEmail,
//                 SupplierRepresentative      = soto.SupplierRepresentative,
//                 SupplierEmail               = soto.SupplierEmail,
//                 GovernedBy                  = soto.GovernedBy,
//                 Problem                     = soto.Problem,
//                 CustomerResponsibilitiesJson = JsonSerializer.Serialize(soto.CustomerResponsibilities),
//                 TargetOutcomesJson          = JsonSerializer.Serialize(soto.TargetOutcomes),

//             };

//             _ctx.Sotos.Add(entity);
//             await _ctx.SaveChangesAsync();
//         }

//         public async Task<SotoResponse?> GetAsync(string reference)
//         {
//             var e = await _ctx.Sotos.FindAsync(reference);
//             if (e == null) return null;

//             return new SotoResponse
//             {
//                 Reference                = e.Reference,
//                 Customer                 = e.Customer,
//                 CustomerCompanyNumber    = e.CustomerCompanyNumber,
//                 CustomerRepresentative   = e.CustomerRepresentative,
//                 CustomerEmail            = e.CustomerEmail,
//                 SupplierRepresentative   = e.SupplierRepresentative,
//                 SupplierEmail            = e.SupplierEmail,
//                 GovernedBy               = e.GovernedBy,
//                 Problem                  = e.Problem,
//                 CustomerResponsibilities = JsonSerializer.Deserialize<List<string>>(e.CustomerResponsibilitiesJson),
//                 TargetOutcomes           = JsonSerializer.Deserialize<List<TargetOutcomeResponse>>(e.TargetOutcomesJson),

//             };
//         }

//         public async Task UpdateAsync(SotoResponse soto)
//         {
//             var e = await _ctx.Sotos.FindAsync(soto.Reference);
//             if (e == null) throw new KeyNotFoundException($"SOTO {soto.Reference} not found.");

//             // update fields
//             e.CustomerResponsibilitiesJson = JsonSerializer.Serialize(soto.CustomerResponsibilities);
//             e.TargetOutcomesJson           = JsonSerializer.Serialize(soto.TargetOutcomes);
        
//             // you can update other fields too if you wish

//             _ctx.Sotos.Update(e);
//             await _ctx.SaveChangesAsync();
//         }
//     }
// }
