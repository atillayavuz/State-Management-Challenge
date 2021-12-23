using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StateManagement.Data.Repositories.Mongo
{
    public interface IMongoRepository<T>
    {
        Task<List<T>> SearchAsync<T>(Expression<Func<T, bool>> expression, int skip = 0, int limit = 100, string collectionName = null) where T : class;

        List<T> Search<T>(Expression<Func<T, bool>> expression, int skip = 0, int limit = 100, string collectionName = null) where T : class;

        Task<T> GetAsync<T>(Expression<Func<T, bool>> expression, string collectionName = null) where T : class;

        Task InsertOneAsync<T>(T item, string collectionName = null) where T : class;

        Task UpdateOneAsync<T>(Expression<Func<T, bool>> expression, T item, string collectionName = null) where T : class;

        Task DeleteManyAsync<T>(Expression<Func<T, bool>> expression, string collectionName = null) where T : class;

        Task DeleteOneAsync<T>(Expression<Func<T, bool>> expression, string collectionName = null) where T : class;
    }
}
