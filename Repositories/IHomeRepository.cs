using Innerglow_App.Models;

namespace Innerglow_App.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Category>> Categories();
        Task<IEnumerable<Product>> GetProducts(string Search = "", int categoryID = 0);
    }
}