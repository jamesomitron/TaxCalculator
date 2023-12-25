using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using TaxCalculatorAPI.Request;

namespace TaxCalculatorAPI.Validations
{

    public class TaxRecordValidator : AbstractValidator<TaxRecordRequest>
    {
        ApplicationDBContext _context;
        public TaxRecordValidator(ApplicationDBContext context)
        {
            _context = context;

            RuleFor(x => x.PostalCode).NotEmpty().WithMessage("Please specify a postal code");
            RuleFor(x => x.AnnualIncome).NotEmpty().WithMessage("Please enter an annual income");
            RuleFor(x => x.AnnualIncome).PrecisionScale(10, 2, true).WithMessage("Please enter a valid annual income");
            RuleFor(x => x.PostalCode).MustAsync(BeAValidPostcode).WithMessage("Please specify a valid postcode");
        }

        private async Task<bool> BeAValidPostcode(string postcode, CancellationToken token)
        {
            var unitOfWork = new UnitOfWork(_context);

            var postalCodes = await unitOfWork.TaxPostalCode.GetPostalCodeByCodeAsync(postcode);

            if (postalCodes is null)
            {
                return false;
            }

            return true;
        }

    }
}
