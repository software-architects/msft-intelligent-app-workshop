using Erp.Logic;
using System;
using Xunit;

namespace Erp.Test
{
    public class TestRevenueCalculator
    {
        [Fact]
        public void TestTotalRevenueCalculation()
        {
            Assert.Equal(42, RevenueCalculator.CalculateTotalRevenue(2, 21));
        }
    }
}
