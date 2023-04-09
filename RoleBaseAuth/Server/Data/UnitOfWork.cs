using RoleBaseAuth.Server.Interfaces;
using System;

namespace RoleBaseAuth.Server.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IProductRepository Products { get; }

        public UnitOfWork(ApplicationDbContext DbContext,
            IProductRepository productRepository)
        {
            this._context = DbContext;

            this.Products = productRepository;
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
