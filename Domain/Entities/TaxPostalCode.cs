using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public record TaxPostalCode : BaseEntity
{
    public string Code { get; init; }
    public TaxType TaxCalculationType { get; init; }
}

