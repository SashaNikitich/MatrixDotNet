using System;
using MatrixDotNet;
using MatrixDotNet.Extensions.Statistics;
using Xunit;

namespace MatrixDotNetTests
{
    public class StatisticsTests
    {
        [Fact]
        public void IntervalRowMeanTest_FindsIntervalRowMeanByMatrix5x4WhichReturns7Dot63_AssertMustBeTrue()
        {
            // Arrange
            Matrix<double> matrix = new[,]
            {
                { 5.7, 6.7,  6.2,  7.0  },
                { 6.7, 7.7,  7.2,  11.0 },
                { 7.7, 8.7,  8.2,  6.0  },
                { 8.7, 9.7,  9.2,  4.0  },
                { 9.7, 10.7, 10.2, 2.0  }
            };
            TableIntervals[] tables = { TableIntervals.Ni,TableIntervals.Xi };
            var intervals = new Intervals<double>(matrix,tables);
            const double expected = 7.63;
            
            // Act
            double actual = intervals.GetIntervalRowMean();
            
            // Assert
            Assert.True(expected - actual < 0.00001);
        }
    }
}