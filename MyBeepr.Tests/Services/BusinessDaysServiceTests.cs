using System;
using System.Threading.Tasks;
using NUnit.Framework;
using MyBeepr.Core.Services;
using MyBeepr.Services;
using MyBeepr.Tests.Repositories;

namespace MyBeepr.Tests.Services
{
    [TestFixture]
    public class BusinessDaysServiceTests
    {
        private IBusinessDaysService _businessDayService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _businessDayService = new BusinessDayService(new HolidayRepository());
        }

        private static object[] _dateTestCases =
        {
            // Fixed day tests
            new object[] { new DateTime(2014, 4, 20), new DateTime(2014, 4, 30), 6}, // Friday
            new object[] { new DateTime(2015, 4, 20), new DateTime(2015, 4, 30), 7}, // Saturday
            
            // Shifting day tests
            new object[] { new DateTime(2012, 1, 20), new DateTime(2012, 1, 30), 4}, // Thursday
            new object[] { new DateTime(2013, 1, 20), new DateTime(2013, 1, 30), 6}, // Saturday
            new object[] { new DateTime(2014, 1, 20), new DateTime(2014, 1, 30), 6}, // Sunday
            new object[] { new DateTime(2015, 1, 20), new DateTime(2015, 1, 30), 6}, // Monday
            
            // Test shift outside of the period
            new object[] { new DateTime(2019, 1, 20), new DateTime(2019, 1, 27), 5},
            new object[] { new DateTime(2020, 1, 20), new DateTime(2020, 1, 27), 4}, 
            
            // Test shift into the period
            new object[] { new DateTime(2019, 1, 27), new DateTime(2019, 1, 30), 1},
            new object[] { new DateTime(2020, 1, 27), new DateTime(2020, 1, 30), 2},

            // Occurrence day tests
            new object[] { new DateTime(2019, 6, 1), new DateTime(2019, 6, 16), 9},
            new object[] { new DateTime(2020, 6, 1), new DateTime(2020, 6, 16), 9},
            
            new object[] { new DateTime(2019, 6, 1), new DateTime(2019, 6, 10), 5},
            new object[] { new DateTime(2020, 6, 1), new DateTime(2020, 6, 10), 5},

            // provided test cases
            new object[] { new DateTime(2014, 8, 7), new DateTime(2014, 8, 11), 1}, 
            new object[] { new DateTime(2014, 8, 13), new DateTime(2014, 8, 21), 5}
        };

        [TestCaseSource(nameof(_dateTestCases))]
        public async Task SimpleBusinessDayTests(DateTime startDate, DateTime endDate, long businessDays)
        {
            var result = await _businessDayService.CalcBusinessDaysAsync(startDate, endDate);
            Assert.AreEqual(businessDays, result);
        }
    }
}