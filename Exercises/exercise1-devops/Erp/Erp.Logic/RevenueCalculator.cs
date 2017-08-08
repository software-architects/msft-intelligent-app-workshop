namespace Erp.Logic
{
    public static class RevenueCalculator
    {
        public static decimal CalculateTotalRevenue(int amount, decimal unitPrice)
        {
            return amount * unitPrice;
        }
    }
}
