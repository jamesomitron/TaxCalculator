using Domain.Common;

namespace Domain.Entities;

public record TaxProgressiveRates : BaseEntity
{
    public decimal Rate { get; set; }
    public decimal FromValue { get; set; }
    public decimal? ToValue { get; set; }
}
