using EFCoreExam.DTOs.Tag;
using EFCoreExam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _TagService;

        public TagController(ITagService TagService)
        {
            _TagService = TagService;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
        {
            var Tags = await _TagService.GetTagsAsync();
            return Ok(Tags);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TagDto>> GetTag(int id)
        {
            var Tag = await _TagService.GetTagByIdAsync(id);
            if (Tag == null)
            {
                return NotFound();
            }
            return Ok(Tag);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTag([FromForm] CreateTagDto TagDto)
        {
            await _TagService.CreateTagAsync(TagDto);
            return Ok(true);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTag(int id, [FromForm] UpdateTagDto updateTagDto)
        {
            if (id != updateTagDto.Id)
            {
                return BadRequest();
            }
            await _TagService.UpdateTagAsync(id, updateTagDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTag(int id)
        {
            await _TagService.DeleteTagAsync(id);
            return NoContent();
        }
    }
}
