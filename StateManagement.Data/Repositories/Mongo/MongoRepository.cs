using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StateManagement.Data.Repositories.Mongo
{
    public class MongoRepository<T> : IMongoRepository<T>
    {
        private protected IMongoClient _client;
        private protected IMongoDatabase _database;

        public MongoRepository(IMongoClient client)
        {
            _client = client;
            _database = _client.GetDatabase("StateManagement");
        }

        private IMongoCollection<T> Collection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName ?? typeof(T).Name);
        }

        public async Task<List<T>> SearchAsync<T>(Expression<Func<T, bool>> expression, int skip = 0, int limit = 100, string collectionName = null) where T : class
        {
            return await Collection<T>(collectionName).Find(expression).Skip(skip).Limit(limit).ToListAsync();
        }

        public List<T> Search<T>(Expression<Func<T, bool>> expression, int skip = 0, int limit = 100, string collectionName = null) where T : class
        {
            return Collection<T>(collectionName).Find(expression).Skip(skip).Limit(limit).ToList();
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> expression, string collectionName = null) where T : class
        {
            return await Collection<T>(collectionName ?? typeof(T).Name).Find(expression).FirstOrDefaultAsync();
        }

        public async Task InsertOneAsync<T>(T item, string collectionName = null) where T : class
        {
            await Collection<T>(collectionName ?? typeof(T).Name).InsertOneAsync(item);
        }
        public async Task UpdateOneAsync<T>(Expression<Func<T, bool>> expression, T item, string collectionName = null) where T : class
        {
            await Collection<T>(collectionName ?? typeof(T).Name).ReplaceOneAsync(expression, item);
        }

        public async Task DeleteManyAsync<T>(Expression<Func<T, bool>> expression, string collectionName = null) where T : class
        {
            await Collection<T>(collectionName ?? typeof(T).Name).DeleteManyAsync(expression);
        }

        public async Task DeleteOneAsync<T>(Expression<Func<T, bool>> expression, string collectionName = null) where T : class
        {
            await Collection<T>(collectionName ?? typeof(T).Name).DeleteOneAsync(expression);
        }
    }
}
