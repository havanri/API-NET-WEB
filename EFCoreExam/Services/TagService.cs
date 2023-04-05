using AutoMapper;
using EFCoreExam.DTOs.Tag;
using EFCoreExam.Helper.Slug;
using EFCoreExam.Models;
using EFCoreExam.Repositories;
using EFCoreExam.UnitOfWork;

namespace EFCoreExam.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITagRepository _TagRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TagService(IUnitOfWork unitOfWork, ITagRepository TagRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _TagRepository = TagRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<TagDto>> GetTagsAsync()
        {
            var Tags = await _TagRepository.GetAll();
            return Tags.Select(c => new TagDto
            {
                Id = c.Id,
                Name = c.Name,
                // map other properties
            });
        }

        public async Task<TagDto> GetTagByIdAsync(int id)
        {
            var Tag = await _TagRepository.GetById(id);
            if (Tag == null)
            {
                // handle not found
            }
            return new TagDto
            {
                Id = Tag.Id,
                Name = Tag.Name,
                // map other properties
            };
        }

        public async Task CreateTagAsync(CreateTagDto createTagDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var Tag = _mapper.Map<Tag>(createTagDto);
                Tag.CreatedAt = DateTime.Now;
                await _TagRepository.Add(Tag);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }

        }

        public async Task UpdateTagAsync(int id, UpdateTagDto updateTagDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var Tag = await _TagRepository.GetById(id);
                if (Tag == null)
                {
                    throw new ArgumentException("Tag not found");
                }
                Tag = _mapper.Map<Tag>(updateTagDto);
                Tag.UpdatedAt = DateTime.Now;
                // map other properties
                await _TagRepository.Update(id, Tag);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task DeleteTagAsync(int id)
        {
            var Tag = await _TagRepository.GetById(id);
            if (Tag == null)
            {
                throw new ArgumentException("Tag not found");
            }
            _TagRepository.Delete(Tag);
            await _unitOfWork.SaveAsync();
        }
    }
}
