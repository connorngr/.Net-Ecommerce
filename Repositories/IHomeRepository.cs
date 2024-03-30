using WebApp.Models;

namespace WebApp.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Category>> Categories();
        Task<IEnumerable<Product>> GetProducts(string Search = "", int categoryID = 0);
    }
}