using Infrastructure.Data;
using Infrastructure.Persistence;

namespace TaxCalculatorAPI.EndPoints;

public static class TaxPostalCode
{
     
    public static void AddPostalCodeEndPoints(this IEndpointRouteBuilder app, ApplicationDBContext context)
    {
        var unitOfWork = new UnitOfWork(context);

        app.MapGet("/api/postalcode", async () =>
        {
            var output = await unitOfWork.TaxPostalCode.GetPostalCodeAsync();

            return Results.Ok(output);
        });
    } 

    
}
