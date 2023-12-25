
using Domain.Entities;
using Domain.Enums;

namespace Application.TaxCalculation;

public interface ITaxTypeByPostalCode
{
    TaxType Execute(string PostalCode, IEnumerable<TaxPostalCode> TaxPostalCodeList);

}
