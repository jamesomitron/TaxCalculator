using Domain.Repository;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using TaxCalculationTest.Infrastucture.Tests.DBContextExtension;

namespace TaxCalculationTest.Infrastucture.Tests;

public class TaxRepositoryTest
{
    private ApplicationDBContext _context;

    private ITaxConfigValueRepository _taxConfigValueRepository;
    private ITaxProgressiveRatesRepository _taxProgressiveRatesRepository;
    private ITaxPostalCodeRepository _taxPostalCodeRepository;
    private ITaxRecordRepository _taxRecordRepository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDBContext>()
           .UseInMemoryDatabase($"TestTaxRecord-{Guid.NewGuid()}")
           .Options;

        _context = new ApplicationDBContext(options);

        _ = _context.Database.EnsureDeleted();
        _ = _context.Database.EnsureCreated();
        _ = _context.AddTestData();

        _taxPostalCodeRepository = new TaxPostalCodeRepository(_context);
        _taxConfigValueRepository = new TaxConfigValueRepository(_context);
        _taxProgressiveRatesRepository = new TaxProgressiveRatesRepository(_context);
        _taxRecordRepository = new TaxRecordRepository(_context);
    }

    [Test]
    public async Task TaxConfigValueRepository_GetCountOfTaxConfigValues()
    {
        //act
        var result = await _taxConfigValueRepository.GetConfigValueAsync();

        //assert
        Assert.That(result.Count(), Is.EqualTo(4));
    }

    [Test]
    public async Task TaxConfigValueRepository_GetTaxConfigValues()
    {
        //act
        var result = await _taxConfigValueRepository.GetConfigValueAsync();

        //assert
        Assert.That(result.Where(o => o.Item.Equals("FlatRate")).ToList().Count(), Is.EqualTo(1));
        Assert.That(result.Where(o => o.Item.Equals("FlatValue")).ToList().Count(), Is.EqualTo(1));
        Assert.That(result.Where(o => o.Item.Equals("FlatValueRate")).ToList().Count(), Is.EqualTo(1));
        Assert.That(result.Where(o => o.Item.Equals("MinimumAnnualIncome")).ToList().Count(), Is.EqualTo(1));
        Assert.That(result.Where(o => o.Item.Equals("Invalid")).ToList().Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task TaxProgressiveRatesRepository_GetProgressiveRates()
    {
        //act
        var output = await _taxProgressiveRatesRepository.GetProgressiveTaxMatrixAsync();
        var result = output.FirstOrDefault(o => o.Rate.Equals(10));

        //assert
        Assert.That(result.Rate, Is.EqualTo(10));
        Assert.That(result.FromValue, Is.EqualTo(0));
        Assert.That(result.ToValue, Is.EqualTo(8350));
    }

    [Test]
    public async Task TaxProgressiveRatesRepository_GetProgressiveRates_InvalidRate()
    {
        //act
        var output = await _taxProgressiveRatesRepository.GetProgressiveTaxMatrixAsync();
        var result = output.FirstOrDefault(o => o.Rate.Equals(0));

        //assert
        Assert.That(result, Is.EqualTo(null));
    }

    [Test]
    public async Task TaxPostalCodeRepository_GetPostalCodes()
    {
        //act
        var result = await _taxPostalCodeRepository.GetPostalCodeAsync();

        //assert
        Assert.That(result.Count(), Is.EqualTo(4));
        Assert.That(result.Where(o => o.Code.Equals("7441")).ToList().Count(), Is.EqualTo(1));
        Assert.That(result.Where(o => o.Code.Equals("A100")).ToList().Count(), Is.EqualTo(1));
        Assert.That(result.Where(o => o.Code.Equals("7000")).ToList().Count(), Is.EqualTo(1));
        Assert.That(result.Where(o => o.Code.Equals("1000")).ToList().Count(), Is.EqualTo(1));
        Assert.That(result.Where(o => o.Code.Equals("Invalid")).ToList().Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task TaxPostalCodeRepository_GetPostalCodeByCode()
    {
        //act
        var result = await _taxPostalCodeRepository.GetPostalCodeByCodeAsync("1000");

        //assert
        Assert.That(result.Code, Is.EqualTo("1000"));
        Assert.That(result.TaxCalculationType, Is.EqualTo(TaxType.Progressive));
    }

    [Test]
    public async Task TaxRecordRepository_GetRecords_ShouldReturnFiveRecords()
    {
        //act
        var result = await _taxRecordRepository.GetTaxRecordAsync();

        //assert
        Assert.That(result.Count(), Is.EqualTo(5));
    }

    [Test]
    public async Task TaxRecordRepository_AddRecords()
    {
        //arrange
        var taxRecord = new TaxRecord { PostalCode = new TaxPostalCode { Code = "A100", TaxCalculationType = TaxType.FlatValue }, AnnualIncome = 40, CalculatedTaxValue = 4 };

        //act
        _taxRecordRepository.Add(taxRecord);
        await _context.SaveChangesAsync();

        var result = await _taxRecordRepository.GetTaxRecordAsync(taxRecord.Id);

        //assert
        Assert.That(result.PostalCode, Is.EqualTo(taxRecord.PostalCode));
        Assert.That(result.AnnualIncome, Is.EqualTo(taxRecord.AnnualIncome));
        Assert.That(result.CalculatedTaxValue, Is.EqualTo(taxRecord.CalculatedTaxValue));
    }
}
