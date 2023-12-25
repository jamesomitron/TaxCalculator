using taxRecordEntity = Domain.Entities;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Persistence;
using TaxCalculatorAPI.Request;
using Application.TaxCalculation;
using Microsoft.AspNetCore.Mvc;

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

        app.MapPost("/api/calculatetax", async (IValidator<TaxRecordRequest> validator, ITaxCalculationFactory taxCalculationFactory, [FromBody]TaxRecordRequest taxRecordRequest) =>
        {
            var validationResult = await validator.ValidateAsync(taxRecordRequest);
            if(!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            try
            {
               
                var code = await unitOfWork.TaxPostalCode.GetPostalCodeByCodeAsync(taxRecordRequest.PostalCode);
                if (code == null)
                {
                    return Results.NotFound("Invalid Postal Code Entered");
                }

                var taxConfig = await unitOfWork.TaxConfigValue.GetConfigValueAsync();
                var taxProgressiveRate = await unitOfWork.TaxProgressiveRates.GetProgressiveTaxMatrixAsync();
                
                var _taxCalculationFactory = taxCalculationFactory.GetTaxCalculationType(code.TaxCalculationType);

                var tax = _taxCalculationFactory.Execute(
                        taxRecordRequest.AnnualIncome,
                        taxConfig,
                        taxProgressiveRate
                    );

                var taxRecord = new taxRecordEntity.TaxRecord { AnnualIncome = taxRecordRequest.AnnualIncome, PostalCode = code, CalculatedTaxValue = tax };
                unitOfWork.TaxRecord.Add(taxRecord);
                await unitOfWork.CompleteAsync();

                return Results.Ok(taxRecord);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }

        });
    }


}