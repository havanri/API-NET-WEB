using EFCoreExam.DTOs.Tag;

namespace EFCoreExam.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetTagsAsync();
        Task<TagDto> GetTagByIdAsync(int id);
        Task CreateTagAsync(CreateTagDto TagDto);
        Task UpdateTagAsync(int id, UpdateTagDto updateTagDto);
        Task DeleteTagAsync(int id);
    }
}
