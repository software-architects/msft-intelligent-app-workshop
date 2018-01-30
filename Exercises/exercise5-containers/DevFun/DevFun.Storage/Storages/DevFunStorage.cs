using _4tecture.DataAccess.EntityFramework.Storages;
using DevFun.Common.Storages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevFun.Storage.Storages
{
    public class DevFunStorage: EFStorageBase, IDevFunStorage
    {
        public DevFunStorage(DbContextOptions options, IEnumerable<IRelationalEntityConfiguration> entityConfigurations) : base(options, entityConfigurations)
        {
        }
    }
}
