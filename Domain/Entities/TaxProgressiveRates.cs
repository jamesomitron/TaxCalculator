using Domain.Common;

namespace Domain.Entities;

public record TaxProgressiveRates : BaseEntity
{
    public decimal Rate { get; set; }
    public int FromValue { get; set; }
    public int ToValue { get; set; }
}
