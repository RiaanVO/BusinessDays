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

            var workingDays = DayUtility.CalcWorkingDaysBetween(startDate, endDate);
			
            for (var year = startDate.Year; year <= endDate.Year; year++)
            {
                foreach (var holiday in holidays)
                {
                    switch (holiday.HolidayType)
                    {
                        case EHolidayType.FixedDate:
                            if(DayUtility.fixedOnInPeriodWorkingDay(holiday, startDate, endDate, year)) workingDays -= 1;
                            break;
                        case EHolidayType.ShiftingDay: 
                            if(DayUtility.shiftingOnInPeriodWorkingDay(holiday, startDate, endDate, year)) workingDays -= 1;
                            break;
                        case EHolidayType.OccurrenceDay: 
                            if(DayUtility.occurrenceOnInPeriodWorkingDay(holiday, startDate, endDate, year)) workingDays -= 1;
                            break;
                    }
                }
            }

            return workingDays;
        }


    }
}