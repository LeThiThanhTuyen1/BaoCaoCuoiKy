namespace InventoryManagement.Models
{
    public class StatisticsViewModel
    {
        public decimal TotalInventoryValue { get; set; }
        public int TotalQuantityImported { get; set; }
        public decimal TotalValueImported { get; set; }
        public int TotalQuantityExported { get; set; }
        public decimal TotalValueExported { get; set; }
        public IEnumerable<History> Histories { get; set; }
    }
}