using DevFun.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevFun.Common.Services
{
    public interface IDevJokeService
    {
        Task<DevJoke> GetRandomJoke();
        Task<IEnumerable<DevJoke>> GetJokes();

        Task<DevJoke> Create(DevJoke joke);

        Task<DevJoke> Update(DevJoke joke);

        Task<DevJoke> Delete(int id);
        Task<DevJoke> GetJokeById(int id);
        Task<int> GetCount();
    }
}
