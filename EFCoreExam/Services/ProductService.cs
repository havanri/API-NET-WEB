using AutoMapper;
using EFCoreExam.Helper.Slug;
using EFCoreExam.Models;
using EFCoreExam.Repositories;
using EFCoreExam.UnitOfWork;

namespace EFCoreExam.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IAlbumImageRepository _albumImageRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductService(IUnitOfWork unitOfWork, IProductRepository productRepository, ICategoryRepository categoryRepo, IMapper mapper, IAlbumImageRepository albumImageRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = productRepository;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _albumImageRepo = albumImageRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<Product>> getFullInForAllProduct()
        {
            var products = await _productRepo.getFullInForAllProduct();
            return products;
        }
        public async Task<bool> isExistCategory(int categoryId)
        {
            var category = await _categoryRepo.GetById(categoryId);
            if (category == null)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> CreateProductAsync(CreateProductDto createProductDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Kiểm tra xem sản phẩm có tồn tại chưa
                var product = await _productRepo.GetProductByNameAsync(createProductDto.Name);
                if (product != null)
                {
                    throw new Exception($"Product {createProductDto.Name} already exists");
                }

                // Tạo mới sản phẩm
                product = _mapper.Map<CreateProductDto, Product>(createProductDto);
                product.Slug = StringExtendsions.Slugify(createProductDto.Name);
                await _productRepo.Add(product);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Product>> getAll()
        {
            var products = await _productRepo.getFullInForAllProduct();
            return products;
        }

        public async Task<bool> UpdateProductAsync(int productId, UpdateProductDto updateProductDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var product = await _productRepo.GetById(productId);

                if (product == null)
                {
                    throw new ArgumentException("Product not found");
                }
                product = _mapper.Map<UpdateProductDto, Product>(updateProductDto);
                product.Id = productId;
                product.UpdatedAt = DateTime.Now;
                await _productRepo.Update(productId, product);
                var productTest = _productRepo.GetById(productId);

                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                // Handle exception here
                throw new Exception($"Unable to create new category: {ex.Message}");
            }
        }
        public async Task<bool> UpdateProductImagesAsync(int productId, List<IFormFile> images)
        {
            var product = await _productRepo.GetProductByIdAsync(productId);
            if (product == null)
            {
                return false;
            }
            if (images == null)
            {
                return false;
            }
            // xóa tất cả các hình ảnh của sản phẩm trong database
            _albumImageRepo.BulkDelete(product.AlbumImages);
            var listImages = await UploadAlbumImagesAsync(images);
            // lưu trữ các hình ảnh mới và thêm chúng vào database
            foreach (var image in listImages)
            {
                var imageModel = new AlbumImage
                {
                    ImagePath = image,
                    ImageName = image,
                    CreatedAt = DateTime.Now
                };
                await _albumImageRepo.Add(imageModel);
                await _unitOfWork.SaveAsync();
            }
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        public async Task<List<string>> UploadAlbumImagesAsync(List<IFormFile> albumImages)
        {
            var uploadedImages = new List<string>();

            foreach (var albumImage in albumImages)
            {
                if (albumImage.ContentType != "image/jpeg" && albumImage.ContentType != "image/png")
                {
                    throw new ArgumentException("Only JPG and PNG images are supported.");
                }

                var fileName = Path.GetFileNameWithoutExtension(albumImage.FileName);
                var fileExtension = Path.GetExtension(albumImage.FileName);
                fileName = fileName + "_" + DateTime.Now.Ticks + fileExtension;
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await albumImage.CopyToAsync(fileStream);
                }
                uploadedImages.Add(fileName);
            }

            return uploadedImages;
        }
        public async Task DeleteProduct(int productId)
        {
            var product = await _productRepo.GetProductByIdAsync(productId);
            if (product != null)
            {
                // Xóa liên kết của sản phẩm với các ảnh
                product.AlbumImages.Clear();
                // Xóa sản phẩm
                _productRepo.Delete(product);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
