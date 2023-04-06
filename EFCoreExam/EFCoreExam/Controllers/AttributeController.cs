using EFCoreExam.DTOs.Attribute;
using EFCoreExam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributeService _AttributeService;

        public AttributeController(IAttributeService AttributeService)
        {
            _AttributeService = AttributeService;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<AttributeDto>>> GetAttributes()
        {
            var Attributes = await _AttributeService.GetAttributesAsync();
            return Ok(Attributes);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AttributeDto>> GetAttribute(int id)
        {
            var Attribute = await _AttributeService.GetAttributeByIdAsync(id);
            if (Attribute == null)
            {
                return NotFound();
            }
            return Ok(Attribute);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAttribute([FromForm] CreateAttributeDto AttributeDto)
        {
            await _AttributeService.CreateAttributeAsync(AttributeDto);
            return Ok(true);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAttribute(int id, [FromForm] UpdateAttributeDto updateAttributeDto)
        {
            if (id != updateAttributeDto.Id)
            {
                return BadRequest();
            }
            await _AttributeService.UpdateAttributeAsync(id, updateAttributeDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAttribute(int id)
        {
            await _AttributeService.DeleteAttributeAsync(id);
            return NoContent();
        }
    }
}
