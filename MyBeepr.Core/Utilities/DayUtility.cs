using System;
using System.Linq;
using MyBeepr.Core.Models;

namespace MyBeepr.Core.Utilities
{
    public static class DayUtility
    {
        /// <summary>
        /// Function designed to return the number of working weekdays using a single pass calculation
        /// </summary>
        /// <param name="start">The starting date of the period</param>
        /// <param name="end">The ending date of the period</param>
        /// <returns>The number of working weekdays within the period as an int</returns>
        public static int CalcWorkingDaysBetween(DateTime start, DateTime end) {
            var fullPeriod = end.Subtract(start);
            
            // If the period is small, use a simple linear algorithm to extract working days
            if (fullPeriod.TotalDays < 7)
            {
                return CalcWorkingDaysBetweenLinear(start, end);
            }
            
            // Use constant time algorithm for large date ranges 
            return CalcWorkingDaysBetweenConstant(start, end);
        }

        private static int CalcWorkingDaysBetweenConstant(DateTime start, DateTime end)
        {
            var shiftedEnd = end.AddDays(-1);
            var dayBetweenExact = shiftedEnd.Subtract(start.AddDays(1)).Days + 1;
                
            // Grab the days left and days progressed in the week
            var startDaysLeftInWeek = 7 - ((int) start.DayOfWeek + 1);
            var endDaysProgressedInWeek = (int) shiftedEnd.DayOfWeek + 1;

            // Calculate the number of full weeks in between the start and end period (removing the ends to get exact weeks)
            var weeksBetween = (dayBetweenExact - startDaysLeftInWeek - endDaysProgressedInWeek) / 7;

            // Conditionally remove saturday and sunday from the edge weeks
            var workingDaysAtStart = startDaysLeftInWeek 
                                     - (startDaysLeftInWeek > 0 ? 1 : 0)
                                     - (startDaysLeftInWeek == 7 ? 1 : 0);
            
            var workingDaysAtEnd = endDaysProgressedInWeek 
                                     - (endDaysProgressedInWeek > 0 ? 1 : 0)
                                     - (endDaysProgressedInWeek == 7 ? 1 : 0);
            
            return (weeksBetween * 5) + workingDaysAtStart + workingDaysAtEnd;
        }
        
        /// <summary>
        /// Function designed to return the number of working weekdays using a linq expression
        /// Linear time algorithm that should be used for small date ranges
        /// </summary>
        /// <param name="start">The starting date of the period</param>
        /// <param name="end">The ending date of the period</param>
        /// <returns>The number of working weekdays within the period as an int</returns>
        public static int CalcWorkingDaysBetweenLinear(DateTime start, DateTime end) {
            var dayDifference = (int)end.AddDays(-1).Subtract(start).TotalDays;
            
            // Loop through the period and conditionally add the working days
            return Enumerable
                .Range(1, dayDifference >=0 ? dayDifference : 0)
                .Select(x => start.AddDays(x))
                .Count(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday);
        }

        private static bool OnWeekday(DateTime date)
        {
            return (int) date.DayOfWeek > 0 && (int) date.DayOfWeek < 6;
        }

        public static int HolidayOverlapsWorkingDay(Holiday holiday, DateTime start, DateTime end, int year)
        {
            switch (holiday.HolidayType)
            {
                case EHolidayType.FixedDate:
                    return fixedOnInPeriodWorkingDay(holiday, start, end, year);
                case EHolidayType.ShiftingDay:
                    return shiftingOnInPeriodWorkingDay(holiday, start, end, year);
                case EHolidayType.OccurrenceDay:
                    return occurrenceOnInPeriodWorkingDay(holiday, start, end, year);
                default:
                    return 0;
            }
        }

        private static int fixedOnInPeriodWorkingDay(Holiday holiday, DateTime start, DateTime end, int year)
        {
            var holidayDate = new DateTime(year, holiday.Month, holiday.Day ?? 0);
            return holidayDate > start && holidayDate < end && OnWeekday(holidayDate) ? 1 : 0;
        }
        
        private static int shiftingOnInPeriodWorkingDay(Holiday holiday, DateTime start, DateTime end, int year)
        {
            var holidayDate = new DateTime(year, holiday.Month, holiday.Day ?? 0);
            switch ((int)holidayDate.DayOfWeek)
            {
                case 0:
                    holidayDate = holidayDate.AddDays(1);
                    break;
                case 6:
                    holidayDate = holidayDate.AddDays(2);
                    break;
            }

            return holidayDate > start && holidayDate < end && OnWeekday(holidayDate) ? 1 : 0;
        }

        private static int occurrenceOnInPeriodWorkingDay(Holiday holiday, DateTime start, DateTime end, int year)
        {
            var holidayDate = new DateTime(year, holiday.Month, 1);
            var currentDayOfWeek = (int)holidayDate.DayOfWeek;
            if (currentDayOfWeek != holiday.DayOfWeek)
            {
                // Shift the day to the start of the next week
                if (holiday.DayOfWeek < currentDayOfWeek)
                {
                    holidayDate = holidayDate.AddDays(7 - currentDayOfWeek);
                }

                // shift the date to the desired day of the week
                holidayDate = holidayDate.AddDays(holiday.DayOfWeek ?? 0);
            }
            
            // Shift the date to the desired day in the month
            holidayDate = holidayDate.AddDays(7 * ((holiday.OccurrenceInMonth ?? 1) - 1));

            return holidayDate > start && holidayDate < end && OnWeekday(holidayDate) ? 1 : 0;
        }
    }
}