namespace TaxCalculationTest.Application.Tests;

public class TaxCalculation_FlatValue
{
    private ITaxCalculationByType _taxCalculation;
    private IEnumerable<TaxConfigValue> taxConfigValues;

    [SetUp]
    public void Setup()
    {
        var taxCalculationFactory = new TaxCalculationFactory();

        _taxCalculation = taxCalculationFactory.GetTaxCalculationType(TaxType.FlatValue);

        taxConfigValues = new List<TaxConfigValue>()
        {
            new TaxConfigValue{Item = "FlatValue", Value = 10000 },
            new TaxConfigValue{Item = "FlatValueRate", Value = 5 },
            new TaxConfigValue{Item = "MinimumAnnualIncome", Value = 200000 }
        };
    }

    [Test]
    public void FlatValueCalculation_EmptyTaxRateMap()
    {
        //arrange
        decimal annualIncome = 1000;

        //act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _taxCalculation.Execute(annualIncome, Enumerable.Empty<TaxConfigValue>(), Enumerable.Empty<TaxProgressiveRates>());
        });

        //assert
        Assert.That(ex.Message, Is.EqualTo("No Tax Configuration Value Found"));
    }

    [Test]
    public void FlatValueCalculation_NoFlatValueInTaxRateMap()
    {
        //arrange
        decimal annualIncome = 1000;

        taxConfigValues = new List<TaxConfigValue>()
        {
            new TaxConfigValue{Item = "FlatValueRate", Value = 5 },
            new TaxConfigValue{Item = "MinimumAnnualIncome", Value = 200000 }
        };

        //act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _taxCalculation.Execute(annualIncome, taxConfigValues, Enumerable.Empty<TaxProgressiveRates>());
        });

        //assert
        Assert.That(ex.Message, Is.EqualTo("No Flat Value Found"));
    }

    [Test]
    public void FlatValueCalculation_NoFlatValueRateInTaxRateMap()
    {
        //arrange
        decimal annualIncome = 1000;

        taxConfigValues = new List<TaxConfigValue>()
        {
            new TaxConfigValue{Item = "FlatValue", Value = 10000 },
            new TaxConfigValue{Item = "MinimumAnnualIncome", Value = 200000 }
        };

        //act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _taxCalculation.Execute(annualIncome, taxConfigValues, Enumerable.Empty<TaxProgressiveRates>());
        });

        //assert
        Assert.That(ex.Message, Is.EqualTo("No Flat Value Rate Found"));
    }

    [Test]
    public void FlatValueCalculation_NoMinimumAnnualIncomeInTaxRateMap()
    {
        //arrange
        decimal annualIncome = 1000;

        taxConfigValues = new List<TaxConfigValue>()
        {
            new TaxConfigValue{Item = "FlatValue", Value = 10000 },
            new TaxConfigValue{Item = "FlatValueRate", Value = 5 }
        };

        //act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _taxCalculation.Execute(annualIncome, taxConfigValues, Enumerable.Empty<TaxProgressiveRates>());
        });

        //assert
        Assert.That(ex.Message, Is.EqualTo("No Minimum Annual Income Found"));
    }

    [Test]
    public void FlatValueCalculation_AnnualIncomeEqualToZero()
    {
        //arrange
        decimal annualIncome = 0;

        //act
        var result = _taxCalculation.Execute(annualIncome, taxConfigValues, Enumerable.Empty<TaxProgressiveRates>());

        //assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void FlatValueCalculation_AnnualIncomeLessThanMinimumAnnualIncome()
    {
        //arrange 
        decimal annualIncome = 1000;

        //act
        var result = _taxCalculation.Execute(annualIncome, taxConfigValues, Enumerable.Empty<TaxProgressiveRates>());

        //assert
        Assert.That(result, Is.EqualTo(50));
    }

    [Test]
    public void FlatValueCalculation_AnnualIncomeGreaterThanMinimumAnnualIncome()
    {
        //arrange 
        decimal annualIncome = 210000;

        //act
        var result = _taxCalculation.Execute(annualIncome, taxConfigValues, Enumerable.Empty<TaxProgressiveRates>());

        //assert
        Assert.That(result, Is.EqualTo(10000));
    }
}
