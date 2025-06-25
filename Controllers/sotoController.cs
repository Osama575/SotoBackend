using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SotoGeneratorAPI.Models;
using SotoGeneratorAPI.Services;
// using SotoGeneratorAPI.Data.Repositories;


namespace SotoGeneratorAPI.Controllers
{   
    [ApiController]
    [Route("soto")]
    public class SotoController : ControllerBase
    {
        private readonly SotoGeneratorService _service;
        // private readonly ISotoRepository _repo;

        // public SotoController(SotoGeneratorService service, ISotoRepository repo)
         public SotoController(SotoGeneratorService service)
        {
            _service = service;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateSoto([FromBody] SotoRequest request)
        {
            var response = await _service.GenerateSotoAsync(request);
            return Ok(response);
        }

    //     [HttpGet("{reference}")]
    //     public async Task<IActionResult> Get(string reference)
    //     {
    //         var soto = await _repo.GetAsync(reference);
    //         return soto is null ? NotFound() : Ok(soto);
    //     }

    //     [HttpPut("{reference}")]
    //     public async Task<IActionResult> Update(string reference, [FromBody] SotoResponse updated)
    //     {
    //         updated.Reference = reference;
    //         await _repo.UpdateAsync(updated);
    //         return NoContent();
    //     }
     }
}
