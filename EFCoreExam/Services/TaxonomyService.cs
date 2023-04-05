using AutoMapper;
using EFCoreExam.DTOs.Taxonomy;
using EFCoreExam.Models;
using EFCoreExam.Repositories;
using EFCoreExam.UnitOfWork;

namespace EFCoreExam.Services
{
    public class TaxonomyService : ITaxonomyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaxonomyRepository _TaxonomyRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TaxonomyService(IUnitOfWork unitOfWork, ITaxonomyRepository TaxonomyRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _TaxonomyRepository = TaxonomyRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<TaxonomyDto> GetTaxonomyByIdAsync(int id)
        {
            var Taxonomy = await _TaxonomyRepository.GetById(id);
            if (Taxonomy == null)
            {
                // handle not found
            }
            return new TaxonomyDto
            {
                Id = Taxonomy.Id,
                Name = Taxonomy.Name,
                // map other properties
            };
        }

        public async Task CreateTaxonomyAsync(CreateTaxonomyDto createTaxonomyDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var Taxonomy = _mapper.Map<Taxonomy>(createTaxonomyDto);
                Taxonomy.CreatedAt = DateTime.Now;
                await _TaxonomyRepository.Add(Taxonomy);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }

        }

        public async Task UpdateTaxonomyAsync(int id, UpdateTaxonomyDto updateTaxonomyDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var Taxonomy = await _TaxonomyRepository.GetById(id);
                if (Taxonomy == null)
                {
                    throw new ArgumentException("Taxonomy not found");
                }
                Taxonomy = _mapper.Map<Taxonomy>(updateTaxonomyDto);
                Taxonomy.UpdatedAt = DateTime.Now;
                // map other properties
                await _TaxonomyRepository.Update(id, Taxonomy);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task DeleteTaxonomyAsync(int id)
        {
            var Taxonomy = await _TaxonomyRepository.GetById(id);
            if (Taxonomy == null)
            {
                throw new ArgumentException("Taxonomy not found");
            }
            _TaxonomyRepository.Delete(Taxonomy);
            await _unitOfWork.SaveAsync();
        }

        public Task<IEnumerable<TaxonomyDto>> GetTaxonomiesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
