using EFCoreExam.DTOs.Taxonomy;

namespace EFCoreExam.Services
{
    public interface ITaxonomyService
    {
        Task<IEnumerable<TaxonomyDto>> GetTaxonomiesAsync();
        Task<TaxonomyDto> GetTaxonomyByIdAsync(int id);
        Task CreateTaxonomyAsync(CreateTaxonomyDto createTaxonomyDto);
        Task UpdateTaxonomyAsync(int id, UpdateTaxonomyDto updateTaxonomyDto);
        Task DeleteTaxonomyAsync(int id);
    }
}
