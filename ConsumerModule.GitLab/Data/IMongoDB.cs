using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsumerModule.GitLab.Data
{
    public interface IMongoDB<TEntity> where TEntity : IEntity
    {
        Task<bool> Delete(Guid id);
        Task<IEnumerable<TEntity>> GetAll(int skip = 0, int limit = 100);
        Task<IEnumerable<TEntity>> GetPaged(int page, int pagesize);
        Task<TEntity> GetById(Guid id);
        Task<Guid> Insert(TEntity entity);
    }
}