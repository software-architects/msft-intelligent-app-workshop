using DevFun.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;
using DevFun.Common.Entities;
using System.Threading.Tasks;
using DevFun.Common.Storages;
using Microsoft.Extensions.Logging;
using _4tecture.DataAccess.Common.Storages;
using DevFun.Common.Repositories;
using System.Linq;

namespace DevFun.Logic.Services
{
    public class DevJokeService : IDevJokeService
    {
        public IStorageFactory<IDevFunStorage> StorageFactory { get; private set; }
        public ILogger Logger { get; private set; }

        public DevJokeService(
            IStorageFactory<IDevFunStorage> storageFactory,
            ILoggerFactory loggerFactory)
        {
            this.StorageFactory = storageFactory ?? throw new ArgumentNullException(nameof(storageFactory));
            this.Logger = loggerFactory.CreateLogger(nameof(DevJokeService));
        }

     

        public async Task<DevJoke> Create(DevJoke joke)
        {
            using (var session = StorageFactory.CreateStorageSession())
            {
                var repo = session.ResolveRepository<IDevJokeRepository>();
                var result = repo.Add(joke);
                await session.SaveChanges();
                return result;
            }
        }

        public async Task<DevJoke> Delete(int id)
        {
            using (var session = StorageFactory.CreateStorageSession())
            {
                var repo = session.ResolveRepository<IDevJokeRepository>();
                var result = repo.Delete(id);
                await session.SaveChanges();
                return result;
            }
        }

        public Task<IEnumerable<DevJoke>> GetJokes()
        {
            using (var session = StorageFactory.CreateStorageSession())
            {
                var repo = session.ResolveRepository<IDevJokeRepository>();
                var result = repo.GetAll().ToList();
                return Task.FromResult(result.AsEnumerable<DevJoke>());
            }
        }

        public Task<int> GetCount()
        {
            using (var session = StorageFactory.CreateStorageSession())
            {
                var repo = session.ResolveRepository<IDevJokeRepository>();
                var result = repo.GetAll().Count();
                return Task.FromResult(result);
            }
        }

        public Task<DevJoke> GetJokeById(int id)
        {
            using (var session = StorageFactory.CreateStorageSession())
            {
                var repo = session.ResolveRepository<IDevJokeRepository>();
                var result = repo.GetById(id);
                return Task.FromResult(result);
            }
        }

        public Task<DevJoke> GetRandomJoke()
        {
            using (var session = StorageFactory.CreateStorageSession())
            {
                var repo = session.ResolveRepository<IDevJokeRepository>();
                var result = repo.GetAll().OrderBy(r => Guid.NewGuid()).FirstOrDefault();
                return Task.FromResult(result);
            }
        }

        public async Task<DevJoke> Update(DevJoke joke)
        {
            using (var session = StorageFactory.CreateStorageSession())
            {
                var repo = session.ResolveRepository<IDevJokeRepository>();
                var result = repo.Update(joke);
                await session.SaveChanges();
                return result;
            }
        }

        
    }
}
