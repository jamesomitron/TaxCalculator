namespace TaxCalculatorAPI.Request
{
    public record TaxRecordRequest
    {
        public string? PostalCode { get; set; }
        public decimal AnnualIncome { get; set; }
    }
}
