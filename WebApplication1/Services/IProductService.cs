using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();

    Task<ProductDto?> GetByIdAsync(int id);

    Task<Product> CreateAsync(CreateProductDto product);

    Task<bool> UpdateAsync(int id, UpdateProductDto product);
    Task<bool> DeleteAsync(int id);
}