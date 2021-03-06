using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace ConsumerModule.GitLab.Data
{
    public abstract class BaseMongoDB<TEntity> : IMongoDB<TEntity> where TEntity : IEntity
    {
        // Client for establishing a connection
        private readonly IMongoClient _mongoClient;
        // Connection ready database
        protected readonly IMongoDatabase Database;
        protected readonly string CollectionName = $"{typeof(TEntity).Name.ToLower()}";
        
        // Shorthand methods for using update, sort, filtere, Projection and IndexKeys!
        protected UpdateDefinitionBuilder<TEntity> Update => Builders<TEntity>.Update;
        protected SortDefinitionBuilder<TEntity> Sort => Builders<TEntity>.Sort;
        protected FilterDefinitionBuilder<TEntity> Filter => Builders<TEntity>.Filter;
        protected ProjectionDefinitionBuilder<TEntity> Projection => Builders<TEntity>.Projection;
        protected IndexKeysDefinitionBuilder<TEntity> IndexKeys => Builders<TEntity>.IndexKeys;
        
        public BaseMongoDB(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;

            // Connect to database --> Find database by name or create if not exists
            Database = _mongoClient.GetDatabase(Program.mongoConfig.DefaultDatabaseName);
            
            // Search for collection of type TEntity, if not exists create an empty collection
            if (!CollectionExists(Database, CollectionName))
            {
                Database.CreateCollection(CollectionName);
            }
            
            // Apply enum string conventionpack
            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String)
            };
            ConventionRegistry.Register("EnumStringConvention", pack, t => true);
        }

        private bool CollectionExists(IMongoDatabase db, string collectionName)
        {
            return db.ListCollections(
                new ListCollectionsOptions {Filter = new BsonDocument("name", collectionName)}).Any();
        }

        protected IMongoCollection<TEntity> Collection()
        {
            return Database.GetCollection<TEntity>(CollectionName);
        }

        public async Task<bool> Delete(Guid id)
        {
            return (await Collection().DeleteOneAsync(entity => entity.Id == id)).IsAcknowledged;
        }

        public async Task<IEnumerable<TEntity>> GetAll(int skip = 0, int limit = 100)
        {
            return await Collection()
                .Find(Filter.Empty)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetPaged(int page, int pagesize)
        {
            return await GetAll(page * pagesize, pagesize);
        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await Collection().Find(entity => entity.Id == id).SingleOrDefaultAsync();
        }

        public virtual async Task<Guid> Insert(TEntity entity)
        {
            await Collection().InsertOneAsync(entity, new InsertOneOptions());

            return entity.Id;
        }
    }
}