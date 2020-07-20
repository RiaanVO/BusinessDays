using System;
using MyBeepr.Core.Models;

namespace MyBeepr.Core.Utilities
{
    public static class DayUtility
    {
        public static int CalcWorkingDaysBetween(DateTime start, DateTime end) {
            // Shift end date back one to align the between
            var fullPeriod = end.Subtract(start);
            var workingDays = 0;

            // Return if they are the same day or following days
            if (fullPeriod.Days < 2) return workingDays;

            var shiftedStart = start.AddDays(1);
            var shiftedEnd = end.AddDays(-1);
			
            var startDow = (int) start.DayOfWeek + 1;
			
            var shiftedStartDow = (int) shiftedStart.DayOfWeek + 1;
            var shiftedEndDow = (int) shiftedEnd.DayOfWeek + 1;
			
            var dayBetweenExact = shiftedEnd.Subtract(shiftedStart).Days + 1;

            if (fullPeriod.Days < 7)
            {
                workingDays = dayBetweenExact;

                // Force the DOW to work over a 2 week period if it crosses a weekend
                var repositionedEnd = shiftedEndDow + (shiftedStartDow <= shiftedEndDow ? 0 : 7);
				
                // Check if a boundary falls on a saturday or sunday
                if (shiftedStartDow == 1 || repositionedEnd == 1 || repositionedEnd == 8) workingDays -= 1;
                if (shiftedStartDow == 7 || repositionedEnd == 7 || repositionedEnd == 14) workingDays -= 1;
				
                // Check if the period includes a saturday or sunday
                if (8 < repositionedEnd && 8 > shiftedStartDow) workingDays -= 1;
                if (7 < repositionedEnd && 7 > shiftedStartDow) workingDays -= 1;
            }
            else
            {
                // Grab the days left and days progressed in the week
                var startDliw = 7 - startDow;
                var endDpiw = shiftedEndDow;

                var weeksBetween = (dayBetweenExact - startDliw - endDpiw) / 7;
                workingDays = (weeksBetween * 5) + startDliw + endDpiw;
				
                // remove weekend days from the start week
                if (startDliw > 0) workingDays -= 1;
                if (startDliw == 7) workingDays -= 1;

                // remove weekend days from the end week
                if (endDpiw > 0) workingDays -= 1;
                if (endDpiw == 7) workingDays -= 1;
            }

            return workingDays;
        }

        private static bool OnWeekday(DateTime date)
        {
            return (int) date.DayOfWeek > 0 && (int) date.DayOfWeek < 6;
        }

        public static bool fixedOnInPeriodWorkingDay(Holiday holiday, DateTime start, DateTime end, int year)
        {
            var holidayDate = new DateTime(year, holiday.Month, holiday.Day ?? 0);
            return holidayDate > start && holidayDate < end && OnWeekday(holidayDate);
        }
        
        public static bool shiftingOnInPeriodWorkingDay(Holiday holiday, DateTime start, DateTime end, int year)
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

            return holidayDate > start && holidayDate < end && OnWeekday(holidayDate);
        }

        public static bool occurrenceOnInPeriodWorkingDay(Holiday holiday, DateTime start, DateTime end, int year)
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

            return holidayDate > start && holidayDate < end && OnWeekday(holidayDate);
        }
    }
}