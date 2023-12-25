using taxRecordEntity = Domain.Entities;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Persistence;
using TaxCalculatorAPI.Request;
using Application.TaxCalculation;

namespace TaxCalculatorAPI.EndPoints;

public static class TaxRecord
{

    public static void AddTaxRecordEndPoints(this IEndpointRouteBuilder app, ApplicationDBContext context)
    {
        var unitOfWork = new UnitOfWork(context);

        app.MapGet("/api/taxrecord", async () =>
        {
            var output = await unitOfWork.TaxRecord.GetTaxRecordAsync();

            return Results.Ok(output);
        });

        app.MapGet("/api/calculatetax", async (IValidator<TaxRecordRequest> validator, ITaxCalculationFactory taxCalculationFactory, TaxRecordRequest taxRecordRequest) =>
        {
            try
            {
               
                var code = await unitOfWork.TaxPostalCode.GetPostalCodeByCodeAsync(taxRecordRequest.PostalCode);
                var taxConfig = await unitOfWork.TaxConfigValue.GetConfigValueAsync();
                var taxProgressiveRate = await unitOfWork.TaxProgressiveRates.GetProgressiveTaxMatrixAsync();

                if (code == null)
                {
                    return Results.NotFound("Invalid Postal Code Entered");
                }

                var _taxCalculationFactory = taxCalculationFactory.GetTaxCalculationType(code.TaxCalculationType);

                var tax = _taxCalculationFactory.Execute(
                        taxRecordRequest.AnnualIncome,
                        taxConfig,
                        taxProgressiveRate
                    );

                var taxRecord = new taxRecordEntity.TaxRecord { AnnualIncome = taxRecordRequest.AnnualIncome, PostalCode = code, CalculatedTaxValue = tax };
                unitOfWork.TaxRecord.Add(taxRecord);

                return Results.Ok(taxRecord);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }

        });
    }


}