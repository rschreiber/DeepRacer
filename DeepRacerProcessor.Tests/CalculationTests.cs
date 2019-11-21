using System;
using DeepRacerProcessor.Controllers;
using DeepRacerProcessor.Helpers;
using Xunit;

namespace DeepRacerProcessor.Tests
{
    public class CalculationTests
    {
        [Fact]
        public void TestGetDirectionBetweenTwoPoints()
        {

            var result = GeometryHelper.GetDirectionBetweenTwoPoints(new double[] {5, 7}, new double[] {8, 10});
            Assert.Equal(45, result);
        }
    }
}
