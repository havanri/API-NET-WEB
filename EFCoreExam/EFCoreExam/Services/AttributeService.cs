using AutoMapper;
using EFCoreExam.DTOs.Attribute;
using EFCoreExam.Repositories;
using EFCoreExam.UnitOfWork;
using Attribute = EFCoreExam.Models.Attribute;

namespace EFCoreExam.Services
{
    public class AttributeService : IAttributeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttributeRepository _AttributeRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AttributeService(IUnitOfWork unitOfWork, IAttributeRepository AttributeRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _AttributeRepository = AttributeRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<AttributeDto>> GetAttributesAsync()
        {
            var Attributes = await _AttributeRepository.GetAll();
            return Attributes.Select(c => new AttributeDto
            {
                Id = c.Id,
                Name = c.Name,
                // map other properties
            });
        }

        public async Task<AttributeDto> GetAttributeByIdAsync(int id)
        {
            var Attribute = await _AttributeRepository.GetById(id);
            if (Attribute == null)
            {
                // handle not found
            }
            return new AttributeDto
            {
                Id = Attribute.Id,
                Name = Attribute.Name,
                // map other properties
            };
        }

        public async Task CreateAttributeAsync(CreateAttributeDto createAttributeDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var Attribute = _mapper.Map<Attribute>(createAttributeDto);
                Attribute.CreatedAt = DateTime.Now;
                await _AttributeRepository.Add(Attribute);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }

        }

        public async Task UpdateAttributeAsync(int id, UpdateAttributeDto updateAttributeDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var Attribute = await _AttributeRepository.GetById(id);
                if (Attribute == null)
                {
                    throw new ArgumentException("Attribute not found");
                }
                Attribute = _mapper.Map<Attribute>(updateAttributeDto);
                Attribute.UpdatedAt = DateTime.Now;
                // map other properties
                await _AttributeRepository.Update(id, Attribute);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task DeleteAttributeAsync(int id)
        {
            var Attribute = await _AttributeRepository.GetById(id);
            if (Attribute == null)
            {
                throw new ArgumentException("Attribute not found");
            }
            _AttributeRepository.Delete(Attribute);
            await _unitOfWork.SaveAsync();
        }
    }
}
