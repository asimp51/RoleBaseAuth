using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoleBaseAuth.Server.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<List<T>> GetAll();
        Task Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
