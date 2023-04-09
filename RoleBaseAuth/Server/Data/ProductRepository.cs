using RoleBaseAuth.Server.Interfaces;
using RoleBaseAuth.Shared.Model;

namespace RoleBaseAuth.Server.Data
{
    class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
