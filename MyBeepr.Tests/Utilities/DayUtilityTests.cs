using System;
using NUnit.Framework;
using MyBeepr.Core.Utilities;

namespace MyBeepr.Tests.Utilities
{
    [TestFixture]
    public class DayUtilityTests
    {
        private static object[] _dateTestCases =
        {
            new object[] { new DateTime(1014, 8, 7), new DateTime(2014, 8, 11), 260890}, // long duration test 1000 years

            new object[] { new DateTime(2014, 8, 7), new DateTime(2014, 8, 11), 1}, // provided test case 1
            new object[] { new DateTime(2014, 8, 13), new DateTime(2014, 8, 21), 5}, // provided test case 2
            
            new object[] {new DateTime(2014, 8, 21), new DateTime(2014, 8, 13), 0}, // Inverted days
            
            new object[] { new DateTime(2020, 6, 30), new DateTime(2020, 8, 1), 23}, // full month
            new object[] { new DateTime(2020, 2, 29), new DateTime(2020, 5, 1), 44}, // full 2 month

            new object[] { new DateTime(2020, 7, 12), new DateTime(2020, 7, 18), 5}, // sun to sat, full week
            new object[] { new DateTime(2020, 7, 11), new DateTime(2020, 7, 19), 5}, // sat to sun, full week
            
            new object[] { new DateTime(2020, 7, 8), new DateTime(2020, 7, 8), 0}, // same day
            new object[] { new DateTime(2020, 7, 8), new DateTime(2020, 7, 9), 0}, // following days
            
            new object[] { new DateTime(2020, 7, 8), new DateTime(2020, 7, 10), 1}, // one day between
            
            new object[] { new DateTime(2020, 7, 9), new DateTime(2020, 7, 11), 1}, // one day between
            new object[] { new DateTime(2020, 7, 10), new DateTime(2020, 7, 12), 0}, // one day between
            
            new object[] { new DateTime(2020, 7, 10), new DateTime(2020, 7, 13), 0}, // fri to mon over weekend
            new object[] { new DateTime(2020, 7, 13), new DateTime(2020, 7, 17), 3} // monday to friday
        };

        [TestCaseSource(nameof(_dateTestCases))]
        public void CalcWorkingDaysBetweenTests(DateTime startDate, DateTime endDate, long workingDays)
        {
            var result = DayUtility.CalcWorkingDaysBetween(startDate, endDate);
            Assert.AreEqual(workingDays, result);
        }
        
        [TestCaseSource(nameof(_dateTestCases))]
        public void CalcWorkingDaysBetweenLinearTests(DateTime startDate, DateTime endDate, long workingDays)
        {
            var result = DayUtility.CalcWorkingDaysBetweenLinear(startDate, endDate);
            Assert.AreEqual(workingDays, result);
        }
    }
}