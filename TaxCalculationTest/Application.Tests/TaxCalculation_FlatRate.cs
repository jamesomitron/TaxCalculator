namespace TaxCalculationTest.Application.Tests;

public class TaxCalculation_FlatRate
{

    private ITaxCalculationByType _taxCalculation;
    private IEnumerable<TaxConfigValue> taxConfigValues;
    private IEnumerable<TaxConfigValue> taxConfigValues_InvalidRate;

    [SetUp]
    public void Setup()
    {
        var taxCalculationFactory = new TaxCalculationFactory();

        _taxCalculation = taxCalculationFactory.GetTaxCalculationType(TaxType.FlatRate);

        taxConfigValues = new List<TaxConfigValue>()
        {
            new TaxConfigValue{ Item = "FlatRate", Value = (decimal)17.5 }
        };

        taxConfigValues_InvalidRate = new List<TaxConfigValue>()
        {
            new TaxConfigValue{Item = "FlatRate", Value = 0 }
        };
    }

    [Test]
    public void FlatRateCalculation_EmptyTaxRateMap()
    {
        //arrange
        decimal annualIncome = 10000;

        //act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _taxCalculation.Execute(annualIncome, Enumerable.Empty<TaxConfigValue>(), Enumerable.Empty<TaxProgressiveRates>());
        });

        //assert
        Assert.That(ex.Message, Is.EqualTo("No Tax Configuration Value Found"));
    }

    [Test]
    public void FlatRateCalculation_InvalidFlatRate()
    {
        //arrange
        decimal annualIncome = 10000;

        //act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _taxCalculation.Execute(annualIncome, taxConfigValues_InvalidRate, Enumerable.Empty<TaxProgressiveRates>());
        });

        //assert
        Assert.That(ex.Message, Is.EqualTo("Invalid Flat Rate"));
    }

    [Test]
    public void FlatRateCalculation_AnnualIncomeLessThanZero()
    {
        //arrange
        decimal annualIncome = -10000;

        //act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _taxCalculation.Execute(annualIncome, taxConfigValues, Enumerable.Empty<TaxProgressiveRates>());
        });

        //assert
        Assert.That(ex.Message, Is.EqualTo("Invalid Annual Income"));
    }

    [Test]
    public void FlatRateCalculation_AnnualIncomeEqualToZero()
    {
        //arrange
        decimal annualIncome = 0;

        //act
        var result = _taxCalculation.Execute(annualIncome, taxConfigValues, Enumerable.Empty<TaxProgressiveRates>());

        //assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void FlatRateCalculation_AnnualIncomeValid()
    {
        //arrange
        decimal annualIncome = 1000;

        //act
        var result = _taxCalculation.Execute(annualIncome, taxConfigValues, Enumerable.Empty<TaxProgressiveRates>());

        //assert
        Assert.That(result, Is.EqualTo(175));
    }

}