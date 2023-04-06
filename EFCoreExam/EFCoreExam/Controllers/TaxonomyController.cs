using EFCoreExam.DTOs.Taxonomy;
using EFCoreExam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxonomyController : ControllerBase
    {
        private readonly ITaxonomyService _TaxonomyService;

        public TaxonomyController(ITaxonomyService TaxonomyService)
        {
            _TaxonomyService = TaxonomyService;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<TaxonomyDto>>> GetTaxonomys()
        {
            var Taxonomys = await _TaxonomyService.GetTaxonomiesAsync();
            return Ok(Taxonomys);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TaxonomyDto>> GetTaxonomy(int id)
        {
            var Taxonomy = await _TaxonomyService.GetTaxonomyByIdAsync(id);
            if (Taxonomy == null)
            {
                return NotFound();
            }
            return Ok(Taxonomy);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTaxonomy([FromForm] CreateTaxonomyDto TaxonomyDto)
        {
            await _TaxonomyService.CreateTaxonomyAsync(TaxonomyDto);
            return Ok(true);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTaxonomy(int id, [FromForm] UpdateTaxonomyDto updateTaxonomyDto)
        {
            if (id != updateTaxonomyDto.Id)
            {
                return BadRequest();
            }
            await _TaxonomyService.UpdateTaxonomyAsync(id, updateTaxonomyDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaxonomy(int id)
        {
            await _TaxonomyService.DeleteTaxonomyAsync(id);
            return NoContent();
        }
    }
}
