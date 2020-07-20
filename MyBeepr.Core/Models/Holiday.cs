namespace MyBeepr.Core.Models
{
    public class Holiday
    {
        public EHolidayType HolidayType { get; }
        public int? Day { get; }
        public int? DayOfWeek { get; }
        public int? OccurrenceInMonth { get; }
        public int Month { get; }

        public Holiday(EHolidayType holidayType, int? day, int? dayOfWeek, int? occurrenceInMonth, int month)
        {
            this.HolidayType = holidayType;
            this.Day = day;
            this.DayOfWeek = dayOfWeek;
            this.OccurrenceInMonth = occurrenceInMonth;
            this.Month = month;
        }
    }
}