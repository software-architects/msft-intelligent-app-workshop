using _4tecture.DataAccess.Common.Repositories;
using DevFun.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using _4tecture.DataAccess.Common.Storages;
using DevFun.Common.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DevFun.Logic.Repositories
{
    public class DevJokeRepository : RepositoryBase<DevJoke, int>, IDevJokeRepository
    {
        public DevJokeRepository(IStorage storage) : base(storage)
        {
        }

        protected override Expression<Func<DevJoke, bool>> GetPrimaryKeyFilterExpression(int keyValue)
        {
            return e => e.Id == keyValue;
        }

        protected override IQueryable<DevJoke> ApplyDefaultIncludes(IQueryable<DevJoke> query)
        {
            return query;
        }
    }
}
