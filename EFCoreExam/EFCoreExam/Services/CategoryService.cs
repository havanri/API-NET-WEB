using AutoMapper;
using EFCoreExam.DTOs.Category;
using EFCoreExam.Helper.Slug;
using EFCoreExam.Models;
using EFCoreExam.Repositories;
using EFCoreExam.UnitOfWork;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace EFCoreExam.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        private string GetETagFromData(object data)
        {
            if (data == null) return null;
            var json = JsonConvert.SerializeObject(data);
            var bytes = Encoding.UTF8.GetBytes(json);
            var hash = MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAll();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ThumbnailPath = c.ThumbnailPath,
                Slug = c.Slug
                // map other properties
            });
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category == null)
            {
                // handle not found
            }
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                // map other properties
            };
        }

        public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var category = _mapper.Map<Category>(createCategoryDto);
                category.Slug = StringExtendsions.Slugify(category.Name);
                category.CreatedAt = DateTime.Now;
                if(createCategoryDto.formFile != null)
                {
                    category.Thumbnail = createCategoryDto.formFile.FileName;
                    var filename = Guid.NewGuid().ToString() + ".png";
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", filename);
                    var stream = System.IO.File.Create(path);
                    createCategoryDto.formFile.CopyTo(stream);
                    stream.Close();
                    category.ThumbnailPath = Path.Combine("images", filename);
                }
                await _categoryRepository.Add(category);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch(Exception ex)
            {
                _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
            
        }

        public async Task UpdateCategoryAsync(int id,UpdateCategoryDto updateCategoryDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var category = await _categoryRepository.GetById(id);
                if (category == null)
                {
                    throw new ArgumentException("Category not found");
                }
                category = _mapper.Map<Category>(updateCategoryDto);
                category.UpdatedAt = DateTime.Now;
                if (updateCategoryDto.formFile != null)
                {
                    category.Thumbnail = updateCategoryDto.formFile.FileName;
                    var filename = Guid.NewGuid().ToString() + ".png";
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", filename);
                    var stream = System.IO.File.Create(path);
                    updateCategoryDto.formFile.CopyTo(stream);
                    stream.Close();
                    category.ThumbnailPath = Path.Combine("images", filename);
                }
                // map other properties
                await _categoryRepository.Update(id, category);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category == null)
            {
                throw new ArgumentException("Category not found");
            }
            _categoryRepository.Delete(category);
            await _unitOfWork.SaveAsync();
        }
    }
}
