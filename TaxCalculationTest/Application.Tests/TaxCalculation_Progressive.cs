using Domain.Entities;

namespace TaxCalculationTest.Application.Tests;

public class TaxCalculation_Progressive
{
    private ITaxCalculationByType _taxCalculation;
    private List<TaxProgressiveRates> progressiveMatrix;
    private List<TaxProgressiveRates> progressiveMatrix_B;

    [SetUp]
    public void Setup()
    {
        var taxCalculationFactory = new TaxCalculationFactory();

        _taxCalculation = taxCalculationFactory.GetTaxCalculationType(TaxType.Progressive);

        progressiveMatrix = new List<TaxProgressiveRates>()
        {
            new TaxProgressiveRates{ Rate = 10, FromValue = 0, ToValue = 8350},
            new TaxProgressiveRates{ Rate = 15, FromValue = 8351, ToValue = 33950},
            new TaxProgressiveRates{ Rate = 25, FromValue = 33951, ToValue = 82250},
            new TaxProgressiveRates{ Rate = 28, FromValue = 82251, ToValue = 171550},
            new TaxProgressiveRates{ Rate = 33, FromValue = 171551, ToValue = 372950},
            new TaxProgressiveRates{ Rate = 35, FromValue = 372951, ToValue = null},
        };

        progressiveMatrix_B = new List<TaxProgressiveRates>()
        {
            new TaxProgressiveRates{ Rate = 10, FromValue = 0, ToValue = 20000},
            new TaxProgressiveRates{ Rate = 12, FromValue = 20001, ToValue = 80000},
            new TaxProgressiveRates{ Rate = 22, FromValue = 80001, ToValue = 175000},
            new TaxProgressiveRates{ Rate = 24, FromValue = 175001, ToValue = 330000},
            new TaxProgressiveRates{ Rate = 32, FromValue = 330001, ToValue = null},
        };
    }

    [Test]
    public void ProgressiveCalculation_EmptyProgressiveMatrix()
    {
        //arrange
        decimal annualIncome = 10000;

        //act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _taxCalculation.Execute(annualIncome, Enumerable.Empty<TaxConfigValue>(), Enumerable.Empty<TaxProgressiveRates>());
        });

        //assert
        Assert.That(ex.Message, Is.EqualTo("No Progressive Matrix Found"));
    }

    [Test]
    [TestCaseSource(nameof(InvalidMatrixTestData))]
    public void ProgressiveCalculation_InvalidProgressiveMatrix(List<TaxProgressiveRates> progressiveMatrix)
    {
        //arrange
        decimal annualIncome = 10000;

        //act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _taxCalculation.Execute(annualIncome, Enumerable.Empty<TaxConfigValue>(), progressiveMatrix);
        });

        //assert
        Assert.That(ex.Message, Is.EqualTo("Invalid Progressive Matrix Found"));
    }

    private static IEnumerable<List<TaxProgressiveRates>> InvalidMatrixTestData()
    {
        List<TaxProgressiveRates> progressiveMatrix_Invalid_OverlappingRange = new List<TaxProgressiveRates>()
        {
            new TaxProgressiveRates{ Rate = 10, FromValue = 0, ToValue = 20000},
            new TaxProgressiveRates{ Rate = 12, FromValue = 20001, ToValue = 80000},
            new TaxProgressiveRates { Rate = 22, FromValue = 80001, ToValue = 175000 },
            new TaxProgressiveRates { Rate = 24, FromValue = 160001, ToValue = 330000 },
            new TaxProgressiveRates { Rate = 32, FromValue = 330001, ToValue = null },
        };

        List<TaxProgressiveRates> progressiveMatrix_Invalid_UncoveredRange = new List<TaxProgressiveRates>()
        {
            new TaxProgressiveRates{ Rate = 10, FromValue = 0, ToValue = 20000},
            new TaxProgressiveRates{ Rate = 12, FromValue = 20001, ToValue = 80000},
            new TaxProgressiveRates{ Rate = 22, FromValue = 90001, ToValue = 175000},
            new TaxProgressiveRates{ Rate = 24, FromValue = 175001, ToValue = 330000},
            new TaxProgressiveRates{ Rate = 32, FromValue = 330001, ToValue = null},
        };

        List<TaxProgressiveRates> progressiveMatrix_Invalid_StartWithZero = new List<TaxProgressiveRates>()
        {
            new TaxProgressiveRates{ Rate = 10, FromValue = 1, ToValue = 20000},
            new TaxProgressiveRates{ Rate = 12, FromValue = 20001, ToValue = 80000},
            new TaxProgressiveRates{ Rate = 22, FromValue = 80001, ToValue = 175000},
            new TaxProgressiveRates{ Rate = 24, FromValue = 175001, ToValue = 330000},
            new TaxProgressiveRates{ Rate = 32, FromValue = 330001, ToValue = null},
        };

        return new[] { progressiveMatrix_Invalid_OverlappingRange, progressiveMatrix_Invalid_UncoveredRange, progressiveMatrix_Invalid_StartWithZero };
    }

    [TestCase(0, ExpectedResult = 0)]
    [TestCase(8349, ExpectedResult = 834.9)]
    [TestCase(33949, ExpectedResult = 4674.85)]
    [TestCase(82249, ExpectedResult = 16749.75)]
    [TestCase(390951, ExpectedResult = 114516.35)]
    public decimal? ProgressiveCalculation_ProgressiveMatrix(decimal annualIncome)
    {
        return _taxCalculation.Execute(annualIncome, Enumerable.Empty<TaxConfigValue>(), progressiveMatrix);
    }

    [TestCase(0, ExpectedResult = 0)]
    [TestCase(35000, ExpectedResult = 3800)]
    [TestCase(225000, ExpectedResult = 42100)]
    public decimal? ProgressiveCalculation_ProgressiveMatrixSecondData(decimal annualIncome)
    {
        return _taxCalculation.Execute(annualIncome, Enumerable.Empty<TaxConfigValue>(), progressiveMatrix_B);
    }
}
