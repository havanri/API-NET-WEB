using EFCoreExam.DTOs.Attribute;

namespace EFCoreExam.Services
{
    public interface IAttributeService
    {
        Task<IEnumerable<AttributeDto>> GetAttributesAsync();
        Task<AttributeDto> GetAttributeByIdAsync(int id);
        Task CreateAttributeAsync(CreateAttributeDto createAttributeDto);
        Task UpdateAttributeAsync(int id, UpdateAttributeDto updateAttributeDto);
        Task DeleteAttributeAsync(int id);
    }
}
