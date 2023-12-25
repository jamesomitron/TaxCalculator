using Domain.Common;

namespace Domain.Entities;

public record TaxRecord : BaseEntity
{
    public TaxPostalCode PostalCode { get; set; }

    public decimal AnnualIncome { get; set; }

    public decimal CalculatedTaxValue { get; set; }
}
