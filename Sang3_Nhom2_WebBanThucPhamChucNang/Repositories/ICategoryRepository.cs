using Sang3_Nhom2_WebBanThucPhamChucNang.Models;

namespace Sang3_Nhom2_WebBanThucPhamChucNang.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }

}
