namespace MyAppCore.Dtos
{
    public class SaleDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string StockName { get; set; }
        public int Amount { get; set; }
        public int TotalPrice { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
