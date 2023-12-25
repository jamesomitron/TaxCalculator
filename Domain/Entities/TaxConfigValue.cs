using Domain.Common;

namespace Domain.Entities;

public record TaxConfigValue : BaseEntity
{
    public string Item { get; init; }
    public decimal Value { get; init; }
}
