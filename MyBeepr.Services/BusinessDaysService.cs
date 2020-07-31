using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBeepr.Core.Models;
using MyBeepr.Core.Utilities;
using MyBeepr.Core.Repositories;
using MyBeepr.Core.Services;

namespace MyBeepr.Services
{
    public class BusinessDayService : IBusinessDaysService
    {
        private readonly IHolidayRepository _holidayRepository;

        public BusinessDayService(IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        public async Task<long> CalcBusinessDaysAsync(DateTime startDate, DateTime endDate)
        {
            var holidays = await _holidayRepository.ListAsync();

            // var workingDays = DayUtility.CalcWorkingDaysBetween(startDate, endDate);
            var workingDays = DayUtility.CalcWorkingDaysBetweenLinear(startDate, endDate);
			
            for (var year = startDate.Year; year <= endDate.Year; year++)
            {
                workingDays = holidays.Aggregate(workingDays, (current, holiday) => current - DayUtility.HolidayOverlapsWorkingDay(holiday, startDate, endDate, year));
            }

            return workingDays;
        }
    }
}