using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevFun.Common.Storages
{
    public interface IDataInitializer
    {
        Task InitializeData();
    }
}
