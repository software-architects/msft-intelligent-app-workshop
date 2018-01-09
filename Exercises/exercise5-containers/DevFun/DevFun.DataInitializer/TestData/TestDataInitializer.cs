using _4tecture.Modularity.Common.EventAggregation;
using DevFun.Common.Storages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using _4tecture.DependencyInjection.Common;
using DevFun.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using DevFun.Common.Entities;
using System.Linq;

namespace DevFun.DataInitializer.TestData
{
    public class TestDataInitializer : IDataInitializer
    {
        private IServiceProvider serviceLocator;

        public TestDataInitializer(IServiceProvider serviceProvider)
        {
            this.serviceLocator = serviceProvider;
        }

        public async Task InitializeData()
        {
            var service = serviceLocator.GetRequiredService<IDevJokeService>();

            var count = await service.GetCount();
            if (count == 0)
            {
                await service.Create(new DevJoke() { Text = @"Programmer\r\nA machine that turns coffee into code." });
                await service.Create(new DevJoke() { Text = @"Programmer\r\nA person who fixed a problem that you don't know your have, in a way you don't understand." });
                await service.Create(new DevJoke() { Text = @"Algorithm\r\nWord used by programmers when... they do not want to explain what they did." });
                await service.Create(new DevJoke() { Text = @"Q: What's the object-oriented way to become wealthy?\r\nA: Inheritance" });
                await service.Create(new DevJoke() { Text = @"Q: What's the programmer's favourite hangout place?\r\nA: Foo Bar" });
                await service.Create(new DevJoke() { Text = @"Q: How to you tell an introverted computer scientist from an extroverted computer scientist?\r\nA: An extroverted computer scientist looks at your shoes when he talks to you." });
                await service.Create(new DevJoke() { Text = @"Q: Why do Java programmers wear glasses?\r\nA: Because they don't C#" });

            }
        }
    }
}
